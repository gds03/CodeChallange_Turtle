using FluentAssertions;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using TurtleChallange.Logic.Models;
using TurtleChallange.Logic.Tests.Helpers;
using Xunit;
using static TurtleChallange.Logic.Tests.TestData.Models.BoardModelTestData;

namespace TurtleChallange.Logic.Tests.Models
{
    public class BoardModelTests
    {

        [Fact]
        public void Ctor_Throws_ArgumentException_When_Width_DontComply()
        {
            //arrange
            var (width, height, mines, exitPoint) = CreateValidBoardModel();
            width = 0;

            // act
            Action act = () => new BoardModel(width, height, mines, exitPoint);

            // assert
            act.Should().Throw<ArgumentException>()
                .WithMessage("width");
        }

        [Fact]
        public void Ctor_Throws_ArgumentException_When_Height_DontComply()
        {
            //arrange
            var (width, height, mines, exitPoint) = CreateValidBoardModel();
            height = 0;

            // act
            Action act = () => new BoardModel(width, height, mines, exitPoint);

            // assert
            act.Should().Throw<ArgumentException>()
                .WithMessage("height");
        }

        [Fact]
        public void Ctor_Throws_ArgumentNullException_When_MinesAreNull()
        {
            //arrange
            var (width, height, mines, exitPoint) = CreateValidBoardModel();
            mines = null;

            // act
            Action act = () => new BoardModel(width, height, mines, exitPoint);

            // assert
            act.Should().Throw<ArgumentNullException>()
                .WithMessage("**minesPoints**");
        }

        [Fact]
        public void Ctor_Throws_ArgumentNullException_When_ExitPointIsNull()
        {
            //arrange
            var (width, height, mines, exitPoint) = CreateValidBoardModel();
            exitPoint = null;

            // act
            Action act = () => new BoardModel(width, height, mines, exitPoint);

            // assert
            act.Should().Throw<ArgumentNullException>()
                .WithMessage("**exitPoint**");
        }

        [Fact]
        public void Ctor_Throws_ArgumentException_When_ExitPointIsInMiddleOfTheBoard()
        {
            //arrange
            var (width, height, mines, exitPoint) = CreateValidBoardModel();
            exitPoint = new Point(2, 2);

            // act
            Action act = () => new BoardModel(width, height, mines, exitPoint);

            // assert
            act.Should().Throw<ArgumentException>()
                .WithMessage("**Exit point must be set on the boarder of the board. It can't be set inside the board");
        }

        [Fact]
        public void Ctor_Throws_ArgumentNullException_When_MinesAreLowerThanMinimumRequired()
        {
            //arrange
            var (width, height, mines, exitPoint) = CreateValidBoardModel();
            mines = mines.Take(BoardModel.MIN_MINES_COUNT - 1).ToList().AsReadOnly();

            // act
            Action act = () => new BoardModel(width, height, mines, exitPoint);

            // assert
            act.Should().Throw<ArgumentException>()
                .WithMessage("**minesPoints****has a minumum of mines required**");
        }

        [Fact]
        public void Ctor_Called_Successfully()
        {
            //arrange
            var (width, height, mines, exitPoint) = CreateValidBoardModel();

            // act
            Action act = () => new BoardModel(width, height, mines, exitPoint);

            // assert
            act.Should().NotThrow();
        }

        [Fact]
        [Description("Inserting valid data, the call of this method should validate data present in the board")]
        public void GetPositionStatus_Should_ReturnValidSequenceOfData()
        {
            // Arrange

            // 8 x 9
            int height = 9;
            int width = 8;


            /* 01234567      y
             * ********************
             * FFFFFFMF     (0)
             * FFMFFFFF     (1)
             * FFFFFFFF     (2)
             * FFFMFFME     (3)
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

            // Act
            Action act = () =>
            {
                // test mines
                foreach (var mine in mines)
                {
                    board.GetPositionStatus(mine).Should().Be(TileEnum.Mine);
                }

                // test exit
                board.GetPositionStatus(exitPoint).Should().Be(TileEnum.Exit);

                // test free spaces
                for (int lineIdx = 0; lineIdx < height; lineIdx++)
                {
                    for (int columnIdx = 0; columnIdx < width; columnIdx++)
                    {
                        Point current = new Point(columnIdx, lineIdx);

                        if (exitPoint.X == columnIdx && exitPoint.Y == lineIdx)
                            continue;

                        if (mines.Any(p => p.X == columnIdx && p.Y == lineIdx))
                            continue;

                        board.GetPositionStatus(current).Should().Be(TileEnum.Free);
                    }
                }
            };

            // Assert
            act.Should().NotThrow();
            board.DumpToDiagnostics();
        }
    }
}
