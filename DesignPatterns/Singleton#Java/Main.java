
public class Main 
{
	public static void main(String[] args) 
	{
		// TODO Auto-generated method stub
		
		boolean darkMode	= Singleton.singleton().getDarkMode();
		int 	fontSize 	= Singleton.singleton().getFontSize();
		
		System.out.println( darkMode );
		System.out.println( fontSize );
	}
} 