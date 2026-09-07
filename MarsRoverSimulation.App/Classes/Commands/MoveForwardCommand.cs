using MarsRoverSimulation.App.Enums;
using MarsRoverSimulation.App.Interfaces;

namespace MarsRoverSimulation.App.Classes.Commands
{
    /// <summary>
    ///     Represents a command to move the robot forward.
    /// </summary>
    public class MoveForwardCommand : ICommand
    {
        private static readonly Dictionary<Orientation, (int Dx, int Dy)> Deltas = new()
        {
            { Orientation.N, (0, 1) },
            { Orientation.E, (1, 0) },
            { Orientation.S, (0, -1) },
            { Orientation.W, (-1, 0) }
        };

        public void Execute(Robot robot, Grid grid)
        {
            var (dx, dy) = Deltas[robot.Facing];
            int newX = robot.X + dx;
            int newY = robot.Y + dy;

            if (grid.IsInBounds(newX, newY))
            {
                robot.X = newX;
                robot.Y = newY;
                return;
            }

            // Would move off the grid.
            if (grid.HasScentAt(robot.X, robot.Y))
            {
                // A previous robot was lost from this exact point -
                // the instruction is simply ignored.
                return;
            }

            // No scent here yet: this robot is lost. Leave a scent at the
            // last point it occupied before vanishing.
            grid.LeaveScentAt(robot.X, robot.Y);
            robot.MarkLost();
        }
    }
}
