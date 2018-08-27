using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Igneel.IA.ML.Trainers
{
    public interface INNetTrainer
    {
        IEnumerable<ITrainingExample> TrainingSet { get; set; }

        IEnumerable<ITrainingExample> ValidationSet { get; set; }  

        event Action<ExampleStepArg> ExampleStep;

        event Action<LoopBeginStepArg> LoopBegin;

        event Action<LoopStepEndedArg> LoopEnded;

        void InitializeModel();

        void Train();
    }

    public struct ExampleStepArg
    {
        public INNetTrainer Trainer;

        public ITrainingExample Example;

        public int ExampleIndex;

        public int LoopIndex;

        public int TotalLoops;

        public double Error;     
    }

    public struct LoopBeginStepArg
    {
        public INNetTrainer Trainer;      

        public int LoopIndex;

        public int TotalLoops;       
    }

    public struct LoopStepEndedArg
    {
        public INNetTrainer Trainer;

        public int LoopIndex;

        public int TotalLoops;

        public double TraningSetError;

        public double ValidationSetError;
    }

}
