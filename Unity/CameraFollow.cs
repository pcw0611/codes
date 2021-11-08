using UnityEngine;

public class CameraFollow : MonoBehaviour
{
	[SerializeField] Transform	target;

	[SerializeField] float		smoothSpeed = 0.125f;
	[SerializeField] Vector3	offset;

	void LateUpdate()
	{
		Vector3 desiredPosition		= target.position + offset;
		Vector3 smoothedPosition	= Vector3.Lerp( transform.position, desiredPosition, smoothSpeed );

		transform.position = smoothedPosition;
	}
}
