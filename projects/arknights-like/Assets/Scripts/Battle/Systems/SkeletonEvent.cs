using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkeletonEvent : MonoBehaviour
{
	public System.Action onAttack { set; get; }

	void OnAttack()
	{
		if ( null != onAttack )
			onAttack();
	}
}
