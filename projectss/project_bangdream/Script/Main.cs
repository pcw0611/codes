using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.Threading.Tasks;
using System.IO;
using UnityEngine.UI;

public class Main : MonoBehaviour
{
	static public Main instance { private set; get; }

	[SerializeField] private string scorePath;
	[SerializeField] private Config _config;
	[SerializeField] private NoteSpawner _noteSpawner;
	[SerializeField] private Text comboTxt;

	private int combo;
	private float _currBeat;
	private bool _isLoaded;
	private List<string> scoreTexts = new List<string>();

	public float currBeat => _currBeat;

	public async Task Awake()
	{
		instance = this;
		await Load(scorePath);
		_isLoaded = true;
	}

	private async Task Load( string path )
	{
		StreamReader reader = new StreamReader( path );

		while ( !reader.EndOfStream )
			scoreTexts.Add( await reader.ReadLineAsync() );

		scoreTexts.RemoveAt(0);
		_config.bpm = float.Parse(scoreTexts[0]);

		scoreTexts.RemoveAt(0);
		scoreTexts.RemoveAt(0);
		SoundManager.instance.PlayBgm();
		_noteSpawner.FillQueue(scoreTexts);

		reader.Close();
	}

	public void Update()
	{
		if (!_isLoaded)
			return;

		_currBeat += ( _config.bpm / 60f ) * Time.deltaTime;

		_noteSpawner.Spawn();
	}

	public void AddCombo()
	{
		++combo;

		comboTxt.text = combo + " combo";
	}
}
