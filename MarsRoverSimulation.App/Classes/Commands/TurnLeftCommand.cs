using MarsRoverSimulation.App.Interfaces;
using MarsRoverSimulation.App.Enums;



namespace MarsRoverSimulation.App.Classes.Commands
{
    /// <summary>
    ///     Represents a command to turn the robot left.
    /// </summary>
    public class TurnLeftCommand : ICommand
    {
        public void Execute(Robot robot, Grid grid)
        {
            // N -> W -> S -> E -> N
            robot.Facing = (Orientation)(((int)robot.Facing + 3) % 4);
        }
    }
}
