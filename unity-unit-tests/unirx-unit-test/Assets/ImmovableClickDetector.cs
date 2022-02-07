using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;

// 마우스가 움직이지 않는 상태에서 클릭 다운을 감지
// 드래그 이벤트와 클릭 이벤트를 동시에 쓰고 싶을 때 필요함
public class ImmovableClickDetector : MonoBehaviour
{
	public void Start()
	{
		var downStream = Observable.EveryUpdate().Where(_ => Input.GetMouseButtonDown(0)).Select(_ => Input.mousePosition);
		var upStream = Observable.EveryUpdate().Where(_ => Input.GetMouseButtonUp(0)).Select(_ => Input.mousePosition);

		downStream.Subscribe(_=> Debug.Log("마우스 클릭 다운 "));
		upStream.Subscribe( _ => Debug.Log( "마우스 클릭 업 " ) );
		downStream.Zip(upStream, (down, up) => down == up).Subscribe( samePos => 
		{ 
			if(samePos)
				Debug.Log( "마우스 위치 같음" );
		} );
	}
}
