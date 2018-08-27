using Igneel.IA.Resources;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Igneel.IA.ML.Activations
{
    public enum ActivationType
    {
        None,
        Logistic,
        Relu,
        SoftMax,
        Tanh
    }

    public interface IFFNNetActivation
    {
        void Eval(ComputeBuffer input, ComputeBuffer output, int neurons, int layer, FFNNet net);

        void EvalDerivate(ComputeBuffer input, ComputeBuffer output, int layer, int neurons, FFNNet net);
        //void EvalDerivate(ComputeBuffer input, ComputeBuffer output, int neuron, int layer, FFNNet net);
    }

    public class SigmoidF : IFFNNetActivation
    {
        public unsafe void Eval(ComputeBuffer input, ComputeBuffer output, int neurons, int layer, FFNNet net)
        {
            float *x = (float*)input.DataPointer;
            float *y = (float*)output.DataPointer;
          
            for (int i = 0; i < neurons; i++)
            {
                y[i] = 1.0f / (1 + (float)Math.Exp(-x[i]));
            }            
        }

        public unsafe void EvalDerivate(ComputeBuffer input, ComputeBuffer output, int neurons, int layer, FFNNet net)
        {
            for (int neuron = 0; neuron < neurons; neuron++)
            {
                float x = ((float*)input.DataPointer)[neuron];
                ((float*)output.DataPointer)[neuron] = x * (1 - x);
            }
        }
    }

    public class SigmoidParallelF : IFFNNetActivation
    {
        public unsafe void Eval(ComputeBuffer input, ComputeBuffer output, int neurons, int layer, FFNNet net)
        {
            float* x = (float*)input.DataPointer;
            float* y = (float*)output.DataPointer;

            Parallel.For(0, neurons, i =>
            {
                y[i] = 1.0f / (1 + (float)Math.Exp(-x[i]));
            });          
        }

        public unsafe void EvalDerivate(ComputeBuffer input, ComputeBuffer output, int neurons, int layer, FFNNet net)
        {
            float* x = (float*)input.DataPointer;
            float* y = (float*)output.DataPointer;

            Parallel.For(0, neurons, neuron=>
            {                
                y[neuron] = x[neuron] * (1 - x[neuron]);
            });          
        }
    }

    public class SoftMaxF : IFFNNetActivation
    {
        public unsafe void Eval(ComputeBuffer input, ComputeBuffer output, int neurons, int layer, FFNNet net)
        {
            float* x = ((float*)input.DataPointer);
            float* y = ((float*)output.DataPointer);        

            var sumExp = 0.0f;
            int i;

            for (i = 0; i < neurons; i++)
            {
                y[i] = (float)Math.Exp(x[i]);
                sumExp += y[i];
            }

            for (i = 0; i < neurons; i++)
            {
                y[i] /= sumExp;
            }
            
        }

        public void EvalDerivate(ComputeBuffer input, ComputeBuffer output, int neurons, int layer, FFNNet net)
        {
            throw new NotImplementedException();
        }
    }   


}
