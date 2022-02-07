using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
	private static SoundManager _instance;
	public static SoundManager instance => _instance;

	[SerializeField] private AudioClip[] _noteSes;
	[SerializeField] private AudioSource _audioSource;
	[SerializeField] private AudioSource _bgm;

	public void Awake()
	{
		_instance = this;
	}

	public void PlaySE()
	{
		_audioSource.clip = _noteSes[Random.Range( 0, _noteSes.Length )];
		_audioSource.Play();
	}

	public void PlayBgm()
	{
		_bgm.Play();
	}
}
