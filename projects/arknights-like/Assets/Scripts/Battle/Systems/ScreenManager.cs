using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScreenManager : MonoBehaviour
{
	[SerializeField] private Animator animator;

	public System.Action onEnd { set; get; }

	public void OnNextStage()
	{
		animator.SetTrigger( "onNext" );
	}
	void OnEnd()
	{
		if ( null != onEnd )
			onEnd();
	}
}