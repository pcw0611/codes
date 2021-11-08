#include <iostream>

using namespace std;

void Rotate(int* sour, int rotDir)
{
	int dest[5];

	for ( int i = 0; i < 5; ++i )
	{
		int index = (i + (-rotDir)) % 5;

		if ( index < 0 )
			index += 5;

		dest[i] = sour[index];
	}

	memcpy(sour, dest, 20);
}
int main()
{

	int arr[] = { 1, 2, 4, 5, 7 };

	Rotate(arr, 2);

	for ( int i = 0; i < 5; ++i )
		cout << arr[i] << ", ";

	return 0;
}