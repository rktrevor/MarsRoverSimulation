using System;
using System.Collections.Generic;
using System.Text;

namespace MarsRoverSimulation.App.Classes
{

    /// <summary>
    /// A robot that moves and turns on a <see cref="Grid"/>, following a
    /// string of instruction letters.
    /// </summary>
    /// 
    internal class Robot
    {
        // Orientations in clockwise order. Turning right advances one step
        // through this list (mod 4); turning left steps back one.
        private static readonly char[] Orientations = { 'N', 'E', 'S', 'W' };

        // Movement deltas for a "Forward" instruction, keyed by orientation.
        private static readonly Dictionary<char, (int Dx, int Dy)> MoveDelta = new()
        {
            ['N'] = (0, 1),
            ['E'] = (1, 0),
            ['S'] = (0, -1),
            ['W'] = (-1, 0),
        };

        private readonly Grid _grid;
        private readonly Dictionary<char, Action> _commands;

        public Position Position { get; }
        public bool Lost { get; private set; }

        public Robot(Grid grid, Position position)
        {
            _grid = grid;
            Position = position;

            // Command table: instruction letter -> handler.
            //
            // The problem statement notes that "additional command types may
            // be required in the future". Because dispatch goes through this
            // dictionary rather than an if/else or switch chain, a new
            // instruction can be added with a single extra entry here -
            // nothing else in the class (or the input parsing) needs to change.
            _commands = new Dictionary<char, Action>
            {
                ['L'] = TurnLeft,
                ['R'] = TurnRight,
                ['F'] = MoveForward,
            };
        }

        private void TurnLeft()
        {
            int idx = Array.IndexOf(Orientations, Position.Orientation);
            Position.Orientation = Orientations[(idx + 3) % 4]; // -1 mod 4
        }

        private void TurnRight()
        {
            int idx = Array.IndexOf(Orientations, Position.Orientation);
            Position.Orientation = Orientations[(idx + 1) % 4];
        }

        private void MoveForward()
        {
            var (dx, dy) = MoveDelta[Position.Orientation];
            int newX = Position.X + dx;
            int newY = Position.Y + dy;

            if (_grid.InBounds(newX, newY))
            {
                Position.X = newX;
                Position.Y = newY;
                return;
            }

            // Moving would take the robot off the grid.
            if (_grid.HasScent(Position.X, Position.Y))
            {
                // A previous robot was lost from here; ignore this instruction.
                return;
            }

            // Robot is lost: leave a scent at its last valid position.
            _grid.LeaveScent(Position.X, Position.Y);
            Lost = true;
        }

        public void RunInstructions(string instructions)
        {
            foreach (char letter in instructions)
            {
                if (Lost)
                {
                    // Once lost, the robot is gone; stop processing any
                    // remaining letters in the instruction string.
                    break;
                }

                if (_commands.TryGetValue(letter, out var handler))
                {
                    handler();
                }
                // Unrecognized letters are ignored rather than throwing, so
                // future/unsupported instruction types degrade gracefully
                // instead of crashing the whole run.
            }
        }
        public string Report() => Lost ? $"{Position} LOST" : Position.ToString();
    }
}
