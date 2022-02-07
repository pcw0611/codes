using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum NoteType
{
	Normal = 1,
	Flick = 2,

	Skill = 11,

	BPMChange = 20,

	LongStart = 21,
	LongEnd = 25,
	LongEndFlick = 26,
	LongStartSkill = 31,

	SlideAAmong = 4,
	SlideAEnd = 5,
	SlideAEndFlick = 12,

	SlideBAmong = 7,
	SlideBEnd = 8,
	SlideBEndFlick = 13,
}

public enum NoteKind
{
	Normal,
	SlideA,
	SlideB,
	Long,
}

public enum EffectType
{
	Normal,
	Slide,
	Flick,
}

public class SpawnInfo
{
	public float beat;
	public NoteType noteType;
	public int laneNum;
	public int linkLaneNum;

	public SpawnInfo( float beat, NoteType noteType, int laneNumExclusive )
	{ 
		Init(beat, noteType, laneNumExclusive);
	}
	public SpawnInfo( float beat, NoteType noteType, int laneNumExclusive, int linkLaneNumExclusive )
	{
		Init( beat, noteType, laneNumExclusive );

		this.linkLaneNum = linkLaneNumExclusive - 1;
	}

	private void Init( float beat, NoteType noteType, int laneNumExclusive )
	{
		this.beat = beat;
		this.noteType = noteType;
		this.laneNum = laneNumExclusive - 1; 
	}
}