using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Damage
{
	public int			value		{ private set; get; }
	public EffectID		effectID	{ private set; get; }

	public Damage( int value, EffectID effectID )
	{
		this.value = value;
		this.effectID = effectID;
	}
}
