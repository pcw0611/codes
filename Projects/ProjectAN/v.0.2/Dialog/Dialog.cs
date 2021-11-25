using System.Text;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Dialog
{
	public class Dialog : MonoBehaviour, IScreenplayListener
	{
		[SerializeField] Image						image_Bg;
		[SerializeField] GameObject[]				chars;
		[SerializeField] string						speakerName;
		[SerializeField] string						context;

		[SerializeField] private Text				text_SpeakerName;
		[SerializeField] private Text				text_Context;

		[SerializeField] private float				flowSpeed;
		[SerializeField] private int				lineBreakInterval;

		private string								viewContext;

		private Screenplayer						screenplayer;

		private void Start()
		{
			screenplayer = Screenplayer.Load(
				"Assets/Resources/Dialog/Screenplays/screenplay_test.txt"
				, this );
		}
		private void Update()
		{
			if ( Input.GetKeyDown( KeyCode.Mouse0 ) )
			{
				screenplayer?.NextManually();
			}
		}
		private IEnumerator PlayTalk()
		{
			viewContext = "";
			text_SpeakerName.text = speakerName;

			byte[] bytes;
			int byteLegth = 0;

			for ( int i = 0; i < context.Length; ++i )
			{
				viewContext += context[i];
				text_Context.text = viewContext;

				if ( context[i] == ' ' )
					byteLegth += 1;
				else
					byteLegth += 2;

				if ( byteLegth >= lineBreakInterval )
				{
					byteLegth = 0;
					viewContext += "\n";

					if ( context[i + 1] == ' ' && i + 1 < context.Length )
						context = context.Remove( i + 1, 1 );
				}

				yield return new WaitForSeconds( flowSpeed );
			}

			yield break;
		}

		void IScreenplayListener.OnCommandBg( string name )
		{
			Sprite sp = Resources.Load<Sprite>( "Dialog/Bgs/" +name );
			image_Bg.sprite = sp;
		}
		void IScreenplayListener.OnCommandCharacter( CharCommandInfo charCmdInfo )
		{
			for ( int i = 0; i < chars.Length; ++i )
			{
				bool isValid = i < charCmdInfo.names.Count;

				chars[i].SetActive( isValid );

				if ( isValid )
				{
					Image img = chars[ i ].GetComponentInChildren<Image>();

					if ( null != img )
					{
						Sprite sp = Resources.Load<Sprite>( "Dialog/Chars/" +charCmdInfo.names[ i ] );
						img.sprite = sp;

						float focusColor = ( i + 1 ) == charCmdInfo.focus ? 1f : 0.32f;
						img.color = new Color( focusColor, focusColor, focusColor );
					}
				}
			}
		}
		void IScreenplayListener.OnCommandTalk( string name, string context )
		{
			this.speakerName = name;
			this.context = context;
			StartCoroutine( PlayTalk() );
		}
		void IScreenplayListener.OnEndScreenplay()
		{
			Debug.Log( "대본 종료" );
		}
	}
}