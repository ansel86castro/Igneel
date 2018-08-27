using Igneel.IA.ML.Activations;
using Igneel.IA.Resources;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace Igneel.IA.ML
{
    public class FFNNet
    {   
       
        FFNNetLayer[] layers;     
        IBufferAllocator allocator;
        Type precission;

        private FFNNet()
        {

        }

        public FFNNet(FFNNetLayerDesc[] layers, IBufferAllocator allocator, Type precission)
        {
            this.allocator = allocator;
            this.layers = new FFNNetLayer[layers.Length - 1];
            this.precission = precission;

            var previusLayer = layers[0];

            for (int i = 1; i < layers.Length; i++)
            {
                var layer = layers[i];
                this.layers[i - 1] = new FFNNetLayer(layer.Neurons, previusLayer.Neurons, allocator, precission);
                previusLayer = layer;
            }
        }

        public IBufferAllocator Allocator { get { return allocator; } }

        public FFNNetLayer[] Layers { get { return layers; } }

        public FFNNetLayer OutputLayer { get { return layers[layers.Length - 1]; } }

        public ComputeBuffer Output { get { return OutputLayer.Outputs; } }
      
        public bool IsLocked { get; set; }

        public void Lock()
        {
            if (IsLocked)
                return;

            for (int i = 0; i < layers.Length; i++)
            {
                layers[i].Lock();
            }
            IsLocked = true;                
        }

        public void Unlock()
        {
            if (!IsLocked)
                return;

            for (int i = 0; i < layers.Length; i++)
            {
                layers[i].UnLock();
            }

            IsLocked = false;
        }

        public FFNNet Clone()
        {
            FFNNet net = new FFNNet()
            {              
                allocator = allocator
            };
            net.layers = new FFNNetLayer[layers.Length];
            for (int i = 0; i < layers.Length; i++)
            {
                layers[i] = layers[i].Clone();
            }
            return net;
        }     

        public FFNNetDTO ExportDTO()
        {
            FFNNetDTO dto = new FFNNetDTO
            {
                Precission = precission,
                Layers = layers.Select(x => x.ExportDTO()).ToArray(),
            };
            return dto;
        }

        public static FFNNet FromDTO(FFNNetDTO dto, IBufferAllocator allocator)
        {
            FFNNet net = new FFNNet();
            net.allocator = allocator;
            net.precission = dto.Precission;
            net.layers = new FFNNetLayer[dto.Layers.Length];

            for (int i = 0; i < net.layers.Length; i++)
            {
                net.layers[i] = FFNNetLayer.FromDTO(dto.Layers[i], allocator, net.precission);
            }
            return net;
        }
    }

    public class FFNNetDTO
    {
        public Type Precission { get; set; }

        public FFNNetLayerDTO[] Layers { get; set; }
    }

   
}
