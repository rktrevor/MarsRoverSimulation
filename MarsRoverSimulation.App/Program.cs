using MarsRoverSimulation.App.Base;
using MarsRoverSimulation.App.Classes;
using MarsRoverSimulation.App.Helper;

public static class Program
{
   
    public static void Main(string[] args)
    {
        Console.WriteLine("=====================================");
        Console.WriteLine("      Mars Robots Simulator");
        Console.WriteLine("=====================================");

        bool exit = false;
        while (!exit)
        {
            ShowMenu();
            switch (ReadMenuChoice())
            {
                case 1:
                    RunInteractiveEntry();
                    break;
                case 2:
                    RunPastedBlock();
                    break;
                case 3:
                    RunSampleData();
                    break;
                case 4:
                    exit = true;
                    break;
            }
        }

        Console.WriteLine();
        Console.WriteLine("Goodbye.");
    }

    private static void ShowMenu()
    {
        Console.WriteLine();
        Console.WriteLine("Main menu:");
        Console.WriteLine("  1. Enter grid and robots one at a time");
        Console.WriteLine("  2. Paste a full input block (grid + robot pairs, original format)");
        Console.WriteLine("  3. Run the built-in sample data");
        Console.WriteLine("  4. Exit");
    }

    private static int ReadMenuChoice()
    {
        while (true)
        {
            Console.Write("Select an option (1-4): ");
            string? input = Console.ReadLine();
            if (int.TryParse(input, out int choice) && choice is >= 1 and <= 4)
            {
                return choice;
            }
            Console.WriteLine("Invalid choice - please enter a number from 1 to 4.");
        }
    }

    // -------------------------------------------------------------
    // Option 1: guided, one prompt at a time, with validation
    // -------------------------------------------------------------

    private static void RunInteractiveEntry()
    {
        var grid = ReadGridSizeInteractively();
        var simulation = new SimulationMain(grid);

        Console.WriteLine();
        Console.WriteLine($"Grid ready: (0,0) to ({grid.MaxX},{grid.MaxY}).");

        int robotNumber = 1;
        while (true)
        {
            Console.WriteLine();
            Console.Write($"Add robot #{robotNumber}? (y/n): ");
            if (!ReadYesNo())
            {
                break;
            }

            var robot = ReadRobotPositionInteractively();
            string instructions = ReadInstructionsInteractively();

            simulation.Run(robot, instructions);

            Console.WriteLine($"  Result: {robot}");
            robotNumber++;
        }

        if (robotNumber == 1)
        {
            Console.WriteLine("No robots were entered.");
        }
    }

    private static Grid ReadGridSizeInteractively()
    {
        while (true)
        {
            Console.WriteLine();
            Console.Write("Enter the upper-right grid coordinates, e.g. '5 3': ");
            string line = Console.ReadLine() ?? string.Empty;

            if (InputParser.TryParseGridSize(line, out int maxX, out int maxY, out string? error))
            {
                return new Grid(maxX, maxY);
            }

            Console.WriteLine($"  {error}");
        }
    }

    private static Robot ReadRobotPositionInteractively()
    {
        while (true)
        {
            Console.Write("  Enter starting position 'x y orientation', e.g. '1 1 E': ");
            string line = Console.ReadLine() ?? string.Empty;

            if (InputParser.TryParseRobot(line, out var robot, out string? error))
            {
                return robot!;
            }

            Console.WriteLine($"    {error}");
        }
    }

    private static string ReadInstructionsInteractively()
    {
        while (true)
        {
            Console.Write("  Enter instruction string, e.g. 'RFRFRFRF': ");
            string line = (Console.ReadLine() ?? string.Empty).Trim();

            if (InputParser.TryValidateInstructions(line, out string? error, out string? warning))
            {
                if (warning != null)
                {
                    Console.WriteLine($"    {warning}");
                }
                return line;
            }

            Console.WriteLine($"    {error}");
        }
    }

    private static bool ReadYesNo()
    {
        while (true)
        {
            string answer = (Console.ReadLine() ?? string.Empty).Trim().ToLowerInvariant();
            switch (answer)
            {
                case "y":
                case "yes":
                    return true;
                case "n":
                case "no":
                    return false;
                default:
                    Console.Write("  Please answer 'y' or 'n': ");
                    break;
            }
        }
    }

    // -------------------------------------------------------------
    // Option 2: paste the whole block in the original file format
    // -------------------------------------------------------------

    private static void RunPastedBlock()
    {
        Console.WriteLine();
        Console.WriteLine("Paste the grid line, then each robot's position and instruction lines.");
        Console.WriteLine("Enter a blank line when you're done:");
        Console.WriteLine();

        var lines = new List<string>();
        string? line;
        while (!string.IsNullOrWhiteSpace(line = Console.ReadLine()))
        {
            lines.Add(line.Trim());
        }

        if (lines.Count == 0)
        {
            Console.WriteLine("No input received.");
            return;
        }

        if (!InputParser.TryParseGridSize(lines[0], out int maxX, out int maxY, out string? gridError))
        {
            Console.WriteLine($"Invalid grid line: {gridError}");
            return;
        }

        var grid = new Grid(maxX, maxY);
        var simulation = new SimulationMain(grid);
        var results = new List<string>();

        int index = 1;
        while (index < lines.Count)
        {
            if (index + 1 >= lines.Count)
            {
                Console.WriteLine("Warning: trailing position line with no instruction string was ignored.");
                break;
            }

            if (!InputParser.TryParseRobot(lines[index], out var robot, out string? robotError))
            {
                Console.WriteLine($"Skipping invalid robot line '{lines[index]}': {robotError}");
                index += 2;
                continue;
            }

            string instructions = lines[index + 1];
            simulation.Run(robot!, instructions);
            results.Add(robot!.ToString());
            index += 2;
        }

        Console.WriteLine();
        Console.WriteLine("Results:");
        foreach (var result in results)
        {
            Console.WriteLine(result);
        }
    }

    // -------------------------------------------------------------
    // Option 3: built-in sample, for a quick demo
    // -------------------------------------------------------------

    private static void RunSampleData()
    {
        string[] sample =
        {
                "5 3",
                "1 1 E",
                "RFRFRFRF",
                "3 2 N",
                "FRRFLLFFRRFLL",
                "0 3 W",
                "LLFFFLFLFL"
            };

        var (maxX, maxY) = InputParser.ParseGridSize(sample[0]);
        var grid = new Grid(maxX, maxY);
        var simulation = new SimulationMain(grid);

        Console.WriteLine();
        Console.WriteLine("Results:");

        for (int i = 1; i + 1 < sample.Length; i += 2)
        {
            var robot = InputParser.ParseRobot(sample[i]);
            string instructions = sample[i + 1];
            simulation.Run(robot, instructions);
            Console.WriteLine(robot.ToString());
        }
    }
}