using Igneel.IA.Resources;

namespace Igneel.IA.ML.Trainers
{
    public interface ITrainingExample
    {
        ComputeBuffer Input { get; }

        ComputeBuffer Output { get; }
    }

}
