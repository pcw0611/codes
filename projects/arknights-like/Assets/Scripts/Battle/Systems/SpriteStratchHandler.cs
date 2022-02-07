using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpriteStratchHandler : MonoBehaviour
{
	public bool isAspectRatio;

    void Start()
    {
		transform.localScale	= Vector3.one;

		var topRightCorner		= Camera.main.ScreenToWorldPoint(
			new Vector3(Screen.width, Screen.height, Camera.main.transform.position.z));

		var worldSpaceWidth		= topRightCorner.x * 2;
		var worldSpaceHeight	= topRightCorner.y * 2;

		var spriteSize			= GetComponent<SpriteRenderer>().bounds.size;

		var scaleFactorX		= worldSpaceWidth / spriteSize.x;
		var scaleFactorY		= worldSpaceHeight / spriteSize.y;

		if( isAspectRatio )
		{
			if ( scaleFactorX > scaleFactorY )
				scaleFactorY = scaleFactorX;
			else
				scaleFactorX = scaleFactorY;
		}

		transform.localScale = new Vector3( scaleFactorX, scaleFactorY );
	}
}
