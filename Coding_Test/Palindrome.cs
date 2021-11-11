using System;

class palindrome
{
	static void Main( string[] args )
	{
		Console.WriteLine( IsPalindrome("madam" ) );
	}

	static public bool IsPalindrome( string arg )
	{
		for ( int i = 0; i < arg.Length / 2; i++ )
		{
			if ( arg[i] != arg[arg.Length - 1 - i] )
				return false;
		}

		return true;
	}
}