using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Effect : MonoBehaviour
{
	[SerializeField] private Sprite[] normalSprites;
	[SerializeField] private Sprite[] slideSprites;
	[SerializeField] private Sprite[] flickSprites;
	[SerializeField] private SpriteRenderer spriteRenderer;

	private Sprite[] _currSprites;
	private EffectType _effectType;

	private int frame;
	private int maxFrame;

	public void Active(EffectType effectType)
	{
		this._effectType = effectType;

		frame = 0;

		switch ( effectType )
		{
			case EffectType.Normal:
				_currSprites = normalSprites; break;
			case EffectType.Flick:
				_currSprites = flickSprites; break;
			case EffectType.Slide:
				_currSprites = slideSprites; break;
		}

		maxFrame = _currSprites.Length - 1;
		gameObject.SetActive( true );

		FixedUpdate();
	}

	public void Disable()
	{
		frame = 0;
		gameObject.SetActive(false);
	}

	private void FixedUpdate()
	{
		if(!gameObject.activeSelf)
			return;

		spriteRenderer.sprite = _currSprites[frame];
		++frame;

		if( frame > maxFrame )
		{
			switch ( _effectType )
			{
				case EffectType.Normal:
				case EffectType.Flick:
					gameObject.SetActive( false );
					break;
				case EffectType.Slide:
					frame = 0;
					break;
			}
		}
	}
}
