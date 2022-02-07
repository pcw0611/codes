using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UniRx;
using UniRx.Triggers;
using UnityEngine.Video;

namespace Battle
{
	public class LethalmoveManager : MonoBehaviour
	{
		private enum State
		{
			Idle,
			ZoomStart,
			CutScene,
			ZoomEnd,
			Motion,
			End,
		}

		static public LethalmoveManager				Instance			{ get; private set; }

		[SerializeField] private GameObject			defaultBg;
		[SerializeField] private Animator			lethalMoveAnim;
		[SerializeField] private Camera				camera;
		[SerializeField] private GameObject			cutSceneObject;
		[SerializeField] private VideoPlayer		videoPlayer;
		[SerializeField] private Transform			effectArea;

		private Damage								damage;
		private Vector3								desiredPosition;
		private Vector3								cameraOriginPos;
		private List<IBattleCommandListener>		targetListners;
		private IDObjectStateListener				behaviourListener;
		private ILethalActionListener				lethalListener;

		private State								state;

		private int zoomMinSize						= 4;
		private int zoomMaxSize						= 5;

		public System.Action						onEndLethalmove		{ set; get; }

		public void Start()
		{
			state = State.Idle;
			videoPlayer.loopPointReached += OnEndCutScene;
			videoPlayer.targetTexture.Release();

			Instance = this;
			cameraOriginPos = camera.transform.position;

			// 필살기를 사용했을 때 타겟 오브젝트의 위치까지 이동
			this.UpdateAsObservable()
				.Where( _ => state == State.ZoomStart )
				.Subscribe( _ =>
				{
					desiredPosition.z = camera.transform.position.z;
					camera.transform.position = Vector3.Lerp( camera.transform.position, desiredPosition, Time.deltaTime * 3f );
					camera.orthographicSize = Mathf.Lerp( camera.orthographicSize, zoomMinSize, Time.deltaTime * 2f );

				// 타겟 오브젝트 지점까지 도달하면 컷씬 재생
				if ( Vector3.Distance( camera.transform.position, desiredPosition ) < 0.01f )
					{
						cutSceneObject.SetActive( true );
						videoPlayer.frame = 0;
						videoPlayer.Play();
						state = State.CutScene;
					}
				} );

			// 필살기 사용 종료시 카메라 중앙으로 재조정
			this.UpdateAsObservable()
				.Where( _ => state == State.ZoomEnd )
				.Subscribe( _ =>
				{
					desiredPosition.z = camera.transform.position.z;
					camera.transform.position = Vector3.Lerp( camera.transform.position, desiredPosition, Time.deltaTime * 1f );
					camera.orthographicSize = Mathf.Lerp( camera.orthographicSize, zoomMaxSize, Time.deltaTime * 2f );

				// 타겟 오브젝트 지점까지 도달하면 End State로 변경
				if ( Vector3.Distance( camera.transform.position, desiredPosition ) < 0.2f )
					{
						camera.orthographicSize = zoomMaxSize;
						camera.transform.position = cameraOriginPos;
						state = State.Motion;
						OnEndLethalmove();
					}
				} );

			// 모션 사용
			this.UpdateAsObservable()
				.Where( _ => state == State.Motion )
				.Subscribe( _ =>
				{
					EffectFactory.Create( (int)EffectID.Lethal, effectArea );

					foreach ( var listener in targetListners )
					{
						listener.OnHit( damage, null );
					}

					state = State.End;
					lethalListener.OnReachLethal_3();
					OnEndLethalmove();
				} );
		}

		public void OnUseLethalmove( DObject dObject, Damage damage, List<IBattleCommandListener> targetListeners,
			IDObjectStateListener behaviourListner )
		{
			this.damage = damage;
			this.targetListners = targetListeners;
			lethalListener = dObject as ILethalActionListener;
			( UIManager.Instance as IUIRequestListener ).OnUseLethalmove();
			lethalMoveAnim.gameObject.SetActive( true );
			lethalMoveAnim.SetTrigger( "onBlackout" );

			desiredPosition = dObject.Center.position;
			state = State.ZoomStart;

			lethalListener.OnReachLethal_1();
		}
		private void OnEndLethalmove()
		{
			( UIManager.Instance as IUIRequestListener ).OnEndLethalmove();
			lethalMoveAnim.SetTrigger( "onBlackoutOff" );

			if ( null != onEndLethalmove )
				onEndLethalmove();
		}
		private void OnEndCutScene( VideoPlayer vp )
		{
			cutSceneObject.SetActive( false );
			desiredPosition = cameraOriginPos;
			state = State.ZoomEnd;

			lethalListener.OnReachLethal_2();
		}
	}
}