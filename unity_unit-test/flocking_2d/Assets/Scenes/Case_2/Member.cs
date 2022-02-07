using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Member : MonoBehaviour
{
	public Vector3 velocity;
	public Vector3 acceleration;
	public Vector3 position;

	public Level level;
	public MemberConfig conf;

	Vector3 wanderTarget;

	bool turning;

	private void Start()
	{
		level = FindObjectOfType<Level>();
		conf = FindObjectOfType<MemberConfig>();

		position = transform.position;
		velocity = new Vector3(Random.Range(-3,3),Random.Range(-3,3),0);
	}


	private void Update()
	{
		if ( Vector3.Distance( transform.position, Vector3.zero ) >= GlobalFlock.tankSize )
		{
			turning = true;
		}
		else
		{
			turning = false;
		}

		acceleration = Separation();
		acceleration = Vector3.ClampMagnitude(acceleration, conf.maxAcceleration);

		velocity = velocity + acceleration * Time.deltaTime;
		velocity = Vector3.ClampMagnitude(velocity, conf.maxVelocity);

		
		//WrapAround(ref position, -level.bounds, level.bounds);

		if( Vector2.Distance( transform.position, Vector2.zero ) >= level.bounds )
		{
			
			Vector3 ranVec = new Vector3( Random.Range(-level.bounds, level.bounds)
				,Random.Range( -level.bounds, level.bounds ), 0f );

			velocity = ranVec - transform.position;
			velocity = velocity.normalized * Random.Range(0.1f, conf.maxVelocity );
		}

		position = position + velocity * Time.deltaTime;

		Vector3 direction = velocity;
		direction = Vector3.Normalize( direction );
		float angle = Mathf.Atan2( direction.y, direction.x ) * Mathf.Rad2Deg;
		Quaternion rotation = Quaternion.AngleAxis(angle, Vector3.forward);
		transform.rotation = rotation;

		transform.position = position;
	}

	protected Vector3 Wander()
	{
		float jitter = conf.wanderJitter * Time.deltaTime;
		wanderTarget += new Vector3( RandomBinomial() * jitter, RandomBinomial() * jitter, 0);
		wanderTarget = wanderTarget.normalized;
		wanderTarget *= conf.wanderRadius;
		Vector3 targetInLocalSpace = wanderTarget + new Vector3(0, conf.wanderDistance, 0);
		Vector3 targetInWorldSpace = transform.TransformPoint(targetInLocalSpace);
		targetInWorldSpace -= this.position;
		return targetInWorldSpace.normalized;
	}

	virtual protected Vector3 Combine()
	{
		Vector3 finalVec = conf.cohesionPriority * Cohesion() + conf.wanderPriority * Wander()
			+ conf.alignmentPriority * Alignment() + conf.seperationPriority * Separation();
		return finalVec;
	}

	Vector3 Alignment()
	{
		Vector3 alignVector = new Vector3();
		var members = level.GetNeighbors(this, conf.alignmentRadius);
		if(members.Count == 0 )
			return alignVector;

		foreach ( var member in members )
		{
			if(isInFOV(member.position))
			{
				alignVector += member.velocity;
			}
		}

		return alignVector.normalized;
	}

	Vector3 Separation()
	{
		Vector3 separateVector = new Vector3();
		var members = level.GetNeighbors(this, conf.seperationRadius);
		if(members.Count == 0)
			return separateVector;

		foreach ( var member in members )
		{
			if(isInFOV(member.position))
			{
				Vector3 movingTowards = this.position - member.position;

				if( movingTowards.magnitude > 0 )
				{
					separateVector += movingTowards.normalized / movingTowards.magnitude;
				}
			}
		}

		return separateVector.normalized;
	}

	Vector3 Cohesion()
	{
		Vector3 cohesionVector = new Vector3();
		int countMembers = 0;

		var neighbors = level.GetNeighbors(this, conf.cohesionRadius);
		if(neighbors.Count == 0)
			return cohesionVector;

		foreach(var member in neighbors)
		{
			if(isInFOV(member.position))
			{
				cohesionVector += member.position;
				countMembers++;
			}
		}
		if(countMembers == 0)
			return cohesionVector;

		cohesionVector /= countMembers;
		cohesionVector = cohesionVector - this.position;
		cohesionVector = Vector3.Normalize(cohesionVector);
		return cohesionVector;
	}

	void WrapAround(ref Vector3 vector, float min, float max)
	{
		vector.x = WrapAroundFloat(vector.x, min, max);
		vector.y = WrapAroundFloat(vector.y, min, max);
		vector.z = WrapAroundFloat(vector.z, min, max);
	}

	float WrapAroundFloat(float value, float min, float max)
	{
		if( value > max )
			value = min;
		else if( value < min )
			value= max;

		return value;
	}

	float RandomBinomial()
	{
		return Random.Range(0f, 1f) - Random.Range(0f, 1f);
	}

	bool isInFOV(Vector3 vec)
	{
		return Vector3.Angle(this.velocity, vec - this.position) <= conf.maxFOV;
	}
}
