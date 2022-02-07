using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Battle
{
	public class GaugeUI : MonoBehaviour
	{
		[SerializeField] private Slider				hpSlider;
		[SerializeField] private Image				tpCircleSlider;

		public void OnCreate( float hpRate )
		{
			hpSlider.value = hpRate;
		}
		public void OnProgressTP( float tpRate )
		{
			tpCircleSlider.fillAmount = tpRate;
		}
		public void OnHit( float hpRate )
		{
			hpSlider.value = hpRate;
		}
		public void OnDead()
		{
			Destroy( this.gameObject );
		}
	}
}