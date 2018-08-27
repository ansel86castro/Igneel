using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Igneel.IA.Resources;
using Igneel.IA.ML.Activations;

namespace Igneel.IA.ML.Evaluators
{    
    public class FFNNetParallelEvaluator : IFFNNetEvaluator
    {        
        private FFNNet net;

        public FFNNet NeuralNet { get => net;}

        IFFNNetActivation activation;
        IFFNNetActivation[] activations;       

        public FFNNetParallelEvaluator(FFNNet net, IFFNNetActivation activation, (int layer, IFFNNetActivation activation)[]activations)
        {
            this.net = net;
            this.activation = activation;

            this.activations = new IFFNNetActivation[net.Layers.Length];
            foreach (var item in activations)
            {
                this.activations[item.layer] = item.activation ?? activation;
            }            
        }


        public unsafe void ComputeOutputs(ComputeBuffer input)
        {
            bool locked = net.IsLocked;
            if (!locked)
                net.Lock();

            var inputLocked = input.IsLocked;
            if (!inputLocked)
                input.Lock();

            try
            {
                var inputView = input.ViewAs<float>();
                for (int i = 0; i < net.Layers.Length; i++)
                {
                    ComputeBufferView<float> outputs;
                    if (net.Layers[i].Neurons > 32)
                        outputs = ComputeLayerOutputsP(inputView, i);
                    else
                        outputs = ComputeLayerOutputsS(inputView, i);

                    inputView = outputs;
                }
            }
            finally
            {
                if (!locked)
                {
                    net.Unlock();
                }

                if (!inputLocked)
                {
                    input.UnLock();
                }
            }
        }

        private unsafe ComputeBufferView<float> ComputeLayerOutputsP(ComputeBufferView<float> inputView, int iLayer)
        {
            var layer = net.Layers[iLayer];
            int pitch = layer.WeightsPerNeurons;
            var weights = layer.Weights.ViewAs<float>();
            var outputs = layer.Outputs.ViewAs<float>();
            var biases = layer.Biases.ViewAs<float>();

            var activation = activations[iLayer] ?? this.activation;

            Parallel.For(0, layer.Neurons, n =>
            {
                outputs[n] = biases[n];
                for (int iweight = 0; iweight < pitch; iweight++)
                {
                    outputs[n] += weights[n * pitch + iweight] * inputView[iweight];
                }
            });

            activation.Eval(layer.Outputs, layer.Outputs, layer.Neurons ,iLayer, net);
            return outputs;
        }

        private unsafe ComputeBufferView<float> ComputeLayerOutputsS(ComputeBufferView<float> inputView, int ilayer)
        {
            var layer = net.Layers[ilayer];
            int pitch = layer.WeightsPerNeurons;
            var weights = layer.Weights.ViewAs<float>();
            var outputs = layer.Outputs.ViewAs<float>();
            var biases = layer.Biases.ViewAs<float>();

            var activation = this.activations[ilayer] ?? this.activation;

            for (int n= 0; n < layer.Neurons; n++)
            {
                outputs[n] = biases[n];
                for (int iweight = 0; iweight < pitch; iweight++)
                {
                    outputs[n] += weights[n * pitch + iweight] * inputView[iweight];
                }
            }
           
            activation.Eval(layer.Outputs, layer.Outputs, layer.Neurons ,ilayer, net);
            return outputs;
        }

        public IFFNNetActivation GetActivation(int layer)
        {
            return activations[layer];
        }
    }
}
