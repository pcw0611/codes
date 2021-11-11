#include <iostream>
#include <algorithm>

using namespace std;

int main()
{
	int cnt;
	cin >> cnt;

	int* arr = new int[cnt];

	for ( int i = 0; i < cnt; ++i )
	{
		cin >> arr[i];
	}

	sort(arr, arr + cnt);

	cout << arr[0] * arr[cnt - 1];
}
