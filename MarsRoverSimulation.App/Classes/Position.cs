using System;
using System.Collections.Generic;
using System.Text;

namespace MarsRoverSimulation.App.Classes
{
    /// <summary>
    /// A robot's location and facing direction on the grid.
    /// </summary>
    internal class Position
    {
        public int X { get; set; }
        public int Y { get; set; }
        public char Orientation { get; set; }

        public Position(int x, int y, char orientation)
        {
            X = x;
            Y = y;
            Orientation = orientation;
        }

        public override string ToString() => $"{X} {Y} {Orientation}";
    }
}
