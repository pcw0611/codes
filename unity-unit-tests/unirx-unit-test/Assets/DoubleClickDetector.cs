using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;

public interface IDoubleClickListener
{
	void OnDoubleClick();
}	

public class DoubleClickDetector : MonoBehaviour
{
	static List<IDoubleClickListener> _listeners = new List<IDoubleClickListener>();

	static public void AddListener(IDoubleClickListener doubleClickListener) => _listeners.Add( doubleClickListener );

	private void Start()
	{
		var clickStream = Observable.EveryUpdate()
		.Where(_ => Input.GetMouseButtonDown(0));

		clickStream.Buffer( clickStream.Throttle( System.TimeSpan.FromMilliseconds( 250 ) ) )
			.Where( xs => xs.Count >= 2 )
			.Subscribe(_ => _listeners.ForEach(i => i.OnDoubleClick()));
	}
}