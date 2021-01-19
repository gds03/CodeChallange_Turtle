using System;
using System.Collections.Generic;
using System.Linq;
using TurtleChallange.Logic;
using TurtleChallange.Logic.Extensions;
using TurtleChallange.Logic.Models;
using TurtleChallange.Logic.Processor;
using TurtleChallange.Logic.Processor.Models;

namespace TurtleChallange.ConsoleApp
{
    class Program
    {
        private static IExternalLogger logger = new ConsoleLogger();

        static void Main(string[] args)
        {
            Console.WriteLine("This is the turtle challange");
            Console.WriteLine("Please specify the settings of the game");

            // gather board size settings
            var (boardWitdh, boardHeight) = ReadBoardSettings();

            Point exitPoint = GenerateRandomExitPoint(boardWitdh, boardHeight);

            IEnumerable<Point> minePoints = GenerateMinesForDificulty(exitPoint, boardWitdh, boardHeight);            

            Point turtlePoint = GenerateRandomTurtlePoint(minePoints, exitPoint, boardWitdh, boardHeight);

            BoardModel board = new BoardModel(boardWitdh, boardHeight, minePoints.ToList().AsReadOnly(), exitPoint);
            //board.DumpToLog(logger);

            GameProcessor gameProcessor = new GameProcessor(board, new TurtleModel(turtlePoint, GenerateRandomTurtleOrientation()));
            gameProcessor.DumpToLog(logger);
            
            StartGame(gameProcessor);
            Console.ReadLine();
        }

        private static void StartGame(GameProcessor gameProcessor)
        {
            Console.WriteLine("Game is ready. Turtle is on the board with the head of (N, E, S or W) which identifies if is pointing to north, east, south or west.");
            Console.WriteLine("Game starting to accept commands ('m' for move and 'r' for rotating 90 degrees to the right)");

            while(true)
            {
                string input = Console.ReadLine();

                if(!(input == "m" || input == "r"))
                {
                    Console.WriteLine("the only commands accepted are 'm' for move or 'r' for rotating right");
                    continue;
                }

                TurtleAction action = (input == "m") ? TurtleAction.Move : TurtleAction.Rotate;
                
                StepResult stepFeedback = gameProcessor.ProcessStep(action);

                switch(stepFeedback)
                {
                    case StepResult.ExitFound:
                        Console.WriteLine("Congratulations, you reached the exit!!");
                        return;
                    case StepResult.MineHit:
                        Console.WriteLine("Oh noooooooo, you hit a mine. Try a new game");
                        return;
                    case StepResult.WallHit:
                        Console.WriteLine("Going to the wrong side, you just hit the wall");
                        break;
                    case StepResult.MovedStraight:
                    case StepResult.RotatedRight:
                        Console.WriteLine(stepFeedback);
                        break;
                    default: throw new NotSupportedException();
                }

                gameProcessor.DumpToLog(logger);
            }
        }

        private static (int boardWitdh, int boardHeight) ReadBoardSettings()
        {
            while(true)
            {
                Console.WriteLine("Please specify the board width and height with format of (width x height) e.g. 20x4");

                string input = Console.ReadLine();

                if(string.IsNullOrWhiteSpace(input))
                {
                    Console.WriteLine("input can't be empty. try again");
                    continue;
                }

                if(!input.Contains("x"))
                {
                    Console.WriteLine("input not containign separator x");
                    continue;
                }

                string[] split = input.Split("x");

                if (split.Length != 2)
                {
                    Console.WriteLine("input not 2 values.");
                    continue;
                }

                if(!int.TryParse(split[0], out int boardWidth))
                {
                    Console.WriteLine("width is not a integer");
                    continue;
                }

                if (!int.TryParse(split[1], out int boardheight))
                {
                    Console.WriteLine("height is not a integer");
                    continue;
                }

                return (boardWidth, boardheight);
            }
        }
        private static IEnumerable<Point> GenerateMinesForDificulty(Point exitPoint, int boardWitdh, int boardHeight)
        {
            while(true)
            {
                Console.WriteLine("Please specify the level of difficulty going from 1 to 5 e.g. 3");

                string input = Console.ReadLine();

                if (string.IsNullOrWhiteSpace(input))
                {
                    Console.WriteLine("input can't be empty. try again");
                    continue;
                }

                if(!int.TryParse(input, out int dificultyLevel))
                {
                    Console.WriteLine("input is not an integer");
                    continue;
                }

                if(!(dificultyLevel >= 1 && dificultyLevel <= 5))
                {
                    Console.WriteLine("1 <= dificultyLevel <= 5");
                    continue;
                }

                return GenerateMinesForLevel(dificultyLevel, exitPoint, boardWitdh, boardHeight);
            }
        }

        private static IEnumerable<Point> GenerateMinesForLevel(int dificultyLevel, Point exitPoint, int boardWitdh, int boardHeight)
        {
            int numberOfTiles = boardWitdh * boardHeight;

            // level 5 has 40% of map with mines
            // level 4 has 25% of map with mines
            // level 3 has 20% of map with mines
            // level 2 has 15% of map with mines
            // level 1 has 10% of map with mines
            // 3 simple rule

            Dictionary<int, int> map = new Dictionary<int, int>()
            {
                { 1, 10 },
                { 2, 15 },
                { 3, 20 },
                { 4, 25 },
                { 5, 40 },
            };

            Random random = new Random();
            int numberOfMinesToGenerate = (map[dificultyLevel] * numberOfTiles) / 100;
            List<Point> mines = new List<Point>();

            while(numberOfMinesToGenerate > 0)
            {
                // gen coordinate
                Point point = new Point(random.Next(0, boardWitdh - 1), random.Next(0, boardHeight - 1));

                if(!mines.Any(p => p.X == point.X && p.Y == point.Y) && (exitPoint.X != point.X && exitPoint.Y != point.Y))
                {
                    mines.Add(point);
                    numberOfMinesToGenerate--;
                }
            }

            return mines;
        }

        private static Point GenerateRandomExitPoint(int boardWitdh, int boardHeight)
        {
            Random random = new Random();

            bool useStaticX = random.Next(0, 1) == 0;

            int x, y;
            if(useStaticX)
            {
                x = random.Next(0, 1) == 0 ? 0 : boardWitdh - 1;
                y = random.Next(0, boardHeight);
                return new Point(x, y);
            }


            y = random.Next(0, 1) == 0 ? 0 : boardHeight - 1;
            x = random.Next(0, boardWitdh);
            return new Point(x, y);
        }

        private static Point GenerateRandomTurtlePoint(IEnumerable<Point> minePoints, Point exitPoint, int boardWitdh, int boardHeight)
        {
            while(true)
            {
                Random random = new Random();
                Point turtlePoint = new Point(random.Next(0, boardWitdh - 1), random.Next(0, boardHeight - 1));

                if (!minePoints.Any(p => p.X == turtlePoint.X && p.Y == turtlePoint.Y) && (exitPoint.X != turtlePoint.X && exitPoint.Y != turtlePoint.Y))
                    return turtlePoint;
            }
        }

        private static TurtleHeadOrientation GenerateRandomTurtleOrientation() => (TurtleHeadOrientation) new Random().Next(0, TurtleModel.AvailablePositions);
    }
}
