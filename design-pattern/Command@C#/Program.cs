using System;

class Program
{
	static void Main( string[] args )
	{
		RobotKit robotKit = new RobotKit();

		robotKit.AddCommand( new MoveForwardCommand( 1 ) );
		robotKit.AddCommand( new MoveForwardCommand( 1 ) );
		robotKit.AddCommand( new MoveForwardCommand( 1 ) );
		robotKit.AddCommand( new MoveForwardCommand( 1 ) );
		robotKit.AddCommand( new TurnCommand( Robot.Direction.Left ) );
		robotKit.AddCommand( new PickupCommand() );
		robotKit.Start();
	}
}