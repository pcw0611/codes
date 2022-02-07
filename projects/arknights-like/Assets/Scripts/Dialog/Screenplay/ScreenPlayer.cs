using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

namespace Dialog
{
	public class Screenplayer
	{
		public static Screenplayer Load( string filePath, IScreenplayListener screenplayListener )
		{
			StreamReader reader = new StreamReader( filePath );

			List<string> texts = new List<string>();

			while ( !reader.EndOfStream )
				texts.Add( reader.ReadLine() );

			reader.Close();

			return new Screenplayer( texts, screenplayListener );
		}

		private Queue<Command>			commands					= new Queue<Command>();

		public IScreenplayListener		screenplayListener			{ set; get; }

		public Screenplayer( List<string> texts, IScreenplayListener screenplayEvent )
		{
			this.screenplayListener = screenplayEvent;

			foreach ( string text in texts )
			{
				AddCommand( text );
			}

			Next();
		}

		public void AddCommand( string text )
		{
			commands.Enqueue( CommandFactory.Create( text, screenplayListener, Next ) );
		}
		public void NextManually()
		{
			if ( null == commands )
				return;

			Next();
		}
		private void Next()
		{
			if ( null == commands )
				return;

			if ( commands.Count > 0 )
			{
				commands.Dequeue().Execute();
			}
			else
			{
				screenplayListener.OnEndScreenplay();
			}
		}
	}
}
