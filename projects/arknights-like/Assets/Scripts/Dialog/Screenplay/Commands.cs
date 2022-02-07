using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Dialog
{
	public abstract class Command
	{
		public System.Action		onNext		{ set; get; }
		public bool					autoNext	{ set; get; }

		public abstract void Execute();
		public abstract void Parse( string text, IScreenplayListener screenplayCommand );

		protected void End()
		{
			if ( autoNext )
				onNext();
		}
	}

	public class BgCommand : Command
	{
		private string						bgName;
		public System.Action<string>		onActiveBg { set; get; }

		public override void Execute()
		{
			if ( null != onActiveBg )
				onActiveBg( bgName );

			End();
		}
		public override void Parse( string text, IScreenplayListener screenplayEvent )
		{
			string bgName           = "";
			string imageCheckTxt    = "(image=";

			for ( int i = 0; i < text.Length; ++i )
			{
				if ( text.Substring( i, imageCheckTxt.Length ) == imageCheckTxt )
				{
					i = i + imageCheckTxt.Length + 1;

					while ( text[i] != '"' )
					{
						bgName += text[i];
						++i;
					}
					break;
				}
			}

			this.autoNext = true;
			this.bgName = bgName;
			this.onActiveBg = screenplayEvent.OnCommandBg;
		}
	}

	public class TalkCommand : Command
	{
		string										name;
		string										context;
		public System.Action<string, string>		onCommandTalk { set; get; }

		public override void Parse( string text, IScreenplayListener screenplayCommand )
		{
			string name = "";
			string context = "";

			for ( int i = 0; i < text.Length; ++i )
			{
				if ( text.Substring( i, 5 ) == "name=" )
				{
					i = i + 6;

					while ( text[i] != '"' )
					{
						name += text[i];
						++i;
					}
				}
				else if ( text[i] == '\t' )
				{
					++i;

					while ( i < text.Length )
					{
						context += text[i];
						++i;
					}
				}

				this.name = name;
				this.context = context;
				this.onCommandTalk = screenplayCommand.OnCommandTalk;
			}
		}
		public override void Execute()
		{
			if ( null != onCommandTalk )
				onCommandTalk( name, context );

			End();
		}
	}

	public class CharCommand : Command
	{
		private CharCommandInfo					charCmdInfo;

		public System.Action<CharCommandInfo>	onActiveChar { set; get; }

		public override void Parse( string text, IScreenplayListener screenplayEvent )
		{
			CharCommandInfo charCmdInfo = new CharCommandInfo();

			int         charCnt     = 1;
			string      focusValue  = "";
			string      charFmt     = "name";
			string      focusFmt    = "focus=";

			for ( int i = 0; i < text.Length; ++i )
			{
				string checkName = ( charCnt == 1 ? charFmt + "=" : "name" + charCnt + "=" );

				if ( text.Substring( i, checkName.Length ) == checkName )
				{
					string name = "";
					i += checkName.Length + 1;

					while ( text[i] != '"' )
					{
						name += text[i];
						++i;
					}

					charCmdInfo.names.Add( name );
					++charCnt;
				}

				else if ( text.Substring( i, focusFmt.Length ) == focusFmt )
				{
					i += focusFmt.Length;

					while ( text[i] != ')' )
					{
						focusValue += text[i];
						++i;
					}

					charCmdInfo = new  CharCommandInfo( Int32.Parse( focusValue ), new List<string>() );
					break;
				}
			}

			this.autoNext = true;
			this.charCmdInfo = charCmdInfo;
			this.onActiveChar = screenplayEvent.OnCommandCharacter;
		}
		public override void Execute()
		{
			if ( null != onActiveChar )
				onActiveChar( charCmdInfo );

			End();
		}
	}

	public class CommandFactory : Factory
	{
		public static Command Create( string text, IScreenplayListener screenplayEvent, System.Action onNext )
		{
			Command command = null;

			string identiText = text.Substring( 0, 5 );

			switch ( identiText )
			{
				case "[name":
					command = new TalkCommand();
					break;

				case "[Back":
					command = new BgCommand();
					break;

				case "[Char":
					command = new CharCommand();
					break;
			}

			if ( null != command )
			{
				command.Parse( text, screenplayEvent );
				command.onNext = onNext;
			}

			return command;
		}
	}

	public readonly struct CharCommandInfo
	{
		public readonly int				focus;
		public readonly List<string>	names;

		public CharCommandInfo( int focus, List<string> names )
		{
			this.focus = focus;
			this.names = new List<string>();
		}
	}
}