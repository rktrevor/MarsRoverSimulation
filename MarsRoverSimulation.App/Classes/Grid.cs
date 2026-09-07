using System;
using System.Collections.Generic;
using System.Text;

namespace MarsRoverSimulation.App.Classes
{
    /// <summary>
    /// The bounded rectangular grid. Also tracks the "scent" left behind by
    /// robots that have fallen off the edge, keyed by the last grid point
    /// they occupied before disappearing.
    /// </summary>
    public class Grid
    {
        public int MaxX { get; }
        public int MaxY { get; }

        private readonly HashSet<(int X, int Y)> _scents = new();

        public Grid(int maxX, int maxY)
        {
            MaxX = maxX;
            MaxY = maxY;
        }

        public bool IsInBounds(int x, int y) => x >= 0 && x <= MaxX && y >= 0 && y <= MaxY;

        public bool HasScentAt(int x, int y) => _scents.Contains((x, y));

        public void LeaveScentAt(int x, int y) => _scents.Add((x, y));
    }
}
