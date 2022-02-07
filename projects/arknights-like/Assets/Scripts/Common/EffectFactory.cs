using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EffectID
{
	Hit_1,
	Hit_3,
	Lethal,
}

public class EffectFactory : Factory
{
	public static GameObject Create( int ID, Transform transform )
	{
		string path = "";

		switch ( (EffectID)ID )
		{
			case EffectID.Hit_1:		path = "Prefabs/Effects/Hit_1";		break;
			case EffectID.Hit_3:		path = "Prefabs/Effects/Hit_3";		break;
			case EffectID.Lethal:		path = "Prefabs/Effects/Lethal";	break;
			default:
				return null;
		}

		GameObject obj = Instantiate( Resources.Load<GameObject>( path ), transform );
		Destroy( obj.gameObject, 3f );

		return obj;
	}
}