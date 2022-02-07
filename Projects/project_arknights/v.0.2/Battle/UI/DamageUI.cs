using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Battle
{
	public class DamageUI : MonoBehaviour
	{
		[SerializeField] Text damageTxt;

		public void Set( int numbers )
		{
			damageTxt.text = numbers.ToString();
		}
		public void OnEnd()
		{
			Destroy( this.gameObject );
		}
	}
}