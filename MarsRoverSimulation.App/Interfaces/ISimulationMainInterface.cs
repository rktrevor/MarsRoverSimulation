using MarsRoverSimulation.App.Classes;
using System;
using System.Collections.Generic;
using System.Text;

namespace MarsRoverSimulation.App.Interfaces
{
    internal interface ISimulationMainInterface
    {
        string Run(string inputText);
        Position ParsePositionLine(string line);
        string? NextNonEmpty(IEnumerator<string> enumerator);
    }
}
