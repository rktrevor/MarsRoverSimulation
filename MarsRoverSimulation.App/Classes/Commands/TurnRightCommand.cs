using MarsRoverSimulation.App.Interfaces;
using MarsRoverSimulation.App.Enums;


namespace MarsRoverSimulation.App.Classes.Commands
{
    /// <summary>
    ///    Represents a command to turn the robot right.
    /// </summary>
    internal class TurnRightCommand : ICommand
    {
        public void Execute(Robot robot, Grid grid)
        {
            // N -> E -> S -> W -> N
            robot.Facing = (Orientation)(((int)robot.Facing + 1) % 4);
        }
    }


}
