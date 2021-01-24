using System;
using System.ComponentModel.DataAnnotations;
using TurtleChallange.Logic.Models;
using TurtleChallange.Logic.Processor.Models;

namespace TurtleChallange.Logic.Processor
{
    public enum StepResult
    {
        [Display(Name = "Moved straight")]
        MovedStraight = 1,

        [Display(Name = "Rotated right")]
        RotatedRight = 2,

        [Display(Name = "You hit a mine")]
        MineHit = 3,

        [Display(Name = "Congratulations you found the exit!")]
        ExitFound = 4,

        [Display(Name = "You hit the wall")]
        WallHit = 5
    }

    public class GameProcessor : IGameProcessor
    {
        public BoardModel Board { get; }
        public TurtleModel Turtle { get; }

        public GameProcessor(BoardModel board, TurtleModel turtle)
        {
            this.Board = board ?? throw new ArgumentNullException(nameof(board));
            this.Turtle = turtle ?? throw new ArgumentNullException(nameof(turtle));

            if (!CheckIfTurtleIsWithinBoard())  throw new ArgumentException("Turtle is not within the board");
            if (CheckIfTurtleIsOnMine())       throw new ArgumentException("Turtle can't start where a mine is");
        }



        public StepResult ProcessStep(TurtleAction action)
        {
            switch(action)
            {
                case TurtleAction.Move:
                    return MoveTurtle();

                case TurtleAction.Rotate:
                    return RotateTurtle();

                default:
                    throw new NotSupportedException("turtle action not known.");
            }
        }


        private StepResult MoveTurtle()
        {
            // this method worries about hitting mines, moving out of board, finding exit..
            // we move towards the direction of the head.
            switch(Turtle.Orientation)
            {
                case TurtleHeadOrientation.North:
                    if (Turtle.Position.Y == 0)
                        return StepResult.WallHit;
                    break;

                case TurtleHeadOrientation.East:
                    if (Turtle.Position.X == Board.Width - 1)
                        return StepResult.WallHit;
                    break;

                case TurtleHeadOrientation.South:
                    if (Turtle.Position.Y == Board.Height - 1)
                        return StepResult.WallHit;
                    break;

                case TurtleHeadOrientation.West:
                    if (Turtle.Position.X == 0)
                        return StepResult.WallHit;
                    break;

                default: throw new NotSupportedException();
            }

            // move turtle freely
            Turtle.MoveStraight();

            TileEnum status = Board.GetPositionStatus(Turtle.Position);

            switch(status)
            {
                case TileEnum.Free:
                    return StepResult.MovedStraight;

                case TileEnum.Mine:
                    return StepResult.MineHit;

                case TileEnum.Exit:
                    return StepResult.ExitFound;

                default:
                    throw new NotSupportedException("Tile enum not recognized");
            }
        }



        private StepResult RotateTurtle()
        {
            // this method dont worry to much all we need is to rotate in the same cell.
            Turtle.RotateRight();
            return StepResult.RotatedRight;
        }

        private bool CheckIfTurtleIsWithinBoard() =>
            Turtle.Position.X >= 0 && Turtle.Position.X < Board.Width && Turtle.Position.Y >= 0 && Turtle.Position.Y < Board.Height;

        private bool CheckIfTurtleIsOnMine() =>
            Board.GetPositionStatus(Turtle.Position) == TileEnum.Mine;
    }
}
