using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TurtleChallange.Logic.Models;

namespace TurtleChallange.Logic.Tests.TestData.Models
{
    public static class BoardModelTestData
    {
        private static readonly Func<Random> random = new Func<Random>(() => new Random());

        // private
        private static int Gen_Valid_Random_Width() => random().Next(BoardModel.MIN_WIDTH, 16);
        private static int Gen_Valid_Random_Height() => random().Next(BoardModel.MIN_HEIGHT, 16);

        private static IReadOnlyCollection<Point> Gen_Valid_Random_Mines(int boardWidth, int boardHeight)
        {
            int numberOfMines = random().Next(4, 6);

            List<Point> mines = new List<Point>();

            while (numberOfMines > 0)
            {
                // gen mine
                Point newMine = CreateRandomMine(boardWidth, boardHeight);

                if (!mines.Any(p => p.X == newMine.X && p.Y == newMine.Y))
                {
                    mines.Add(newMine);
                    numberOfMines--;
                }
            }

            return mines.AsReadOnly();
        }



        private static Point Gen_Valid_ExitPoint(int boardWidth, int boardHeight)
        {
            // a valid exit point is if the exit is on a boarder of the board

            // X value is betwen 0 and width while y = 0 or y value is between 0 and height and x = 0;

            bool useXstatic = random().Next(0, 1) == 0; // else use Y
            bool useZero = random().Next(0, 1) == 0;    // else use width or height
            
            if (useXstatic)
            {
                // try rand if 0 or width
                return new Point(useZero ? 0 : boardWidth, random().Next(0, boardHeight));
            }

            // use y instead

            return new Point(random().Next(0, boardWidth), useZero ? 0 : boardHeight);
        }



        private static Point CreateRandomMine(int width, int height) =>
            new Point(random().Next(0, width - 1), random().Next(0, height - 1));

        public static (int width, int height, IReadOnlyCollection<Point> mines, Point exitPoint) CreateValidBoardModel()
        {
            int width = Gen_Valid_Random_Width();
            int height = Gen_Valid_Random_Height();

            return (width, height, Gen_Valid_Random_Mines(width, height), Gen_Valid_ExitPoint(width, height));
        }
    }
}

