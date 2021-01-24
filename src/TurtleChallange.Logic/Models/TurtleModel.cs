using System;
using System.Linq;

namespace TurtleChallange.Logic.Models
{
    public enum TurtleHeadOrientation
    {
        North = default,
        East = 1,
        South = 2,
        West = 3
    }

    public class TurtleModel
    {
        public Point Position { get; private set; }
        public TurtleHeadOrientation Orientation { get; private set; }

        public static int AvailablePositions = (int) Enum.GetValues(typeof(TurtleHeadOrientation)).Cast<TurtleHeadOrientation>().Count();

        public TurtleModel(Point position, TurtleHeadOrientation orientation)
        {
            Position = position ?? throw new ArgumentNullException(nameof(position));
            Orientation = orientation;
        }

        public void RotateRight()
        {
            Orientation = (TurtleHeadOrientation)(((int)Orientation + 1) % AvailablePositions);
        }

        public void MoveStraight()
        {
            switch (Orientation)
            {
                case TurtleHeadOrientation.North:
                    Position.Y -= 1;
                    break;

                case TurtleHeadOrientation.East:
                    Position.X += 1;
                    break;

                case TurtleHeadOrientation.South:
                    Position.Y += 1;
                    break;

                case TurtleHeadOrientation.West:
                    Position.X -= 1;
                    break;

                default: throw new NotSupportedException();
            }
        }
    }
}
