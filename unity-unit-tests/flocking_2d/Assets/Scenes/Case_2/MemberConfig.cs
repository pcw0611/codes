using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MemberConfig : MonoBehaviour
{
	public float maxFOV = 180f;
	public float maxAcceleration;
	public float maxVelocity;

	public float wanderJitter;
	public float wanderRadius;
	public float wanderDistance;
	public float wanderPriority;

	public float cohesionRadius;
	public float cohesionPriority;
	
	public float alignmentRadius;
	public float alignmentPriority;
	
	public float seperationRadius;
	public float seperationPriority;

	public float avoidanceRadius;
	public float avoidancePriority;
}
