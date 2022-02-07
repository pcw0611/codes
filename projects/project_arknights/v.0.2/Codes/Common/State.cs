using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class State
{
	public System.Action	onTransition	{ set; get; }
	public System.Action	onUpdate		{ set; get; }

	public void OnTransition()
	{
		if ( null != onTransition )
			onTransition();
	}
	public void Update()
	{
		if( null != onUpdate )
			onUpdate();
	}
}