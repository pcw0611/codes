using System;

class Robot
{
	public enum Direction
	{
		Left,
		Right,
	}

	public void MoveForward( int space )
	{
		Console.WriteLine( space + " 칸 전진" );
	}
	public void Turn( Direction direction )
	{
		Console.WriteLine( ( direction == Direction.Left ? "왼쪽" : "오른쪽" ) + "으로 방향 전환" );
	}
	public void Pickup()
	{
		Console.WriteLine( "앞의 물건 집어들기" );
	}
}
