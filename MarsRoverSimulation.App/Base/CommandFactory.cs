using MarsRoverSimulation.App.Classes.Commands;
using MarsRoverSimulation.App.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace MarsRoverSimulation.App.Base
{
    /// <summary>
    /// Maps instruction letters to ICommand instances. Extend this to
    /// support additional instruction types in future without touching
    /// the simulation loop.
    /// </summary>
    /// 
    internal class CommandFactory
    {
        private static readonly Dictionary<char, ICommand> Commands = new()
        {
            { 'L', new TurnLeftCommand() },
            { 'R', new TurnRightCommand() },
            { 'F', new MoveForwardCommand() }
        };

        public static bool TryCreate(char instruction, out ICommand command) =>
            Commands.TryGetValue(instruction, out command);
    }
}
