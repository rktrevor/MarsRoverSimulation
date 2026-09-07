using MarsRoverSimulation.App.Classes;
using System;
using System.Collections.Generic;
using System.Text;

namespace MarsRoverSimulation.App.Interfaces
{
    /// <summary>
    /// Interface for the main simulation logic. It defines a method to run the simulation with a given robot and instructions.
    /// </summary>
    internal interface ISimulationMainInterface
    {
        Robot Run(Robot robot, string instructions);
    }
}
