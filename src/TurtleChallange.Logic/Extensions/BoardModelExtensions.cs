using System;
using System.Collections.Generic;
using System.Text;
using TurtleChallange.Logic.Models;

namespace TurtleChallange.Logic.Extensions
{
    public static class BoardModelExtensions
    {
        public static void DumpToLog(this BoardModel boardModel, IExternalLogger logger)
        {
            StringBuilder sb = new StringBuilder();

            for (int lineIdx = 0; lineIdx < boardModel.Height; lineIdx++)
            {
                for (int columnIdx = 0; columnIdx < boardModel.Width; columnIdx++)
                {
                    Point current = new Point(columnIdx, lineIdx);

                    switch (boardModel.GetPositionStatus(current))
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

            logger.Log(sb.ToString());
        }
    }
}
