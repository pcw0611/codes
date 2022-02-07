using System;
using System.Collections.Generic;

abstract class Command
{
	public Robot robot { set; get; }

	public abstract void Execute();
}

class MoveForwardCommand : Command
{
	int space;

	public MoveForwardCommand( int space )
	{
		this.space = space;
	}

	public override void Execute()
	{
		robot.MoveForward( space );
	}
}

class TurnCommand : Command
{
	Robot.Direction direction;

	public TurnCommand( Robot.Direction direction )
	{
		this.direction = direction;
	}

	public override void Execute()
	{
		robot.Turn( direction );
	}
}

class PickupCommand : Command
{
	public override void Execute()
	{
		robot.Pickup();
	}
}
