using TurtleChallange.Logic.Models;

namespace TurtleChallange.Logic.Tests.TestData.Models
{
    public static class TurtleModelTestData
    {
        public static TurtleModel CreateValidTurtle() => new TurtleModel(new Point(), TurtleHeadOrientation.East);
    }
}
