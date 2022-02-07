using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
	public GameObject[] cubes;
	public Transform[] points;
	public float beat = (60/130)*2;
	private float timer;

	private void Start()
	{
		
	}

	private void Update()
	{
		if(timer>beat)
		{
			GameObject cube = Instantiate( cubes[Random.Range( 0, 2 )], points[Random.Range( 0, 4 )]);
			cube.transform.localPosition = Vector3.zero;
			cube.transform.Rotate( transform.forward, 90 * Random.Range( 0, 4 ) );
			timer -= beat;
		}

		timer += Time.deltaTime;
	}
}
