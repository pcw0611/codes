using System.Collections;
using System.Collections.Generic;
using UnityEngine;
class Slicer
{
	public static GameObject[] Slice( Plane plane, GameObject objectToCut )
	{
		return new GameObject[2];
	}

	private static GameObject CreateMeshGameObject( GameObject originalObject )
	{
		return new GameObject();
	}
	private static void SetupCollidersAndRigidBodys( ref GameObject gameObject, Mesh mesh, bool useGravity )
	{
	}
}