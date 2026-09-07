using MarsRoverSimulation.App.Classes;
using MarsRoverSimulation.App.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace MarsRoverSimulation.App.Base
{
    /// <summary>
    /// Parses the Mars Rover input format and runs each robot in turn.
    /// Kept separate from Program.cs (and from Console I/O) so it can be
    /// unit tested directly against strings.
    /// </summary>
    /// 
    internal class SimulationMain : ISimulationMainInterface
    {
        private readonly Grid _grid;

        public SimulationMain(Grid grid)
        {
            _grid = grid;
        }

        public Robot Run(Robot robot, string instructions)
        {
            foreach (char c in instructions)
            {
                if (robot.IsLost)
                {
                    // A lost robot is gone; nothing further can be done with it.
                    break;
                }

                if (!CommandFactory.TryCreate(c, out var command))
                {
                    // Unknown instruction letter - ignore it rather than
                    // crash, so unrecognised future instructions degrade
                    // gracefully.
                    continue;
                }

                command.Execute(robot, _grid);
            }

            return robot;
        }
    }
}
