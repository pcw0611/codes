using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Note : MonoBehaviour
{
	[SerializeField] Sprite[] normalsSps;
	[SerializeField] Sprite[] flickSps;
	[SerializeField] Transform rainRoot;
	[SerializeField] Transform noteRoot;
	[SerializeField] Transform amongRoot;
	[SerializeField] Transform flickRoot;

	private NoteType _noteType;
	private Note _previousNote;
	private Note _appearedLinkNote;
	private float _beat;
	private Lane _myLane;
	private Lane _linkLane;
	private Vector3 _velocity;
	private int _laneNum;
	private bool _hooked;
	private bool _isDead;
	private NoteKind _noteKind;

	public bool hooked => _hooked;
	public bool isStartNode => null == _previousNote;
	public Vector3 Velocity { get => _velocity; private set => _velocity = value; }
	public NoteType noteType => _noteType;
	public Lane linkLane => _linkLane;
	public Lane myLane => _myLane;
	public Note linkNote => _appearedLinkNote;
	public bool isDead => _isDead;
	public NoteKind noteKind => _noteKind;
		

	private bool isRainNote => _noteType == NoteType.LongStart || _noteType == NoteType.SlideAAmong || _noteType == NoteType.SlideBAmong;

	private bool isEndNote => _noteType == NoteType.LongEnd || _noteType == NoteType.LongEndFlick 
		|| _noteType == NoteType.SlideAEnd || _noteType == NoteType.SlideAEndFlick
		|| _noteType == NoteType.SlideBEnd || _noteType == NoteType.SlideBEndFlick
		|| _noteType == NoteType.Normal || _noteType == NoteType.Flick;

	private bool isNormalNote => _noteType == NoteType.Normal || _noteType == NoteType.LongStart 
		|| _noteType == NoteType.LongEnd || _noteType == NoteType.SlideAEnd || _noteType == NoteType.SlideBEnd
		|| ( _noteType == NoteType.SlideAAmong && ( null == _previousNote ) )
		|| (_noteType == NoteType.SlideBAmong && ( null == _previousNote ) );

	private bool isFlickNote => _noteType == NoteType.Flick || 
		_noteType == NoteType.LongEndFlick || _noteType == NoteType.SlideAEndFlick || _noteType == NoteType.SlideBEndFlick;

	private bool isAmongNote => ( _noteType == NoteType.SlideAAmong || _noteType == NoteType.SlideBAmong ) && null != _previousNote;

	public float remainingDist
	{
		get
		{
			return Vector3.Distance( transform.position, _myLane.goal.position );
		}
	}
	public float linkNoteRemainDist
	{
		get
		{
			if (linkNote)
			{
				return Vector3.Distance( linkNote.transform.position, _linkLane.goal.position );
			}
			else
			{
				return Vector3.Distance( _linkLane.start.position, _linkLane.goal.position );
			}
		}
	}

	private float distToLongEnd
	{
		get
		{
			if ( null == _appearedLinkNote )
			{
				return Vector3.Distance( transform.position, _linkLane.start.transform.position );
			}
			else
			{
				return Vector3.Distance( transform.position, _appearedLinkNote.transform.position );
			}
		}
	}

	public Note StartNote { get => _previousNote; set => _previousNote = value; }

	public void SetDefault( Lane mylane, SpawnInfo spawnInfo, Vector3 velocity )
	{
		_beat = spawnInfo.beat;
		_myLane = mylane;
		_laneNum = spawnInfo.laneNum;
		_noteType = spawnInfo.noteType;
		_velocity = velocity;

		_noteKind = (_noteType == NoteType.SlideAAmong 
			|| _noteType == NoteType.SlideAEnd || _noteType == NoteType.SlideAEndFlick )
		? NoteKind.SlideA : _noteKind;

		_noteKind = ( _noteType == NoteType.SlideBAmong
			|| _noteType == NoteType.SlideBEnd || _noteType == NoteType.SlideBEndFlick )
		? NoteKind.SlideB : _noteKind;

		_noteKind = ( _noteType == NoteType.LongEnd 
			|| _noteType == NoteType.LongEndFlick || _noteType == NoteType.LongStart || _noteType == NoteType.LongStartSkill)
		? NoteKind.Long : _noteKind;
	}

	public void SetLinkLane( Lane lane )
	{
		_linkLane = lane;
	}

	public void SetNodeSprite()
	{
		noteRoot.gameObject.SetActive( isNormalNote );
		if (isNormalNote)
		{
			var sp = noteRoot.GetComponentInChildren<SpriteRenderer>();
			sp.sprite = normalsSps[_laneNum];
		}

		amongRoot.gameObject.SetActive( isAmongNote );

		flickRoot.gameObject.SetActive( isFlickNote );
		if ( isFlickNote )
		{
			var sp = flickRoot.GetComponentInChildren<SpriteRenderer>();
			sp.sprite = flickSps[_laneNum];
		}
	}

	private void Update()
	{
		Move();
		StretchRain();
	}

	private void StretchRain()
	{
		if ( isRainNote )
		{
			rainRoot.LookAt( _appearedLinkNote ? _appearedLinkNote.transform : _linkLane.start );
			rainRoot.localScale = new Vector3( rainRoot.localScale.x, rainRoot.localScale.y, -distToLongEnd );
		}
	}

	private void Move()
	{
		if (hooked)
			return;

		transform.Translate( _velocity * Time.deltaTime );
	}

	public void Hook()
	{
		transform.parent = _myLane.goal;
		noteRoot.gameObject.SetActive( false );
		_hooked = true;
	}

	public float GetLinkDist()
	{
		return Vector3.Distance( _linkLane.goal.position, _appearedLinkNote.transform.position );
	}

	public void DestroyHookedStartNote()
	{
		if ( null == _previousNote )
			return;

		_previousNote.OnRemove();
	}

	public void OnRemove()
	{
		_myLane.UnHook();
		_isDead = true;
	}

	public void OnAppearLinkNote( Note note )
	{
		this._appearedLinkNote = note;
	}
}