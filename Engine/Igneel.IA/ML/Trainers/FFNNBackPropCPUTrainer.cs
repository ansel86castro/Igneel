using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Igneel.IA.Resources;
using Igneel.IA.ML.Evaluators;
using Igneel.IA.ML.Activations;

namespace Igneel.IA.ML.Trainers
{
    public class FFNNBackPropCPUTrainer : INNetTrainer
    {
        private FFNNet net;
        private IFFNNetEvaluator evaluator;        
        private IEnumerable<ITrainingExample> trainingSet;
        private IEnumerable<ITrainingExample> validationSet;
        private float step;
        private int nLoops;
        private float maxWeight;
        private float minWeight;
        private volatile bool training;

        public FFNNBackPropCPUTrainer(IFFNNetEvaluator evaluator)
        {
            this.net = evaluator.NeuralNet;

        }

        public IEnumerable<ITrainingExample> TrainingSet { get { return trainingSet; } set { trainingSet = value; } }

        public IEnumerable<ITrainingExample> ValidationSet { get { return validationSet; } set { validationSet = value; } }

        public FFNNet Net{ get { return net; } set { net = value; } }

        public event Action<ExampleStepArg> ExampleStep;

        public event Action<LoopBeginStepArg> LoopBegin;

        public event Action<LoopStepEndedArg> LoopEnded;

        public void CancelTraining()
        {
            training = false;
        }

        public void Train()
        {
            int iloop = 0;
            training = true;

            try
            {
                net.Lock();

                while (iloop < nLoops && training)
                {
                    LoopBegin?.Invoke(new LoopBeginStepArg
                    {
                        LoopIndex = iloop,
                        TotalLoops = nLoops,
                        Trainer = this
                    });

                    int iexample = 0;
                    foreach (var example in trainingSet)
                    {
                        example.Input.Lock();
                        example.Output.Lock();

                        if (!training)
                            break;

                        Train(example);

                        var error = ComputeExampleError(example);

                        ExampleStep?.Invoke(new ExampleStepArg
                        {
                            Example = example,
                            ExampleIndex = iexample,
                            LoopIndex = iloop,
                            TotalLoops = nLoops,
                            Trainer = this,
                            Error = error
                        });

                        example.Input.UnLock();
                        example.Output.UnLock();
                        iexample++;
                    }

                    var trainingSetError = ComputeError(trainingSet);
                    var validationError = ComputeError(validationSet);

                    LoopEnded?.Invoke(new LoopStepEndedArg
                    {
                        LoopIndex = iloop,
                        TotalLoops = nLoops,
                        Trainer = this,
                        TraningSetError = trainingSetError,
                        ValidationSetError = validationError
                    });

                    iloop++;
                }
            }
            finally
            {
                net.Unlock();
            }
        }

        private float ComputeError(IEnumerable<ITrainingExample> trainingSet)
        {
            float d = 0;
            foreach (var example in trainingSet)
            {
                example.Input.Lock();
                example.Output.Lock();

                d += ComputeExampleError(example);

                example.Input.UnLock();
                example.Output.UnLock();
            }

            return d;
        }

        private float ComputeExampleError(ITrainingExample example)
        {
            evaluator.ComputeOutputs(example.Input);

            return 0.5f * DistanceSquare(example.Output, net.Output);
        }

        private float DistanceSquare(ComputeBuffer a, ComputeBuffer b)
        {
            var viewA = a.ViewAs<float>();
            var viewB = b.ViewAs<float>();

            float d = 0;
            for (int i = 0; i < viewA.Lenght; i++)
            {
                 var sub = viewA[i] - viewB[i];
                d += sub * sub;
            }

            return d;
        }

        public void InitializeModel()
        {
            try
            {
                net.Lock();

                foreach (var layer in net.Layers)
                {
                    var weights = layer.Weights.ViewAs<float>();
                    var lamdas = layer.Lamdas.ViewAs<float>();
                    var biases = layer.Biases.ViewAs<float>();
                    var output = layer.Outputs.ViewAs<float>();

                    for (int i = 0; i < weights.Lenght; i++)
                    {
                        weights[i] = Rand.Uniform(minWeight, maxWeight);
                    }

                    for (int i = 0; i < lamdas.Lenght; i++)
                    {
                        lamdas[i] = 0;
                    }

                    for (int i = 0; i < output.Lenght; i++)
                    {
                        output[i] = 0;
                    }

                    for (int i = 0; i < biases.Lenght; i++)
                    {
                        biases[i] = Rand.Uniform(minWeight, maxWeight);
                    }
                }
            }
            finally
            {
                net.Unlock();
            }
        }

        private void Train(ITrainingExample example)
        {
            var exampleOutputView = example.Output.ViewAs<float>();

            evaluator.ComputeOutputs(example.Input);

            var layers = net.Layers;
            var layersCount = layers.Length;
            var outputLayerIdx = layersCount - 1;

            for (int ilayer = outputLayerIdx; ilayer >= 0; ilayer--)
            {
                var layer = layers[ilayer];
                var output = layer.Outputs;
                var lamda = layer.Lamdas;
                var weightsView = layer.Weights.ViewAs<float>();
                var biasesView = layer.Biases.ViewAs<float>();

                var lamdaView = lamda.ViewAs<float>();
                var outputView = output.ViewAs<float>();

                var activation = evaluator.GetActivation(ilayer);
                activation.EvalDerivate(output, lamda, ilayer, layer.Neurons, net);

                if (ilayer == outputLayerIdx)
                {
                    //compute error term Lamda(j)= derivate( Err, net(j) ) for output units
                    Parallel.For(0, layer.Neurons, neuron =>
                    {
                        lamdaView[neuron] *= exampleOutputView[neuron] - outputView[neuron];
                    });                           
                }
                else
                {
                    //compute error term for hidden units
                    var nextLayer = layers[ilayer + 1];
                    DotTranspose(nextLayer.Weights, nextLayer.Lamdas, lamda, nextLayer.Neurons, nextLayer.WeightsPerNeurons);
                }

                var input = ilayer == 0 ? example.Input : layers[ilayer - 1].Outputs;
                var inputView = input.ViewAs<float>();  
                
                Parallel.For(0, layer.Neurons, n =>
                {
                    for (int j = 0; j < layer.WeightsPerNeurons; j++)
                    {
                        weightsView[n * layer.WeightsPerNeurons + j] += step * lamdaView[n] * inputView[j];
                    }

                    biasesView[n] += step * lamdaView[n];

                });                
            }
        }

        private unsafe void DotTranspose(ComputeBuffer weights, ComputeBuffer lamda, ComputeBuffer output ,int neurons, int weightsPerNeurons)
        {
            float* w = (float*)weights.DataPointer;
            float* l = (float*)lamda.DataPointer;
            float* o = (float*)output.DataPointer;
            Parallel.For(0, weightsPerNeurons, c =>
            {
                float r = 0;
                for (int i = 0; i < neurons; i++)
                {
                    r += w[i * weightsPerNeurons + c] * l[i];
                }

                o[c] *= r;
            });
        }

    }
}
