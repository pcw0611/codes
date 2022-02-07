using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Battle
{
	public class UIManager : MonoBehaviour, IUIRequestListener
	{
		static public UIManager Instance			{ get; private set; }

		[SerializeField] private Canvas				canvas;
		[SerializeField] private GaugeUI			gaugeUIPrefab;
		[SerializeField] private DamageUI			damageUIprefab;

		public void Start()
		{
			Instance = this;
		}

		void IUIRequestListener.OnRequestStatusBar( DObject dObject )
		{
			// HP ¹Ù »ý¼º
			GaugeUI gaugeUI				= Instantiate<GaugeUI>( gaugeUIPrefab, canvas.transform );
			Vector2 viewportPoint		= Camera.main.WorldToViewportPoint( dObject.Top.position );

			gaugeUI.GetComponent<RectTransform>().anchorMin = viewportPoint;
			gaugeUI.GetComponent<RectTransform>().anchorMax = viewportPoint;
			gaugeUI.OnCreate( dObject.HpRate );

			dObject.OnHit				+= gaugeUI.OnHit;
			dObject.OnProgressTP		+= gaugeUI.OnProgressTP;
			dObject.OnDead				+= gaugeUI.OnDead;
		}
		void IUIRequestListener.OnRequestDamageUI( DObject dObject, Damage damage )
		{
			DamageUI damageUI = Instantiate<DamageUI>( damageUIprefab, canvas.transform );
			damageUI.Set( damage.value );

			Vector2 viewportPoint = Camera.main.WorldToViewportPoint( dObject.Top.position );

			damageUI.GetComponent<RectTransform>().anchorMin = viewportPoint;
			damageUI.GetComponent<RectTransform>().anchorMax = viewportPoint;
		}
		void IUIRequestListener.OnUseLethalmove()
		{
			canvas.gameObject.SetActive( false );
		}
		void IUIRequestListener.OnEndLethalmove()
		{
			canvas.gameObject.SetActive( true );
		}
	}
}