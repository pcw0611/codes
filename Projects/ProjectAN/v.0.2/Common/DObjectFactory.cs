using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Battle
{
	public enum OwnerType
	{
		Own,
		Enemy,
	}
	public enum CharacterID
	{
		AMIYA_MELEE,
		MOSTIMIA,
	}
	public enum MobID
	{
		PROTOTYPE_1
	}

	public abstract class DObjectFactory : Factory
	{
		abstract public DObject Create( int ID, OwnerType ownerType, Transform transform );
	}
	public class CharacterFactory : DObjectFactory
	{
		public override DObject Create( int ID, OwnerType ownerType, Transform transform )
		{
			string path = "";

			switch ( (CharacterID)ID )
			{
				case CharacterID.AMIYA_MELEE: path = "Prefabs/Chars/Amiya_Melee"; break;
				case CharacterID.MOSTIMIA: path = "Prefabs/Chars/Mostimia"; break;
				default:
					return null;
			}

			DObject obj = Instantiate( Resources.Load<Character>( path ), transform );
			obj.OwnerType = ownerType;

			return obj;
		}
	}
	public class MobFactory : DObjectFactory
	{
		public override DObject Create( int ID, OwnerType ownerType, Transform transform )
		{
			string path = "";

			switch ( (MobID)ID )
			{
				case MobID.PROTOTYPE_1: path = "Prefabs/Mobs/Mob_PT1"; break;
				default:
					return null;
			}

			DObject obj = Instantiate( Resources.Load<Mob>( path ), transform );
			obj.OwnerType = ownerType;

			return obj;
		}
	}
}