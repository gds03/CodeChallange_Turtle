using System;
using System.Linq;
using TurtleChallange.Logic.Models;
using TurtleChallange.Logic.Processor;
using static TurtleChallange.Logic.Tests.TestData.Models.BoardModelTestData;
using System.Collections.Generic;

namespace TurtleChallange.Logic.Tests.TestData
{
    public static class GameProcessorTestData
    {
        private static readonly Func<Random> random = new Func<Random>(() => new Random());

        public static GameProcessor CreateValidGameProcessor()
        {
            // create board
            var (width, height, mines, exitPoint) = CreateValidBoardModel();
            BoardModel board = new BoardModel(width, height, mines, exitPoint);

            // create turtle - but turtle must be compliant with the board.
            TurtleModel turtle;
            do
            {
                Point turtlePosition = new Point(random().Next(0, width), random().Next(0, height));
                TurtleHeadOrientation orientation = (TurtleHeadOrientation)random().Next(0, TurtleModel.AvailablePositions - 1);

                turtle = new TurtleModel(turtlePosition, orientation);
            }
            while (new[] { TileEnum.Mine, TileEnum.Exit }.Any(s => s == board.GetPositionStatus(turtle.Position)));

            // ready to create
            return new GameProcessor(board, turtle);
        }

        public static GameProcessor CreateKnownGame()
        {
            // 8 x 9
            int height = 9;
            int width = 8;

            /* 01234567      y
             * ********************
             * FFFFFFMF     (0)
             * FFMFFFFF     (1)
             * FFFFFFFF     (2)
             * FFFMFTME     (3) - with turtle here - head is looking south
             * FFFFFFFM     (4)
             * MFFFFFFF     (5)
             * FFFFFFFM     (6)
             * FFFFFFMF     (7)
             * MFFFFFFF     (8)
            */
            IReadOnlyCollection<Point> mines = new List<Point>()
            {
                new Point(6, 0),
                new Point(2, 1),
                new Point(3, 3), new Point(6, 3),
                new Point(7, 4),
                new Point(0, 5),
                new Point(7, 6),
                new Point(6, 7),
                new Point(0, 8)
            };

            Point exitPoint = new Point(7, 3);
            BoardModel board = new BoardModel(width, height, mines, exitPoint);

            TurtleModel turtle = new TurtleModel(new Point(5, 3), TurtleHeadOrientation.South);
            GameProcessor processor = new GameProcessor(board, turtle);
            return processor;
        }
    }
}
