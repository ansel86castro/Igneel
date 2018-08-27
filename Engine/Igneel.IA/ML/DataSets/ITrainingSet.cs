using Igneel.IA.ML.Trainers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Igneel.IA.ML.DataSets
{
    public interface ITrainingSet:IResourceAllocator
    {
        void Load(INNetTrainer trainer);
    }
}
