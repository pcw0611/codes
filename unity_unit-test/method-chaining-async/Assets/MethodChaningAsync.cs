using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading.Tasks;
using Cysharp.Threading.Tasks;
using System;

public static class Exntensions
{
	public static async UniTask<MethodChaningAsync> Continue( this UniTask<MethodChaningAsync> prev, Func<float, UniTask<MethodChaningAsync>> func,
		float arg1 = 1f)
	{
		await prev;

		return await func(arg1);
	}

	public static async UniTask<MethodChaningAsync> Continue( this UniTask<MethodChaningAsync> prev, Func<UniTask<MethodChaningAsync>> func)
	{
		await prev;

		return await func();
	}
}

public class MethodChaningAsync : MonoBehaviour
{

	public async UniTask<MethodChaningAsync> Await( float delay )
	{
		Debug.Log( "Await" );
		await UniTask.Delay(TimeSpan.FromSeconds(delay));

		return await new UniTask<MethodChaningAsync>();
	}en 
	public async UniTask<MethodChaningAsync> Click()
	{
		while(true)
		{
			if ( Input.GetKeyDown( KeyCode.Mouse0 ) )
			{
				return await new UniTask<MethodChaningAsync>();
			}
			await UniTask.Yield();
		}
	}

	public async void Start()
	{
		await Await(1f)
			.Continue(Await, 2f)
			.Continue(Await, 3f)
			.Continue(Await, 4f)
			.Continue(Await, 5f)
			.Continue(Click);

		Debug.Log("³¡");
	}
}
