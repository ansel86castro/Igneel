using Igneel.IA.ML.Activations;
using Igneel.IA.Resources;
using System;
using System.Runtime.InteropServices;
using System.Text;

namespace Igneel.IA.ML
{
    public struct FFNNetLayerDesc
    {
        public int Neurons { get; set; }

        public ActivationType? Activation { get; set; }
    }


    public class FFNNetLayer
    {
        ComputeBuffer weights;
        ComputeBuffer outputs;
        ComputeBuffer biases;
        ComputeBuffer lambdas;      
        int neurons;
        int weightsPerNeurons;
        private int elementSize;

        private FFNNetLayer()
        {

        }

        public FFNNetLayer(int neurons, int weightsPerNeurons, IBufferAllocator allocator, Type precission)
        {
            this.neurons = neurons;
            this.weightsPerNeurons = weightsPerNeurons;
            this.elementSize = Marshal.SizeOf(precission);
            weights = allocator.AllocateBuffer(neurons * WeightsPerNeurons* elementSize);
            outputs = allocator.AllocateBuffer(neurons*elementSize);
            biases = allocator.AllocateBuffer(neurons*elementSize);
            lambdas = allocator.AllocateBuffer(neurons * elementSize);
        }

        public ComputeBuffer Weights { get => weights; set => weights = value; }

        public ComputeBuffer Outputs { get => outputs; set => outputs = value; }

        public ComputeBuffer Biases { get => biases; set=>biases = value;}

        public ComputeBuffer Lamdas { get => lambdas; set => lambdas = value; }

        public int Neurons { get => neurons; set => neurons = value; }

        public int WeightsPerNeurons { get => weightsPerNeurons; set => weightsPerNeurons = value; }

        public FFNNetLayer Clone()
        {
            FFNNetLayer clone = new FFNNetLayer();
            clone.weights = weights?.Clone();
            clone.outputs = outputs?.Clone();
            clone.biases = biases?.Clone();
            clone.lambdas = lambdas?.Clone();
            clone.neurons = neurons;
            clone.weightsPerNeurons = weightsPerNeurons;

            return clone;
        }

        public void Lock()
        {
            weights.Lock();
            outputs.Lock();
            biases.Lock();
            lambdas.Lock();
        }

        public void UnLock()
        {
            weights.UnLock();
            outputs.UnLock();
            biases.UnLock();
            lambdas.UnLock();
        }

        public FFNNetLayerDTO ExportDTO()
        {
            FFNNetLayerDTO dto = new FFNNetLayerDTO()
            {
                Neurons = neurons,
                WeightsPerNeuron = weightsPerNeurons,
                WeightsBase64 = GetBase64(weights),
                OutputsBase64 = GetBase64(outputs),
                BiasesBase64 = GetBase64(biases),
                LamdasBase64 = GetBase64(lambdas)                 
            };
            return dto;
        }

        public static FFNNetLayer FromDTO(FFNNetLayerDTO dto, IBufferAllocator allocator, Type precission)
        {
            FFNNetLayer layer = new FFNNetLayer()
            {
                elementSize = Marshal.SizeOf(precission),
                neurons = dto.Neurons,
                weightsPerNeurons = dto.WeightsPerNeuron,

                weights = FromBase64(dto.WeightsBase64, allocator),
                outputs = FromBase64(dto.OutputsBase64, allocator),
                biases = FromBase64(dto.BiasesBase64, allocator),
                lambdas = FromBase64(dto.LamdasBase64, allocator)
            };
            return layer;
        }

        private static string GetBase64(ComputeBuffer buffer)
        {
            byte[] array = new byte[buffer.Lenght];

            var ptr = buffer.Lock();

            ClrRuntime.Runtime.Copy(ptr, array, 0, array.Length);

            buffer.UnLock();

            return Convert.ToBase64String(array);
        }

        private static ComputeBuffer FromBase64(string base64, IBufferAllocator allocator)
        {
            var array = Convert.FromBase64String(base64);
            return allocator.AllocateBuffer(array.Length, array);
        }
    }

    public class FFNNetLayerDTO
    {
        public int Neurons { get; set; }

        public int WeightsPerNeuron { get; set; }

        public string WeightsBase64 { get; set; }

        public string OutputsBase64 { get; set; }

        public string LamdasBase64 { get; set; }

        public string BiasesBase64 { get; set; }
    }
}
