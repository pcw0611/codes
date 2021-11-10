using System;

class Program 
{
	static void Main( string[] args )
	{
		Arr arr = new Arr( 10 );
		arr.Add( 7 );
		arr.Add( 8 );
		arr.Add( 9 );
		IIterator iter = arr.MakerIterator();

		iter.Begin();
		while( iter.MoveNext() )
		{
			Console.WriteLine( iter.Current );
		}
	}
}