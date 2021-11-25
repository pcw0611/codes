using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Battle
{
	public class BattleManager : MonoBehaviour, IDObjectStateListener
	{
		private class Round
		{
			public int										round;
			public List<MobID>								mobIds = new List<MobID>();
		}

		[SerializeField] private ScreenManager				screenManager;
		[SerializeField] private Transform[]				Own_Spawn_Areas;
		[SerializeField] private Transform[]				Enemy_Spawn_Areas;

		private BattleState									currState		= BattleState.Begin;
		private Dictionary<BattleState, State>				states			= new Dictionary<BattleState, State>();
		private Queue<Round>								rounds			= new Queue<Round>();
		private List<DObject>								dObjects		= new List<DObject>();
		private List<DObject>								ownChars		= new List<DObject>();
		private List<DObject>								enemys			= new List<DObject>();

		private int											currRound;

		private System.Action								onStartRound;

		private void Start()
		{
			SetRounds();
			SetOnStartRound();
			SetBeginState();
			SetNextRoundState();
			SetWatingState();

			states.Add( BattleState.Battle, new State() );
			states.Add( BattleState.Win, new State() );
			states.Add( BattleState.Lose, new State() );

			Translate( BattleState.Begin );
		}
		private void Update()
		{
			states[currState].Update();
		}

		private void SetRounds()
		{
			Round round = new Round();
			round.mobIds.Add( MobID.PROTOTYPE_1 );
			rounds.Enqueue( round );

			round = new Round();
			round.mobIds.Add( MobID.PROTOTYPE_1 );
			round.mobIds.Add( MobID.PROTOTYPE_1 );
			rounds.Enqueue( round );

			round = new Round();
			round.mobIds.Add( MobID.PROTOTYPE_1 );
			round.mobIds.Add( MobID.PROTOTYPE_1 );
			round.mobIds.Add( MobID.PROTOTYPE_1 );
			rounds.Enqueue( round );

			currRound = 1;
		}
		private void SetOnStartRound()
		{
			System.Action onStartRound = () =>
			{
				DObjectFactory mobFactory   = new MobFactory();

				Round round = rounds.Dequeue();
				if ( null == round )
					return;

				for ( int i = 0; i < round.mobIds.Count; ++i )
				{
					DObject dObj =
					mobFactory.Create( (int)round.mobIds[ i ], OwnerType.Enemy, GetEmptyAreaFirst( Enemy_Spawn_Areas ) );
					dObjects.Add( dObj );
					enemys.Add( dObj as Mob );
				}
			};
		}
		private void SetBeginState()
		{
			states.Add( BattleState.Begin, new State() );
			{
				states[BattleState.Begin].onTransition = () =>
				{
					DObjectFactory charFactory  = new CharacterFactory();

					DObject dObj =
				charFactory.Create( (int)CharacterID.AMIYA_MELEE, OwnerType.Own, GetEmptyAreaFirst( Own_Spawn_Areas ) ) ;
					dObjects.Add( dObj );
					ownChars.Add( dObj as Character );

					onStartRound();
					Translate( BattleState.Waiting );
				};
			}
		}
		private void SetWatingState()
		{
			states.Add( BattleState.Waiting, new State() );
			{
				states[BattleState.Waiting].onUpdate = () =>
				{
					for ( int i = 0; i < dObjects.Count; ++i )
					{
						IBattleCommandListener listener = dObjects[ i ] as IBattleCommandListener;
						listener.OnUpdate( currState, this );
					}
				};
			}
		}
		private void SetNextRoundState()
		{
			states.Add( BattleState.NextRound, new State() );
			{
				states[BattleState.NextRound].onTransition = () =>
				{
					screenManager.OnNextStage();
					screenManager.onEnd = () =>
					{
						onStartRound();

						++currRound;
						Translate( BattleState.Waiting );
					};
				};
			}
		}

		private void Translate( BattleState state )
		{
			currState = state;
			State nextState = states[state];

			nextState.OnTransition();
		}
		private Transform GetEmptyAreaFirst( Transform[] areas )
		{
			foreach ( Transform transform in areas )
			{
				if ( transform.childCount <= 0 )
					return transform;
			}

			return null;
		}
		private List<DObject> GetTarget( DObject own, bool isSkill )
		{
			List<DObject> ret = new List<DObject>();

			if ( !isSkill )
			{
				DObject oneTarget;

				if ( own.OwnerType == OwnerType.Enemy )
				{
					oneTarget = ownChars.Count > 0 ?
					ownChars[Random.Range( 0, ownChars.Count )] : null;
				}
				else
				{
					oneTarget = enemys.Count > 0 ?
					enemys[Random.Range( 0, enemys.Count )] : null;
				}

				if ( oneTarget.IsDead )
					return new List<DObject>();

				ret.Add( oneTarget );
			}
			else
			{
				ret = own.OwnerType == OwnerType.Enemy ? ownChars : enemys;
			}

			return ret;
		}

		void IDObjectStateListener.OnRequestAttack( DObject dObject, bool isSkill )
		{
			if ( currState != BattleState.Waiting )
				return;

			List<DObject> targets = GetTarget( dObject, isSkill );
			if( targets.Count <= 0 )
				return;

			states[BattleState.Battle].onTransition = () =>
			{
				IBattleCommandListener battleListner = dObject as IBattleCommandListener;

				battleListner.OnAttack( targets, this );
			};

			Translate( BattleState.Battle );
		}
		void IDObjectStateListener.OnBattleEnd()
		{
			foreach ( var item in dObjects )
			{
				( item as IBattleCommandListener ).OnBattleEnd( this );
			}

			Translate( BattleState.Waiting );
		}
		void IDObjectStateListener.OnDie( DObject dObject )
		{
			dObjects.Remove( dObject );

			if ( dObject.OwnerType == OwnerType.Enemy )
				enemys.Remove( dObject );
			else
				ownChars.Remove( dObject );

			if ( enemys.Count <= 0 )
			{
				if ( rounds.Count <= 0 )
				{
					Translate( BattleState.Win );
					Debug.Log( "배틀 종료 - 승리" );
				}
				else
				{
					Translate( BattleState.NextRound );
				}
			}
			else if ( ownChars.Count <= 0 )
			{
				Translate( BattleState.Lose );
				Debug.Log( "배틀 종료 - 패배" );
			}
		}
	}
}