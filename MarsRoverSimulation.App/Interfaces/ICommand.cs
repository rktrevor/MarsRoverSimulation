using MarsRoverSimulation.App.Classes;
using System;
using System.Collections.Generic;
using System.Text;

namespace MarsRoverSimulation.App.Interfaces
{
    // ---------------------------------------------------------------------
    // Commands
    //
    // Each instruction letter maps to an ICommand. New instruction types
    // can be added in future simply by implementing ICommand and
    // registering the new letter in CommandFactory - nothing else in the
    // program needs to change.
    // ---------------------------------------------------------------------

    internal interface ICommand
    {
        void Execute(Robot robot, Grid grid);
    }
}
