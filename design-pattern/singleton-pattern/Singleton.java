public class Singleton 
{
	private Singleton() {};
	private static Singleton settings = null;
	
	public static Singleton singleton() 
	{
		if ( settings == null )
			settings = new Singleton();
		
		return settings;
	}
	
	private boolean darkMode 	= false;
	private int 	fontSize 	= 13;
	
	public boolean getDarkMode() { return darkMode; }
	public int getFontSize() { return fontSize; }
	
	public void setDarkMode( boolean _darkMode ) { darkMode = _darkMode; }
	public void setFontSize( int _fontSize ) { fontSize = _fontSize; }
}