using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Threading.Tasks;

public class NoteSpawner : MonoBehaviour
{ 
	[SerializeField] private Config _config;
	[SerializeField] private Note _notePrefab;
	[SerializeField] private Lane[] _lanes;

	private List<SpawnInfo> _spawnInfoQueue = new List<SpawnInfo>();
	private List<Note> _unlinkedLsNotes = new List<Note>();
	private List<Note> _unlinkedSlNotes = new List<Note>();

	private void Awake()
	{
		//_spawnInfoQueue.Add(new SpawnInfo(1, NoteType.LongStart, 1, 3));
		//_spawnInfoQueue.Add( new SpawnInfo( 2, NoteType.LongStart, 5, 6 ) );
		//_spawnInfoQueue.Add(new SpawnInfo(5, NoteType.LongEnd, 3));
		//_spawnInfoQueue.Add( new SpawnInfo( 7, NoteType.LongEnd, 6 ) );
		//_spawnInfoQueue.Add( new SpawnInfo( 1, NoteType.Normal, 1 ) );
		//_spawnInfoQueue.Add( new SpawnInfo( 1.5f, NoteType.Flick, 3 ) );
		//_spawnInfoQueue.Add( new SpawnInfo( 1, NoteType.Normal, 5 ) );
		//_spawnInfoQueue.Add( new SpawnInfo( 2, NoteType.SlideAAmong, 7, 4 ) );
		//_spawnInfoQueue.Add( new SpawnInfo( 4, NoteType.SlideAAmong, 4, 4 ) );
		//_spawnInfoQueue.Add( new SpawnInfo( 5, NoteType.SlideAEnd, 4 ) );
		//_spawnInfoQueue.Add( new SpawnInfo( 6, NoteType.SlideAAmong, 7, 4 ) );
		//_spawnInfoQueue.Add( new SpawnInfo( 8, NoteType.SlideAAmong, 4, 4 ) );
		//_spawnInfoQueue.Add( new SpawnInfo( 9, NoteType.SlideAEndFlick, 4 ) );
		//_spawnInfoQueue.Add( new SpawnInfo( 10, NoteType.LongStart, 1, 2 ) );
		//_spawnInfoQueue.Add( new SpawnInfo( 12, NoteType.LongEndFlick, 2 ) );
	}


	public void Spawn()
	{
		if ( _spawnInfoQueue.Count <= 0 )
			return;

		var spawnInfo = _spawnInfoQueue[0];
		if ( Main.instance.currBeat + ( _config.goalTime * (_config.bpm / 60f)) >= spawnInfo.beat )
		{
			_spawnInfoQueue.RemoveAt( 0 );
			SpawnNote(spawnInfo);
		}
	}

	public void FillQueue(List<string> scoreTexts)
	{
		List<SpawnInfo> recentStartNotes = new List<SpawnInfo>();

		for ( int i = 0; i < scoreTexts.Count; i++ )
		{
			float beat = 0f;
			NoteType noteType = NoteType.Normal;
			int laneNumExclusive;

			int cursor = 0;
			string parsing = "";
			string text = scoreTexts[i];
			int cnt = 0;

			while (true)
			{
				if( cursor >= text.Length )
				{
					laneNumExclusive = int.Parse(parsing);

					var spawnInfo = new SpawnInfo(beat, noteType, laneNumExclusive);
					_spawnInfoQueue.Add( spawnInfo );

					Debug.Log(i + "  " + text);

					if ( noteType == NoteType.LongStart || noteType == NoteType.SlideAAmong || noteType == NoteType.SlideBAmong )
					{
						recentStartNotes.Add( spawnInfo );
					}
					if ( noteType == NoteType.LongEnd || noteType == NoteType.LongEndFlick )
					{
						var start = recentStartNotes.Find( a => a.noteType == NoteType.LongStart);
						start.linkLaneNum = laneNumExclusive - 1;
						recentStartNotes.Remove( start );
					}
					else if ( noteType == NoteType.SlideAAmong )
					{
						var start = recentStartNotes.Find( a => a.noteType == NoteType.SlideAAmong);
						if(start == spawnInfo)
							break;


						if ( null != start )
						{
							start.linkLaneNum = laneNumExclusive - 1;

							recentStartNotes.Remove( start );
						}
					}
					else if ( noteType == NoteType.SlideBAmong )
					{
						var start = recentStartNotes.Find( a => a.noteType == NoteType.SlideBAmong);
						if ( start == spawnInfo )
							break;

						if ( null != start )
						{
							start.linkLaneNum = laneNumExclusive - 1;

							recentStartNotes.Remove( start );
						}
					}
					else if ( noteType == NoteType.SlideAEnd || noteType == NoteType.SlideAEndFlick )
					{
						var start = recentStartNotes.FindLast( a => a.noteType == NoteType.SlideAAmong);
						start.linkLaneNum = laneNumExclusive - 1;
						recentStartNotes.Remove( start );
					}
					else if ( noteType == NoteType.SlideBEnd || noteType == NoteType.SlideBEndFlick )
					{
						var start = recentStartNotes.FindLast( a => a.noteType == NoteType.SlideBAmong);
						start.linkLaneNum = laneNumExclusive - 1;
						recentStartNotes.Remove( start );
					}

					break;
				}
				else if ( text[cursor] == '/' )
				{
					if ( cnt == 0 )
						beat = float.Parse( parsing );
					if ( cnt == 1 )
					{
						var sour = (NoteType)( int.Parse( parsing ) );
						sour = sour == NoteType.LongStartSkill ? NoteType.LongStart : sour;
						sour = sour == NoteType.Skill ? NoteType.Normal : sour;
						//sour = sour == NoteType.SlideBAmong ? NoteType.SlideAAmong: sour;
						//sour = sour == NoteType.SlideBEnd ? NoteType.SlideAEnd : sour;
						//sour = sour == NoteType.SlideBEndFlick ? NoteType.LongStart : sour;

						noteType = sour;

						if( noteType != NoteType.BPMChange && noteType != NoteType.Flick && noteType != NoteType.LongEnd
							 && noteType != NoteType.LongEndFlick && noteType != NoteType.LongStart && noteType != NoteType.LongStartSkill
							  && noteType != NoteType.Normal && noteType != NoteType.SlideAAmong && noteType != NoteType.SlideAEnd
							   && noteType != NoteType.SlideAEndFlick && noteType != NoteType.SlideBAmong && noteType != NoteType.SlideBEnd
								&& noteType != NoteType.SlideBEndFlick )
						{
							Debug.Log("없는 노드 타입 -" + i + " " + text );
						}
					}

					++cnt;
					++cursor;
					parsing = "";
				}
				else
				{
					parsing += text[cursor];
					++cursor;
				}
			}
		}
	}

	private void SpawnNote( SpawnInfo spawnInfo ) 
	{
		if (spawnInfo.noteType == NoteType.BPMChange)
		{
			_config.bpm = spawnInfo.laneNum + 1;
			return;
		}

		var lane = _lanes[spawnInfo.laneNum];
		var laneStart = lane.start;
		var laneGoal = lane.goal;
		var velocity = lane.dir * ( lane.dist / Mathf.Max( 0.1f, _config.goalTime ) );

		var newNote = Instantiate<Note>(_notePrefab, laneStart.position, Quaternion.identity);


		newNote.SetDefault( lane, spawnInfo, velocity );
		switch ( spawnInfo.noteType )
		{
			case NoteType.LongStart:	
				newNote.SetLinkLane(_lanes[spawnInfo.linkLaneNum]);
				_unlinkedLsNotes.Add( newNote );
				break;

			case NoteType.LongEnd:
			case NoteType.LongEndFlick:
				LinkPreviousNote(newNote, _unlinkedLsNotes);	
				break;

			case NoteType.SlideAAmong:
			case NoteType.SlideBAmong:
				LinkPreviousNote(newNote, _unlinkedSlNotes);
				newNote.SetLinkLane( _lanes[spawnInfo.linkLaneNum] );
				_unlinkedSlNotes.Add( newNote );
				break;

			case NoteType.SlideAEnd:
			case NoteType.SlideAEndFlick:
			case NoteType.SlideBEnd:
			case NoteType.SlideBEndFlick:
				LinkPreviousNote( newNote, _unlinkedSlNotes);		
				break;
		}
		newNote.SetNodeSprite();

		lane.Push(newNote);
	}

	public void LinkPreviousNote( Note pushNote, List<Note> unlinks )
	{
		if ( unlinks.Count <= 0 )
			return;

		foreach ( var unlink in unlinks )
		{
			if ( unlink.noteKind != pushNote.noteKind )
				continue;

			pushNote.StartNote = unlink;
			unlink.OnAppearLinkNote( pushNote );
			unlinks.Remove( unlink );
			break;
		}
	}
}
