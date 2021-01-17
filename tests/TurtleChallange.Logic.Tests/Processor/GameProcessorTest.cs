using FluentAssertions;
using System;
using TurtleChallange.Logic.Models;
using TurtleChallange.Logic.Processor;
using Xunit;
using static TurtleChallange.Logic.Tests.TestData.GameProcessorTestData;
using static TurtleChallange.Logic.Tests.TestData.Models.TurtleModelTestData;
using static TurtleChallange.Logic.Tests.TestData.Models.BoardModelTestData;
using TurtleChallange.Logic.Tests.Helpers;
using TurtleChallange.Logic.Processor.Models;

namespace TurtleChallange.Logic.Tests.Processor
{
    public class GameProcessorTest
    {
        [Fact]
        public void Ctor_Throws_ArgumentNullException_When_BoardIsNull()
        {
            // Arrange
            BoardModel board = null;
            TurtleModel turtle = CreateValidTurtle();

            // Act
            Action act = () => new GameProcessor(board, turtle);

            // Assert
            act.Should().Throw<ArgumentNullException>("board");
        }

        [Fact]
        public void Ctor_Throws_ArgumentNullException_When_TurtleIsNull()
        {
            // Arrange
            var (width, height, mines, exitPoint) = CreateValidBoardModel();
            BoardModel board = new BoardModel(width, height, mines, exitPoint);
            TurtleModel turtle = null;

            // Act
            Action act = () => new GameProcessor(board, turtle);

            // Assert
            act.Should().Throw<ArgumentNullException>("turtle");
        }

        [Fact]
        public void Ctor_Called_Successfully()
        {
            // Arrange


            // Act
            Action act = () => CreateValidGameProcessor();


            // Assert
            act.Should().NotThrow();
        }


        [Fact]
        public void GameProcessor_Should_PerformCorrectly_ExecutingHappyPath()
        {
            // Arrange
            GameProcessor processor = CreateKnownGame();
            processor.DumpToDiagnostics();

            // Act
            Action act = () =>
            {
                processor.ProcessStep(TurtleAction.Rotate).Should().Be(StepResult.RotatedRight); // head is now west
                processor.ProcessStep(TurtleAction.Rotate).Should().Be(StepResult.RotatedRight); // head is now north
                processor.ProcessStep(TurtleAction.Move).Should().Be(StepResult.MovedStraight);  // head still north moved one step
                processor.ProcessStep(TurtleAction.Rotate).Should().Be(StepResult.RotatedRight); // head is now east
                processor.ProcessStep(TurtleAction.Move).Should().Be(StepResult.MovedStraight);  // head still east moved one step
                processor.ProcessStep(TurtleAction.Move).Should().Be(StepResult.MovedStraight);  // head still east moved one step

                processor.ProcessStep(TurtleAction.Rotate).Should().Be(StepResult.RotatedRight); // head is now south
                processor.ProcessStep(TurtleAction.Move).Should().Be(StepResult.ExitFound);      // head still south moved one step and found exit
            };

            // Assert
            act.Should().NotThrow();
        }

        [Fact]
        public void GameProcessor_Should_PerformCorrectly_HittingTheWallAndExiting()
        {
            // Arrange
            GameProcessor processor = CreateKnownGame();
            processor.DumpToDiagnostics();

            // Act
            Action act = () =>
            {
                processor.ProcessStep(TurtleAction.Rotate).Should().Be(StepResult.RotatedRight); // head is now west
                processor.ProcessStep(TurtleAction.Rotate).Should().Be(StepResult.RotatedRight); // head is now north
                processor.ProcessStep(TurtleAction.Move).Should().Be(StepResult.MovedStraight);  // head still north moved one step
                processor.ProcessStep(TurtleAction.Rotate).Should().Be(StepResult.RotatedRight); // head is now east
                processor.ProcessStep(TurtleAction.Move).Should().Be(StepResult.MovedStraight);  // head still east moved one step
                processor.ProcessStep(TurtleAction.Move).Should().Be(StepResult.MovedStraight);  // head still east moved one step
                processor.ProcessStep(TurtleAction.Move).Should().Be(StepResult.WallHit);        // head still east and hit the wall

                processor.ProcessStep(TurtleAction.Rotate).Should().Be(StepResult.RotatedRight); // head is now south
                processor.ProcessStep(TurtleAction.Move).Should().Be(StepResult.ExitFound);      // head still south moved one step and found exit.
            };

            // Assert
            act.Should().NotThrow();
        }

        [Fact]
        public void GameProcessor_Should_PerformCorrectly_HittingTheMine()
        {
            // Arrange
            GameProcessor processor = CreateKnownGame();
            processor.DumpToDiagnostics();

            // Act
            Action act = () =>
            {
                processor.ProcessStep(TurtleAction.Rotate).Should().Be(StepResult.RotatedRight); // head is now west
                processor.ProcessStep(TurtleAction.Move).Should().Be(StepResult.MovedStraight);  // head still west moved one step
                processor.ProcessStep(TurtleAction.Move).Should().Be(StepResult.MineHit);        // head still west moved one step and hit a mine               
            };

            // Assert
            act.Should().NotThrow();
        }

        [Fact]
        public void GameProcessor_Should_PerformCorrectly_GoStraightToBottomAndHitWall()
        {
            // Arrange
            GameProcessor processor = CreateKnownGame();
            processor.DumpToDiagnostics();

            // Act
            Action act = () =>
            {
                processor.ProcessStep(TurtleAction.Move).Should().Be(StepResult.MovedStraight);  // head still west moved one step
                processor.ProcessStep(TurtleAction.Move).Should().Be(StepResult.MovedStraight);  // head still west moved one step
                processor.ProcessStep(TurtleAction.Move).Should().Be(StepResult.MovedStraight);  // head still west moved one step
                processor.ProcessStep(TurtleAction.Move).Should().Be(StepResult.MovedStraight);  // head still west moved one step
                processor.ProcessStep(TurtleAction.Move).Should().Be(StepResult.MovedStraight);  // head still west moved one step
                processor.ProcessStep(TurtleAction.Move).Should().Be(StepResult.WallHit);  // head still west moved one step
            };

            // Assert
            act.Should().NotThrow();
        }
    }
}
