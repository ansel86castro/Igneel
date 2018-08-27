using Igneel.IA.ML.Trainers;
using Igneel.IA.Resources;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace Igneel.IA.ML.DataSets
{
    public class MNISTDataSet: ResourceAllocator, ITrainingSet
    {
        private string trainImageFile;
        private string trainLabelFile;
        private string testImageFile;
        private string testLabelFile;
        private IBufferAllocator allocator;

        float[][] testImages;
        float[][] testLabels;

        float[][] trainImages;
        float[][] trainLabels;

        INNetTrainer trainer;

        ITrainingExample[] trainingCache;
        ITrainingExample[] testCache; 


        public MNISTDataSet(string trainImageFile, string trainLabelFile, string testImageFile, string testLabelFile, IBufferAllocator allocator)
        {
            this.trainImageFile = trainImageFile;
            this.trainLabelFile = trainLabelFile;
            this.testImageFile = testImageFile;
            this.testLabelFile = testLabelFile;
            this.allocator = allocator;
        }

        public unsafe void Load(INNetTrainer trainer)
        {
            trainImages = ReadImagesFile(trainImageFile);
            testImages = ReadImagesFile(testImageFile);
            trainLabels = ReadLabelsFile(trainLabelFile);
            testLabels = ReadLabelsFile(testLabelFile);

            trainingCache = new ITrainingExample[trainImages.Length];
            testCache = new ITrainingExample[testImages.Length];

            for (int i = 0; i < trainingCache.Length; i++)
            {
                var imageData = trainImages[i];
                var labelData = trainLabels[i];

                var imageBuffer = allocator.AllocateBuffer(imageData.Length * sizeof(float), imageData);
                var labelBuffer = allocator.AllocateBuffer(labelData.Length * sizeof(float), labelData);
                trainingCache[i] = new MNISTTrainingExample(imageBuffer, labelBuffer);
            }

            for (int i = 0; i < testCache.Length; i++)
            {
                var imageData = testImages[i];
                var labelData = testLabels[i];

                var imageBuffer = allocator.AllocateBuffer(imageData.Length * sizeof(float), imageData);
                var labelBuffer = allocator.AllocateBuffer(labelData.Length * sizeof(float), labelData);
                testCache[i] = new MNISTTrainingExample(imageBuffer, labelBuffer);
            }

            trainer.TrainingSet = trainingCache;
            trainer.ValidationSet = testCache;

        }

        private static unsafe float[][] ReadImagesFile(string imageFile)
        {
            float[][] images;
            using (BinaryReader reader = new BinaryReader(new FileStream(imageFile, FileMode.Open, FileAccess.Read)))
            {
                var buffer = reader.ReadBytes(sizeof(ImageFileHeader));
                fixed (byte* pBuffer = buffer)
                {
                    ImageFileHeader imageFileHeader = *(ImageFileHeader*)pBuffer;
                    images = new float[imageFileHeader.NbImages][];

                    for (int i = 0; i < imageFileHeader.NbImages; i++)
                    {
                        var imageBuffer = reader.ReadBytes(imageFileHeader.Witdth * imageFileHeader.Heigh);
                        var floatArray = new float[imageBuffer.Length];
                        for (int j = 0; j < floatArray.Length; j++)
                        {
                            floatArray[j] = imageBuffer[j] / 255.0f;
                        }
                        images[i] = floatArray;
                    }
                }
            }
            return images;
        }

        private static unsafe float[][] ReadLabelsFile(string file)
        {
            float[][] labels;
            using (BinaryReader reader = new BinaryReader(new FileStream(file, FileMode.Open, FileAccess.Read)))
            {
                var buffer = reader.ReadBytes(sizeof(ImageFileHeader));
                fixed (byte* pBuffer = buffer)
                {
                    LabelFileHeader labelFileHeader = *(LabelFileHeader*)pBuffer;
                    labels = new float[labelFileHeader.NbLabels][];

                    for (int i = 0; i < labelFileHeader.NbLabels; i++)
                    {
                        var label = reader.ReadByte();
                        float[] output = new float[10];
                        output[label] = 1;
                        labels[i] = output;
                    }
                }
            }
            return labels;
        }

        private void LoadTraningExamples(float[][]images, float[][] labels)
        {

        }    

        [StructLayout(LayoutKind.Sequential)]
        struct ImageFileHeader
        {
            public int MagicNumber;

            public int NbImages;

            public int Witdth;

            public int Heigh;

        }

        [StructLayout(LayoutKind.Sequential)]
        struct LabelFileHeader
        {
            public int MagicNumber;

            public int NbLabels;

        }

        public class MNISTTrainingExample : ITrainingExample
        {
            ComputeBuffer imageBuffer;
            ComputeBuffer labelBuffer;

            public MNISTTrainingExample(ComputeBuffer imageBuffer, ComputeBuffer labelBuffer)
            {
                this.imageBuffer = imageBuffer;
                this.labelBuffer = labelBuffer;
            }

            public ComputeBuffer Input => imageBuffer;

            public ComputeBuffer Output => labelBuffer;
        }
    }    
}
