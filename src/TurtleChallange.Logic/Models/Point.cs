namespace TurtleChallange.Logic.Models
{
    // public seter for deserialize.
    public class Point
    {
        public int X { get; set; }
        public int Y { get; set; }

        public Point()
        {

        }

        public Point(int x, int y)
        {
            X = x;
            Y = y;
        }

        public override string ToString() => $"({X},{Y})";
    }
}
