using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseSaber : Saber
{
	protected override void Start()
	{
#if !UNITY_EDITOR
		Destroy(gameObject);
#endif
	}
	protected override void Update()
	{
		Vector3 mousePos		= Input.mousePosition;
		Ray ray					= Camera.main.ScreenPointToRay(Input.mousePosition);
		transform.position		= ray.origin + ray.direction;

		base.Update();
	}
}
