using MarsRoverSimulation.App.Classes;
using MarsRoverSimulation.App.Enums;

namespace MarsRoverSimulation.App.Helper
{
    // ---------------------------------------------------------------------
    // Input parsing helpers
    // Helper to help with the input parsing and validation of the grid size, robot position, and instruction strings.
    // ---------------------------------------------------------------------

    public static class InputParser
    {
        public const int MaxCoordinate = 50;
        public const int MaxInstructionLength = 99;
        private static readonly HashSet<char> KnownInstructionLetters = new() { 'L', 'R', 'F' };

        public static (int MaxX, int MaxY) ParseGridSize(string line)
        {
            var parts = SplitWhitespace(line);
            return (int.Parse(parts[0]), int.Parse(parts[1]));
        }

        public static Robot ParseRobot(string line)
        {
            var parts = SplitWhitespace(line);
            int x = int.Parse(parts[0]);
            int y = int.Parse(parts[1]);
            var facing = Enum.Parse<Orientation>(parts[2], ignoreCase: true);
            return new Robot(x, y, facing);
        }

        /// <summary>Non-throwing version of ParseGridSize, for validating interactive input.</summary>
        public static bool TryParseGridSize(string line, out int maxX, out int maxY, out string? error)
        {
            maxX = maxY = 0;
            error = null;
            var parts = SplitWhitespace(line);

            if (parts.Length != 2 || !int.TryParse(parts[0], out maxX) || !int.TryParse(parts[1], out maxY))
            {
                error = "Please enter two whole numbers, e.g. '5 3'.";
                return false;
            }

            if (maxX < 0 || maxY < 0 || maxX > MaxCoordinate || maxY > MaxCoordinate)
            {
                error = $"Coordinates must be between 0 and {MaxCoordinate}.";
                return false;
            }

            return true;
        }

        /// <summary>Non-throwing version of ParseRobot, for validating interactive input.</summary>
        public static bool TryParseRobot(string line, out Robot? robot, out string? error)
        {
            robot = null;
            error = null;
            var parts = SplitWhitespace(line);

            if (parts.Length != 3)
            {
                error = "Please enter x, y and an orientation, e.g. '1 1 E'.";
                return false;
            }

            if (!int.TryParse(parts[0], out int x) || !int.TryParse(parts[1], out int y))
            {
                error = "X and Y must be whole numbers.";
                return false;
            }

            if (x < 0 || y < 0 || x > MaxCoordinate || y > MaxCoordinate)
            {
                error = $"Coordinates must be between 0 and {MaxCoordinate}.";
                return false;
            }

            if (!Enum.TryParse<Orientation>(parts[2], ignoreCase: true, out var facing))
            {
                error = "Orientation must be one of N, S, E, W.";
                return false;
            }

            robot = new Robot(x, y, facing);
            return true;
        }

        /// <summary>Validates an instruction string. Unknown letters only produce a warning,
        /// since the program is designed to tolerate future instruction types.</summary>
        public static bool TryValidateInstructions(string line, out string? error, out string? warning)
        {
            error = null;
            warning = null;

            if (string.IsNullOrEmpty(line))
            {
                error = "Instruction string cannot be empty.";
                return false;
            }

            if (line.Length > MaxInstructionLength)
            {
                error = $"Instruction string must be shorter than {MaxInstructionLength + 1} characters.";
                return false;
            }

            var upper = line.ToUpperInvariant();
            if (upper.Any(c => !char.IsLetter(c)))
            {
                error = "Instructions may only contain letters.";
                return false;
            }

            if (upper.Any(c => !KnownInstructionLetters.Contains(c)))
            {
                warning = "Note: this contains letters other than L, R, F. " +
                           "They'll be accepted but ignored unless a matching command has been implemented.";
            }

            return true;
        }

        private static string[] SplitWhitespace(string line) =>
            line.Split((char[])null!, StringSplitOptions.RemoveEmptyEntries);
    }
}
