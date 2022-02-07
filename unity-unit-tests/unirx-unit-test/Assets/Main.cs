using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Main : MonoBehaviour, IDoubleClickListener
{
	public void Awake()
	{
		DoubleClickDetector.AddListener(this);
	}

	public void OnDoubleClick()
	{
		Debug.Log("Doubleclick detected!");
	}
}