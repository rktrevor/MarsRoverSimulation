using System;
using System.Collections.Generic;
using System.Text;

namespace MarsRoverSimulation.App.Classes
{
    /// <summary>
    /// The bounded rectangular surface of Mars, with the lower-left corner
    /// fixed at (0, 0). Tracks the "scent" left behind at grid points where
    /// a robot has previously been lost.
    /// </summary>
    
    internal class Grid
    {
        public int MaxX { get; }
        public int MaxY { get; }

        private readonly HashSet<(int X, int Y)> _scents = new();

        public Grid(int maxX, int maxY)
        {
            MaxX = maxX;
            MaxY = maxY;
        }

        public bool InBounds(int x, int y) =>
            x >= 0 && x <= MaxX && y >= 0 && y <= MaxY;

        public bool HasScent(int x, int y) => _scents.Contains((x, y));

        public void LeaveScent(int x, int y) => _scents.Add((x, y));
    }
}
