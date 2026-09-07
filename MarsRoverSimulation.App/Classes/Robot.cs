using MarsRoverSimulation.App.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace MarsRoverSimulation.App.Classes
{
    /// <summary>
    ///    Represents a robot on the grid with its position, orientation, and lost status.
    /// </summary>
    public class Robot
    {
        public int X { get; set; }
        public int Y { get; set; }
        public Orientation Facing { get; set; }
        public bool IsLost { get; private set; }

        public Robot(int x, int y, Orientation facing)
        {
            X = x;
            Y = y;
            Facing = facing;
        }

        public void MarkLost() => IsLost = true;

        public override string ToString()
        {
            var pos = $"{X} {Y} {Facing}";
            return IsLost ? $"{pos} LOST" : pos;
        }
    }
}
