namespace Dialog
{
	public interface IScreenplayListener
	{
		void OnCommandBg( string name );
		void OnCommandTalk( string name, string context );
		void OnEndScreenplay();
		void OnCommandCharacter( CharCommandInfo charCmdInfo );
	}
}
