using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Battle
{
	public interface IBattleCommandListener
	{
		void OnUpdate( BattleState battleState, IDObjectStateListener behaviourListner );
		void OnAttack( List<DObject> targets, IDObjectStateListener behaviourListner );
		void OnHit( Damage damage, IDObjectStateListener behaviourListner );
		void OnBattleEnd( IDObjectStateListener behaviourListner );
	}

	public interface IDObjectStateListener
	{
		void OnRequestAttack( DObject dObject, bool isSkill );
		void OnBattleEnd();
		void OnDie( DObject dObject );
	}

	public interface ILethalActionListener
	{
		void OnReachLethal_1();
		void OnReachLethal_2();
		void OnReachLethal_3();
	}

	public interface IUIRequestListener
	{
		void OnRequestStatusBar( DObject dObject );
		void OnRequestDamageUI( DObject dObject, Damage damage );
		void OnUseLethalmove();
		void OnEndLethalmove();
	}
}