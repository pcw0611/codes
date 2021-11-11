#include <iostream>
#include <vector>

using namespace std;

struct coord
{
	int x;
	int y;
};


bool is_existing(coord cdd, vector<coord>* cds)
{
	for ( int i = 0; i < cds->size(); ++i )
	{
		if ( (*cds)[i].x == cdd.x && (*cds)[i].y == cdd.y )
			return true;
	}

	return false;
}
void get_in_quadrant(vector<coord>* coords, int start_x, int start_y, int t_dist, int x_dir, int y_dir)
{
	for ( int x = 0; x <= t_dist; ++x )
	{
		for ( int y = 0; y <= t_dist; ++y )
		{
			coord vector;
			vector.x = (start_x + (x * x_dir)) - start_x;
			vector.y = (start_y + (y * y_dir)) - start_y;

			int dist = abs(vector.x) + abs(vector.y);

			if ( dist == t_dist )
			{
				coord cd;
				cd.x = start_x + vector.x;
				cd.y = start_y + vector.y;

				if ( !is_existing(cd, coords) )
				{
					coords->push_back(cd);
				}
			}
		}
	}
}
void get_detectable_coords(vector<coord>* coords, int sour_x, int sour_y, int sour_dist)
{
	get_in_quadrant(coords, sour_x, sour_y, sour_dist, +1, +1);
	get_in_quadrant(coords, sour_x, sour_y, sour_dist, +1, -1);
	get_in_quadrant(coords, sour_x, sour_y, sour_dist, -1, -1);
	get_in_quadrant(coords, sour_x, sour_y, sour_dist, -1, +1);
}
int main()
{
	int cnt;
	vector<coord> aList;
	vector<coord> bList;
	vector<int>	ress;

	cin >> cnt;

	for ( int i = 0; i < cnt; ++i )
	{
		int x1, y1, r1;
		int x2, y2, r2;

		cin >> x1 >> y1 >> r1 >> x2 >> y2 >> r2;

		x1 = min(x1, 10000);
		y1 = min(y1, 10000);
		x2 = min(x2, 10000);
		y2 = min(y2, 10000);

		x1 = max(x1, -10000);
		y1 = max(y1, -10000);
		x2 = max(x2, -10000);
		y2 = max(y2, -10000);

		r1 = min(r1, 10000);
		r2 = min(r2, 10000);
		r1 = max(r1, 1);
		r2 = max(r2, 1);

		get_detectable_coords(&aList, x1, y1, r1);
		get_detectable_coords(&bList, x2, y2, r2);

		int count = 0;
		for ( int i = 0; i < aList.size(); ++i )
		{
			for ( int j = 0; j < bList.size(); ++j )
			{
				if ( aList[i].x == bList[j].x && aList[i].y == bList[j].y )
				{
					++count;
				}
			}
		}

		if ( x1 == x2 && y1 == y2 && r1 == r2 )
			count = -1;

		cin.ignore();
		aList.clear();
		bList.clear();
		ress.push_back(count);
	}

	for ( int i = 0; i < ress.size(); ++i )
	{
		cout << ress[i] << endl;
	}
}