using System;
using System.Collections.Generic;
using System.Linq;

namespace TurtleChallange.Logic.Models
{
    public enum TileEnum
    {
        Free = default,
        Mine = 1,
        Exit = 2
    }

    public class BoardModel
    {
        public const int MIN_WIDTH = 4;
        public const int MIN_HEIGHT = 4;
        public const int MIN_MINES_COUNT = 2;

        private readonly TileEnum[,] tiles; // line and columns -> width  is columns and height is lines

        public int Width { get; }   // columns
        public int Height { get; }  // lines


        public BoardModel(int width, int height, IReadOnlyCollection<Point> minesPoints, Point exitPoint)
        {
            if (width < MIN_WIDTH)                      throw new ArgumentException(nameof(width));
            if (height < MIN_HEIGHT)                    throw new ArgumentException(nameof(height));
            if (minesPoints == null)                    throw new ArgumentNullException(nameof(minesPoints));
            if (exitPoint == null)                      throw new ArgumentNullException(nameof(exitPoint));
            if (minesPoints.Count < MIN_MINES_COUNT)   throw new ArgumentException($"{nameof(minesPoints)} has a minumum of mines required of ({MIN_MINES_COUNT})");

            Width = width;
            Height = height;

            tiles = new TileEnum[height, width];

            SetExit(exitPoint);
            SetMines(minesPoints);
        }

        public TileEnum GetPositionStatus(Point point)
        {
            if (!CheckIfPointIsWithinBoard(point))  throw new ArgumentException("Point outside of bounds");
            
            return tiles[point.Y, point.X];       
        }

        // private
        private void SetExit(Point exitPoint)
        {
            if (!CheckIfPointIsWithinBoard(exitPoint))          throw new ArgumentException("Exit point is not within the board");
            if (!CheckIfPointIsInBoarderOfTheBoard(exitPoint))  throw new ArgumentException("Exit point must be set on the boarder of the board. It can't be set inside the board");

            tiles[exitPoint.Y, exitPoint.X] = TileEnum.Exit;
        }

        private void SetMines(IReadOnlyCollection<Point> minesPoints)
        {
            // check first if mines are within else collect invalid and throw.
            IReadOnlyCollection<Point> invalidMines = minesPoints.Where(p => !CheckIfPointIsWithinBoard(p)).ToList().AsReadOnly();

            if(invalidMines.Any())
            {
                throw new ArgumentException($"Mines are out of bounds of the board. {string.Join(", ", invalidMines)}");
            }

            foreach (var point in minesPoints)
            {
                tiles[point.Y, point.X] = TileEnum.Mine;
            }
        }      


        private bool CheckIfPointIsWithinBoard(Point p) => 
            p.X >= 0 && p.X < Width && p.Y >= 0 && p.Y < Height;

        private bool CheckIfPointIsInBoarderOfTheBoard(Point exitPoint) => 
            (exitPoint.X == 0 || exitPoint.X == Width -1) || (exitPoint.Y == 0 || exitPoint.Y == Height -1);
    }
}
