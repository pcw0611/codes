//// ProjectCT.cpp : 이 파일에는 'main' 함수가 포함됩니다. 거기서 프로그램 실행이 시작되고 종료됩니다.
////
//
//#include <iostream>
//#include <string>
//#include <list>
//using namespace std;
//
//int ranks[101] = { 0 };
//
//int Rank(int score)
//{
//	ranks[score] = 1;
//	int rank = 0;
//
//	for ( int i = score; i < 101; ++i )
//	{
//		if ( ranks[i] > 0 )
//			++rank;
//	}
//
//	return rank;
//}
//int main()
//{
//	int score;
//
//	while ( true )
//	{
//		cout << "점수 추가" << endl;
//		cin >> score;
//		cout << endl << Rank(score) << "등" << endl;
//	}
//}