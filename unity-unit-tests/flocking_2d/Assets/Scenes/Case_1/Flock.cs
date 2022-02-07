using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Flock : MonoBehaviour
{
	public float speed = 0.1f;
	float rotationSpeed = 4f;
	Vector3 averageHeading;
	Vector3 averagePosition;
	float neighbourDistance = 1f;
    // Start is called before the first frame update

	bool turning = false;
	void OnDrawGizmos()
	{
		Gizmos.color = new Color( 1, 0, 0 );
		Gizmos.DrawLine( transform.position, transform.position + transform.right * 2f );
	}	
	void Start()
    {
        speed = Random.Range( 0.1f, 0.4f);
    }
    void Update()
    {
		if( Vector3.Distance(transform.position, Vector3.zero) >= GlobalFlock.tankSize )
		{
			turning = true;
		}
		else
		{
			turning = false;
		}

		if(turning)
		{
			Vector3 direction = Vector3.zero - transform.position;
			direction = Vector3.Normalize(direction);
			float angle = Mathf.Atan2( direction.y, direction.x ) * Mathf.Rad2Deg;
			Quaternion rotation = Quaternion.AngleAxis(angle, Vector3.forward);
			transform.rotation = rotation;//Quaternion.Slerp( transform.rotation, rotation, rotationSpeed * Time.deltaTime );
		}

		if( Random.Range(0,5) < 1)
			ApplyRules();

        transform.Translate( Time.deltaTime * speed, 0, 0);
    }

	private void ApplyRules()
	{
		GameObject[] gos;
		gos = GlobalFlock.allFish;

		Vector3 vcentre = Vector3.zero;
		Vector3 vavoid = Vector3.zero;
		float gSpeed = 0.1f;

		Vector3 goalPos = GlobalFlock.goalPos;

		float dist;

		int groupSize = 0;
		foreach ( GameObject go in gos )
		{
			if( go != this.gameObject )
			{
				dist = Vector3.Distance(go.transform.position, this.transform.position);
				 
				if( dist <= neighbourDistance )
				{
					vcentre += go.transform.position;
					groupSize++;

					if( dist < 1f )
					{
						vavoid = vavoid + (this.transform.position - go.transform.position);
					}

					Flock anotherFlock = go.GetComponent<Flock>();
					gSpeed = gSpeed + anotherFlock.speed;
				}
			}
		}

		if(groupSize > 0 )
		{
			vcentre = vcentre/groupSize + (goalPos - this.transform.position);
			speed = gSpeed / groupSize;

			Vector3 dir = (vcentre + vavoid) - transform.position;
			if( dir != Vector3.zero )
			{
				//transform.rotation = Quaternion.Slerp( transform.rotation,
				//	Quaternion.LookRotation( dir ),
				//	rotationSpeed * Time.deltaTime );

				float angle = Mathf.Atan2( dir.y, dir.x ) * Mathf.Rad2Deg;
				Quaternion rotation = Quaternion.AngleAxis(angle, Vector3.forward);
				transform.rotation = rotation;// Quaternion.Slerp( transform.rotation, rotation, rotationSpeed * Time.deltaTime );
			}
		}
	}
}
