using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UniRx;
using UniRx.Triggers;

public class RxButton : MonoBehaviour
{
	[SerializeField] private Button		button1;
	[SerializeField] private Button     button2;
	[SerializeField] private Text		text;

    // Start is called before the first frame update
    void Start()
    { 
		// * Stream Operators Examples

		// Buffer
		button1
			.OnClickAsObservable()
			.Buffer( 3 )
			.SubscribeToText( text, _ => "clicked" );
		//= Skip
		//button1
		//	.OnClickAsObservable()
		//	.Skip( 2 )
		//	.SubscribeToText( text, _ => "clicked" );

		// Zip
		// First + Repeat로 1회 동작할 때마다 스트림을 다시 만듬
		button1.OnClickAsObservable()
			.Zip( button2.OnClickAsObservable(), ( b1, b2 ) => "Clicked" )
			.First()
			.Repeat()
			.SubscribeToText( text, x => text.text + x + "\n" );

		// Filter (조건)
		// Map (메세지 변경)
		// SelectMany (새로운 스트림을 생성하고 그 스트림에 흐르는 메세지를 본래이 스트림의 메시지로 취급, FlatMap)
		// Throttle (쓰로틀, debounce) 도착한 때 최후의 메세지만 처리 (키를 엄청 눌러도 마지막 키만 처리함) 서버나 유저 클릭 실수들을 미연에 방지
		// Throttle 최초에 메세지가 올때부터 일정 시간 무시 (최초 메세지는 ㅏㅂㄷ음)
		// Delay (메세지 전달 연기)
		// DistinctUntilChanged (값이 변경될때만 통지) true / false 등을 처리
		// SkilUntil (지정한 스트림에 메세지가 올때까지 메세지를 스킵함
		// TakeUntil (메세지 보내던중 다른데서 메세지 오면 스트림 종료함)
		// Repeat
		// SkipUntil + TakUntil + Repeat 
		// 이벤트 A가 올때부터 이벤트 B가 올때까지 처리할 떄 사용 ex) 드래그로 오브젝트 회전

		// 실제 사용예
		// 1. 더블 클릭 판정
		// 2. 값의 변화 감시
		// 3. 값의 변화 가다듬기
		// 4. WWW를 사용하기 쉽게 하기
		// 5. 기존 라이브러리를 스트림으로 변환
	}
}