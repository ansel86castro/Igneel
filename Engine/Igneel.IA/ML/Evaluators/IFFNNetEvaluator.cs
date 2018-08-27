using Igneel.IA.ML.Activations;
using Igneel.IA.Resources;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Igneel.IA.ML.Evaluators
{
    public interface IFFNNetEvaluator
    {
        FFNNet NeuralNet { get; }

        IFFNNetActivation GetActivation(int layer);

        void ComputeOutputs(ComputeBuffer input);
    }
}
