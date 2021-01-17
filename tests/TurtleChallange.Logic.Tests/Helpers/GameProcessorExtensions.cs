using System;
using System.Collections.Generic;
using System.Text;
using TurtleChallange.Logic.Models;
using TurtleChallange.Logic.Processor;

namespace TurtleChallange.Logic.Tests.Helpers
{
    public static class GameProcessorExtensions
    {
        public static void DumpToDiagnostics(this GameProcessor processor)
        {
            StringBuilder sb = new StringBuilder();

            for (int line = 0; line < processor.Board.Height; line++)
            {
                for (int column = 0; column < processor.Board.Width; column++)
                {
                    // check if turtle is here.
                    if (processor.Turtle.Position.X == column && processor.Turtle.Position.Y == line)
                    {
                        sb.Append("T");
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
                            sb.Append("E");
                            break;

                        default: throw new NotSupportedException();
                    }
                }

                sb.AppendLine();
            }

            System.Diagnostics.Trace.WriteLine(sb.ToString());
        }
    }
}
