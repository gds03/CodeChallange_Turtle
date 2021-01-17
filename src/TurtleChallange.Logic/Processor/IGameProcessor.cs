using TurtleChallange.Logic.Processor.Models;

namespace TurtleChallange.Logic.Processor
{
    public interface IGameProcessor
    {
        StepResult ProcessStep(TurtleAction step);
    }
}