#include <iostream>

using namespace std;

int main()
{
	const int length		= 1000;
	const int width			= 20;
	const int height		= length / width;

	int arr[length]			= { 0 };
	int maxMoveX			= width;
	int maxMoveY			= height - 1;
	int currentIndex		= -1;
	int value				= 0;
	bool inverse			= false;

	while ( true )
	{
		for ( int x = 0; x < maxMoveX; ++x )
		{
			currentIndex += inverse ? -1 : +1;

			arr[ currentIndex ] = value;

			++value;
		}
		--maxMoveX;

		for ( int y = 0; y < maxMoveY; ++y )
		{
			currentIndex += inverse ? -width : +width;

			arr[ currentIndex ] = value;

			++value;
		}
		--maxMoveY;

		inverse = !inverse;

		if ( value >= length )
			break;
	}

	for ( int i = 0; i < length; i++ )
	{
		cout << arr[ i ] << "\t";

		if ( ( i + 1 ) % width == 0 )
			cout << endl;
	}
}