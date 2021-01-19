using System;
using System.Collections.Generic;
using System.Text;
using TurtleChallange.Logic.Models;
using TurtleChallange.Logic.Processor;

namespace TurtleChallange.Logic.Extensions
{
    public static class GameProcessorExtensions
    {
        public static void DumpToLog(this GameProcessor processor, IExternalLogger logger)
        {
            StringBuilder sb = new StringBuilder();

            for (int line = 0; line < processor.Board.Height; line++)
            {
                for (int column = 0; column < processor.Board.Width; column++)
                {
                    // check if turtle is here.
                    if (processor.Turtle.Position.X == column && processor.Turtle.Position.Y == line)
                    {
                        string head;
                        switch(processor.Turtle.Orientation)
                        {
                            case TurtleHeadOrientation.North: head = "N"; break;
                            case TurtleHeadOrientation.East:  head = "E"; break;
                            case TurtleHeadOrientation.South: head = "S"; break;
                            case TurtleHeadOrientation.West: head = "W"; break;
                            default: throw new NotSupportedException();
                        }

                        sb.Append(head);
                        continue;
                    }

                    // continue displaying board
                    Point current = new Point(column, line);
                    switch (processor.Board.GetPositionStatus(current))
                    {
                        case TileEnum.Free:
                            sb.Append("-");
                            break;
                        case TileEnum.Mine:
                            sb.Append("*");
                            break;
                        case TileEnum.Exit:
                            sb.Append("L");
                            break;

                        default: throw new NotSupportedException();
                    }
                }

                sb.AppendLine();
            }

            logger.Log(sb.ToString());
        }
    }
}
