using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lane : MonoBehaviour
{
	[SerializeField] private int _laneNum;
	[SerializeField] private Transform _start;
	[SerializeField] private Transform _goal;
	[SerializeField] private Effect _mainEffect;
	[SerializeField] private Effect _subEffect;
	[SerializeField] private Config _config;
	private Note _hookedNote;
	private bool _hooked;
	private float _linkNoteRemainingDistFirst;
	private Vector3 originGoalPos;

	private List<Note> notes = new List<Note>();

	public Transform goal => _goal;
	public Transform start => _start;
	public float dist => Vector3.Distance( _start.position,  _goal.position );
	public Vector3 dir => ( originGoalPos - _start.position ).normalized;

	private void Awake()
	{
		originGoalPos = goal.position;
	}

	private void Update()
	{
		Move();
		CheckNotes();
	}

	private void Move()
	{
		if (!_hooked)
			return;

		goal.position = Vector3.Lerp( 
			originGoalPos, 
			_hookedNote.linkLane.goal.position,  
			1f - (_hookedNote.linkNoteRemainDist / _linkNoteRemainingDistFirst));
	}

	public void Push( Note note )
	{
		notes.Add( note );
	}

	private void CheckNotes()
	{
		for ( int i = 0; i < notes.Count; ++i )
		{
			if(notes[i].isDead)
			{
				Destroy( notes[i].gameObject );
				notes.Remove(notes[i]);
				continue;
			}

			float dist = Vector3.Distance(_goal.position, notes[i].transform.position);

			var dir = _goal.position - notes[i].transform.position;

			if ( dir.z > 0 )
			{
				notes[i].transform.position = _goal.position;
				var noteType = notes[i].noteType;
				switch ( noteType )
				{
					case NoteType.Normal:
					{
						Destroy( notes[i].gameObject );
						notes.Remove( notes[i] );

						_mainEffect.Active( EffectType.Normal );
					}
					break;

					case NoteType.Flick:
					{
						Destroy( notes[i].gameObject );
						notes.Remove( notes[i] );

						_mainEffect.Active( EffectType.Flick );
					}
					break;

					case NoteType.LongStart:
					case NoteType.SlideAAmong:
					case NoteType.SlideBAmong:
					{
						if(_hooked)
							break;

						_hooked = true;
						notes[i].Hook();
						_linkNoteRemainingDistFirst = notes[i].linkNoteRemainDist;
						_hookedNote = notes[i];

						notes[i].DestroyHookedStartNote();

						_mainEffect.Active( EffectType.Slide );
						_subEffect.Active( EffectType.Normal );
					}
					break;

					case NoteType.LongEndFlick:
					case NoteType.SlideAEndFlick:
					case NoteType.SlideBEndFlick:
					{
						notes[i].DestroyHookedStartNote();
						Destroy( notes[i].gameObject );
						notes.Remove( notes[i] );

						_subEffect.Active( EffectType.Flick );
					}
					break;

					case NoteType.LongEnd:
					case NoteType.SlideAEnd:
					case NoteType.SlideBEnd:
					{
						notes[i].DestroyHookedStartNote();
						Destroy( notes[i].gameObject );
						notes.Remove( notes[i] );

						_subEffect.Active( EffectType.Normal );
					}
					break;
				}
				
				Main.instance.AddCombo();
				SoundManager.instance.PlaySE();
			}
		}
	}

	public void UnHook()
	{
		_hooked = false;
		goal.position = originGoalPos;

		_mainEffect.Disable();
		_subEffect.Disable();
	}
}
