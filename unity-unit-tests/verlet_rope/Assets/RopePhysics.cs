using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class RopePhysics : MonoBehaviour
{
	public LineRenderer lineRenderer;
	public int segmentCount = 15;
	public int constraintLoop = 15;
	public float segmentLength = 0.1f;
	public float ropeWidth = 0.1f;
	public Vector2 gravity = new Vector2(0, -9.81f);
	[Space(10f)]
	public Transform startTransform;
	public Transform endTransform;

	List<Segment> segments = new List<Segment>();

	private void Reset() 
	{
		TryGetComponent(out lineRenderer);
	}

	private void Awake()
	{
		Vector2 segmentPos = startTransform.position;
		for ( int i = 0; i < segmentCount; i++ )
		{
			segments.Add(new Segment(segmentPos));
			segmentPos.y -= segmentLength;
		}
	}

	private void DrawRope()
	{

		lineRenderer.startWidth = ropeWidth;
		lineRenderer.endWidth = ropeWidth;
		Vector3[] segmentPositions = new Vector3[segments.Count];
		for ( int i = 0; i < segments.Count; i++ )
		{
			segmentPositions[i] = segments[i].position;
			
		} 

		lineRenderer.positionCount = segmentPositions.Length;
		lineRenderer.SetPositions(segmentPositions);
	}

	private void LateUpdate()
	{
	}

	public void OnDrawGizmos()
	{
		for ( int i = 0; i < segments.Count; i++ )
		{
			Gizmos.DrawSphere( segments[i].position + new Vector2(5, 0), 2f);
			Handles.Label(segments[i].position, "{" + i + "}" );
		}
	}

	private void FixedUpdate()
	{
		float dist = Vector3.Distance( startTransform.position, endTransform.position  );
		int newSegmentCount = Mathf.CeilToInt( dist / segmentLength );
		Vector2 segmentPos = startTransform.position;


		if ( newSegmentCount > segments.Count )
		{
			Vector2 endVec2 = new Vector2(endTransform.position.x, endTransform.position.y);
			Vector2 endDir = (endVec2 - segments[ segments.Count - 1].position).normalized;

			segments.Insert( 0, new Segment( segments[0].position ) );
		}
		if ( newSegmentCount < segments.Count )
		{
			segments.RemoveAt( segments.Count - 1 );
		}

		UpdateSegments();

		for ( int i = 0; i < constraintLoop; i++ )
		{
			ApplyConstraint();
			AdjustCollision();
		}
		DrawRope();
	}

	private void UpdateSegments()
	{
		for ( int i	= 0; i < segments.Count; i++ )
		{
			segments[i].velocity = segments[i].position - segments[i].previousPos;
			segments[i].previousPos = segments[i].position;
			segments[i].position += gravity * Time.fixedDeltaTime * Time.fixedDeltaTime;
			segments[i].position += segments[i].velocity;
		}
	}

	private void AdjustCollision()
	{
		for ( int i = 0; i < segments.Count; i++ )
		{
			Vector2 dir = segments[i].position - segments[i].previousPos;
			RaycastHit2D hit = Physics2D.CircleCast(segments[i].position, ropeWidth * 0.5f, 
				dir.normalized, 0f);

			if( hit)
			{
				segments[i].position = hit.point + hit.normal * ropeWidth * 0.5f;
				segments[i].previousPos = segments[i].position;
			}
		}
	}

	private void ApplyConstraint()
	{
		segments[0].position = startTransform.position;
		segments[segments.Count - 1].position = endTransform.position;
		for(int i = 0; i < segments.Count - 1; ++i)
		{
			float distance = (segments[i].position - segments[i + 1].position).magnitude;
			float difference = segmentLength - distance;
			Vector2 dir = (segments[i + 1].position - segments[i].position).normalized;

			Vector2 movement = dir * difference;

			if( i == 0)
				segments[i + 1].position += movement;
			else if (i  == segments.Count - 2)
				segments[i].position -=movement;
			else
			{
				segments[ i ].position -= movement * 0.5f;
				segments[i + 1].position += movement * 0.5f;
			}

		}
	}


	public class Segment
	{
		public Vector2 previousPos;
		public Vector2 position;
		public Vector2 velocity;

		public Segment(Vector2 _position)
		{
			previousPos = _position;
			position = _position;
			velocity = Vector2.zero;
		}
	}
}
