namespace TurtleChallange.Logic.Tests
{
    public class TestsLogger : IExternalLogger
    {
        public void Log(string contents)
        {
            System.Diagnostics.Trace.WriteLine(contents);
        }
    }
}
