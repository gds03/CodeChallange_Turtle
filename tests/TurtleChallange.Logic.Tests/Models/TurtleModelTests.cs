using FluentAssertions;
using System;
using TurtleChallange.Logic.Models;
using Xunit;

namespace TurtleChallange.Logic.Tests.Models
{
    public class TurtleModelTests
    {
        [Fact]
        public void Ctor_Throws_ArgumentNullException_When_Position_IsNull()
        {
            // arrange
            Point position = null;

            // act
            Action act = () => new TurtleModel(position, TurtleHeadOrientation.East);

            // assert
            act.Should().Throw<ArgumentNullException>("**position**");
        }

        [Fact]
        public void RotateRight_Should_RotateToEast()
        {
            // arrange
            TurtleHeadOrientation head = TurtleHeadOrientation.North;
            TurtleModel model = new TurtleModel(new Point(), head);

            // act
            Action act = () => model.RotateRight();

            // assert
            model.Orientation.Should().Be(head);
            act.Should().NotThrow();
            model.Orientation.Should().Be(TurtleHeadOrientation.East);
        }

        [Fact]
        public void RotateRight_Should_RotateToSouth()
        {
            // arrange
            TurtleHeadOrientation head = TurtleHeadOrientation.East;
            TurtleModel model = new TurtleModel(new Point(), head);

            // act
            Action act = () => model.RotateRight();

            // assert
            model.Orientation.Should().Be(head);
            act.Should().NotThrow();
            model.Orientation.Should().Be(TurtleHeadOrientation.South);
        }

        [Fact]
        public void RotateRight_Should_RotateToWest()
        {
            // arrange
            TurtleHeadOrientation head = TurtleHeadOrientation.South;
            TurtleModel model = new TurtleModel(new Point(), head);

            // act
            Action act = () => model.RotateRight();

            // assert
            model.Orientation.Should().Be(head);
            act.Should().NotThrow();
            model.Orientation.Should().Be(TurtleHeadOrientation.West);
        }

        [Fact]
        public void RotateRight_Should_RotateToNorth()
        {
            // arrange
            TurtleHeadOrientation head = TurtleHeadOrientation.West;
            TurtleModel model = new TurtleModel(new Point(), head);

            // act
            Action act = () => model.RotateRight();

            // assert
            model.Orientation.Should().Be(head);
            act.Should().NotThrow();
            model.Orientation.Should().Be(TurtleHeadOrientation.North);
        }
    }
}
