using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Saber : MonoBehaviour
{
	public GameObject		saberTip;
	public GameObject		saberBase;
	public float			forceAppliedToCut;
	public LayerMask		layer;
	private Vector3			previousPos;
	private Vector3			triggerEnterTipPos;
	private Vector3			triggerEnterBasePos;
	private Vector3			triggerExitTipPos;
	private Vector3			triggerExitBasePos;

	protected virtual void Start()
	{
#if UNITY_EDITOR
		Destroy(gameObject);
#endif
	}
	// Update is called once per frame
	protected virtual void Update()
    {
		RaycastHit hit;
		if(Physics.Raycast(transform.position, transform.forward, out hit, 1, layer))
		{
			if(Vector3.Angle(transform.position - previousPos, hit.transform.up)> 130 )
			{
				Destroy( hit.transform.gameObject );
			}
		}
		previousPos = transform.position;
    }

	private void OnTriggerEnter( Collider other )
	{
		triggerEnterTipPos	= saberTip.transform.position;
		triggerEnterBasePos = saberBase.transform.position;
	}

	private void OnTriggerExit( Collider other )
	{
		triggerExitTipPos = saberTip.transform.position;

		//Create a triangle between the tip and base so that we can get the normal
		Vector3 side1 = triggerExitTipPos - triggerEnterTipPos;
		Vector3 side2 = triggerExitTipPos - triggerEnterBasePos;

		//Get the point perpendicular to the triangle above which is the normal
		//https://docs.unity3d.com/Manual/ComputingNormalPerpendicularVector.html
		Vector3 normal = Vector3.Cross(side1, side2).normalized;

		//Transform the normal so that it is aligned with the object we are slicing's transform.
		Vector3 transformedNormal = ((Vector3)(other.gameObject.transform.localToWorldMatrix.transpose * normal)).normalized;

		//Get the enter position relative to the object we're cutting's local transform
		Vector3 transformedStartingPoint = other.gameObject.transform.InverseTransformPoint(triggerEnterTipPos);

		Plane plane = new Plane();

		plane.SetNormalAndPosition(
				transformedNormal,
				transformedStartingPoint );

		var direction = Vector3.Dot(Vector3.up, transformedNormal);

		//Flip the plane so that we always know which side the positive mesh is on
		if ( direction < 0 )
		{
			plane = plane.flipped;
		}

		GameObject[] slices = Slicer.Slice(plane, other.gameObject);
		Destroy( other.gameObject );

		Rigidbody rigidbody = slices[1].GetComponent<Rigidbody>();
		Vector3 newNormal = transformedNormal + Vector3.up * forceAppliedToCut;
	}
}
 