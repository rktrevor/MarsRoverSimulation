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
        public string? NextNonEmpty(IEnumerator<string> enumerator)
        {
            while (enumerator.MoveNext())
            {
                if (!string.IsNullOrWhiteSpace(enumerator.Current))
                {
                    return enumerator.Current;
                }
            }
            return null;
        }

        public Position ParsePositionLine(string line)
        {
            var parts = line.Split((char[]?)null, StringSplitOptions.RemoveEmptyEntries);
            int x = int.Parse(parts[0]);
            int y = int.Parse(parts[1]);
            char orientation = parts[2][0];
            return new Position(x, y, orientation);
        }

        public string Run(string inputText)
        {
            // Normalize line endings and split, keeping blank lines out of
            // the way but preserving order of the meaningful ones.
            var lines = inputText
                .Replace("\r\n", "\n")
                .Split('\n');

            using var enumerator = ((IEnumerable<string>)lines).GetEnumerator();

            string? gridLine = NextNonEmpty(enumerator);
            if (gridLine is null)
            {
                return string.Empty;
            }

            var gridParts = gridLine.Split((char[]?)null, StringSplitOptions.RemoveEmptyEntries);
            int maxX = int.Parse(gridParts[0]);
            int maxY = int.Parse(gridParts[1]);
            var grid = new Grid(maxX, maxY);

            var results = new List<string>();

            while (true)
            {
                string? positionLine = NextNonEmpty(enumerator);
                if (positionLine is null)
                {
                    break;
                }

                string? instructionLine = NextNonEmpty(enumerator);
                if (instructionLine is null)
                {
                    // Malformed input (position with no instruction line) -
                    // nothing more to process.
                    break;
                }

                var position = ParsePositionLine(positionLine);
                var robot = new Robot(grid, position);
                robot.RunInstructions(instructionLine.Trim());
                results.Add(robot.Report());
            }

            return string.Join("\n", results);
        }
    }
}
