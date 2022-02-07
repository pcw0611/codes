using System.Collections;
using System.Collections.Generic;

class RobotKit
{
	private Robot robot = new Robot();
	private List<Command> commands = new List<Command>();

	public void AddCommand( Command command )
	{
		commands.Add( command );
	}
	public void Start()
	{
		foreach ( Command command in commands )
		{
			command.robot = robot;
			command.Execute();
		}
	}
}