using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Spine.Unity;
using UniRx.Triggers;
using UniRx;

namespace Battle
{
	public class DObject : MonoBehaviour, IBattleCommandListener, ILethalActionListener
	{
		[SerializeField] private Transform				center;
		[SerializeField] private Transform				top;
		[SerializeField] private Animator				animator;
		[SerializeField] private SkeletonEvent			skeletonEvent;
		[SerializeField] private SkeletonAnimator		skeletonAnimator;

		[SerializeField] int							damageValue			= 10;
		[SerializeField] float							maxTurnPoint		= 100f;

		private bool									isHitEffect;
		private bool									isCreateEffect		= true;
		private int										maxhp				= 100;
		private int										hp;
		private int										sp;
		protected int									maxSp				= 100;
		protected float									turnPoint;

		public System.Action							OnAttack		{ set; get; }
		public System.Action<float>						OnHit			{ set; get; }
		public System.Action<float>						OnProgressTP	{ set; get; }
		public System.Action							OnDead			{ set; get; }

		public float									HpRate			{ get { return (float)hp / maxhp; } }
		public Transform								Top				{ get { return top; } }
		public Transform								Center			{ get { return center; } }
		public OwnerType								OwnerType		{ set; get; }
		public bool										IsDead			{ get { return hp <= 0; } }

		protected void Start()
		{
			hp = maxhp;

			( UIManager.Instance as IUIRequestListener ).OnRequestStatusBar( this );

			this.UpdateAsObservable()
				.Where( _ => isCreateEffect )
				.Subscribe( _ =>
				{
					skeletonAnimator.skeleton.a = Mathf.Clamp( Mathf.Lerp( skeletonAnimator.skeleton.a, 1f, Time.deltaTime * 3f ), 0f, 1f );

					if ( skeletonAnimator.skeleton.a >= 1f )
						isCreateEffect = false;
				} );

			this.UpdateAsObservable()
				.Where( _ => isHitEffect )
				.Subscribe( _ =>
				{
					skeletonAnimator.skeleton.g = Mathf.Clamp( Mathf.Lerp( skeletonAnimator.skeleton.g, 1f, Time.deltaTime * 3f ), 0f, 1f );
					skeletonAnimator.skeleton.b = Mathf.Clamp( Mathf.Lerp( skeletonAnimator.skeleton.b, 1f, Time.deltaTime * 3f ), 0f, 1f );

					if ( skeletonAnimator.skeleton.g >= 1f )
						isHitEffect = false;
				} );
		}

		void IBattleCommandListener.OnUpdate( BattleState battleState, IDObjectStateListener dObjectStateListener )
		{
			switch ( battleState )
			{
				case BattleState.Begin:
					break;
				case BattleState.NextRound:
					break;
				case BattleState.Waiting:
					if ( hp <= 0 )
						return;

					UnityEngine.Random.InitState( System.DateTime.Now.Millisecond );
					turnPoint += Time.deltaTime * UnityEngine.Random.Range( 2f, 3f );
					turnPoint = Mathf.Clamp( turnPoint, 0, maxTurnPoint );

					if ( turnPoint >= maxTurnPoint )
					{
						dObjectStateListener.OnRequestAttack( this, sp >= 50 );
					}

					OnProgressTP( turnPoint / maxTurnPoint );
					break;
				case BattleState.Battle:
					break;
				case BattleState.End:
					break;
				default:
					break;
			}
		}
		void IBattleCommandListener.OnAttack( List<DObject> targets, IDObjectStateListener dObjectStateListener )
		{
			if ( null == targets )
				return;

			Damage damage;

			if ( sp >= 50 )
			{
				damage = new Damage( UnityEngine.Random
					.Range( damageValue * 20, (int)( (float)damageValue * 25 ) ), EffectID.Hit_1 );

				List<IBattleCommandListener> battleListeners = new List<IBattleCommandListener>();
				for ( int i = 0; i < targets.Count; ++i )
					battleListeners.Add( targets[i] as IBattleCommandListener );

				turnPoint = 0;
				sp = Mathf.Clamp( sp - 50, 0, maxSp );
				LethalmoveManager.Instance.OnUseLethalmove( this, damage, battleListeners, dObjectStateListener );
				LethalmoveManager.Instance.onEndLethalmove = dObjectStateListener.OnBattleEnd;
			}
			else
			{
				animator.SetTrigger( "onAttack" );
				skeletonEvent.onAttack = () =>
				{
					turnPoint = 0;
					damage = new Damage( UnityEngine.Random
						.Range( damageValue, (int)( (float)damageValue * 1.2f ) ), EffectID.Hit_1 );

					( targets[0] as IBattleCommandListener ).OnHit( damage, dObjectStateListener );

					sp = Mathf.Clamp( sp + 10, 0, maxSp );
				};
			}
		}
		void IBattleCommandListener.OnHit( Damage damage, IDObjectStateListener dObjectStateListener )
		{
			skeletonAnimator.skeleton.g = 0f;
			skeletonAnimator.skeleton.b = 0f;

			isHitEffect = true;

			hp = Mathf.Clamp( hp - damage.value, 0, maxhp );

			if ( null != OnHit )
				OnHit( HpRate );

			if ( null != dObjectStateListener )
				dObjectStateListener.OnBattleEnd();

			EffectFactory.Create( (int)damage.effectID, center );
			( UIManager.Instance as IUIRequestListener ).OnRequestDamageUI( this, damage );
		}
		void IBattleCommandListener.OnBattleEnd( IDObjectStateListener dObjectStateListener )
		{
			if ( hp <= 0 )
			{
				animator.SetTrigger( "onDie" );

				this.UpdateAsObservable()
					.Where( _ => animator.GetCurrentAnimatorClipInfo( 0 )[0].clip.name == "Die" )
					.Where( _ => animator.GetCurrentAnimatorStateInfo( 0 ).normalizedTime > 1 )
					.First()
					.Subscribe( _ =>
					{
						float destroyTime = animator.GetCurrentAnimatorClipInfo( 0 )[ 0 ].clip.length;

						Destroy( gameObject, destroyTime );

						Observable.Timer( TimeSpan.FromSeconds( destroyTime ) )
						.Subscribe( _ =>
						{
							if ( null != OnDead )
								OnDead();

							dObjectStateListener.OnDie( this );
						} );
					} );
			}
		}

		void ILethalActionListener.OnReachLethal_1()
		{
			animator.SetTrigger( "onLethal_1" );
		}
		void ILethalActionListener.OnReachLethal_2()
		{
			animator.SetTrigger( "onLethal_2" );
		}
		void ILethalActionListener.OnReachLethal_3()
		{
			animator.SetTrigger( "onLethal_3" );
		}
	}
}
