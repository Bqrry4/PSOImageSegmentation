using Microsoft.VisualStudio.TestTools.UnitTesting;
using PSOClusteringAlgorithm;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace PSOUnitTests
{
    [TestClass]
    public class PsoKmeansTests
    {
        private const int NumberOfParticles = 10;
        private const int NumberOfClusters = 8;
        private const int NumberOfIterations = 10;
        private const int Repetitions = 15;

        public static IEnumerable<object[]> StrategyCombinations
        {
            get
            {
                return new[]
                {
                    new object[] { 0, 0 },
                    new object[] { 0, 1 },
                    new object[] { 0, 2 },
                    new object[] { 1, 0 },
                    new object[] { 1, 1 },
                    new object[] { 1, 2 },
                    new object[] { 2, 0 },
                    new object[] { 2, 1 },
                    new object[] { 2, 2 },
                };
            }
        }

        [TestMethod]
        [DynamicData(nameof(StrategyCombinations))]
        public void TestToji(int seedStrategy, int socialStrategy)
        {
            var image = new Bitmap(Image.FromFile("./testImages/toji.jpg"));
            var pso = new PSOImageSegmentation(NumberOfClusters, NumberOfParticles, NumberOfIterations);

            pso.GenerateDataSetFromBitmap(image);

            switch (seedStrategy)
            {
                case 0:
                    pso.CentroidSpawner = new SpawnInDomainValues(pso.PointDimensions, pso.DomainLimits);
                    break;
                case 2:
                    pso.CentroidSpawner = new SpawnWithKMeansSeed(pso.DataSet, pso.tmax);
                    break;
                default:
                    pso.CentroidSpawner = new SpawnInDatasetValues(pso.DataSet);
                    break;
            }

            switch (socialStrategy)
            {
                case 1:
                    (pso.VicinityType, pso.VicinitySize) = (SbestType.Social, 3);
                    break;
                case 2:
                    (pso.VicinityType, pso.VicinitySize) = (SbestType.Geographic, 3);
                    break;
                default:
                    (pso.VicinityType, pso.VicinitySize) = (SbestType.Social, 0);
                    break;
            }

            var scores = Enumerable.Range(0, Repetitions).Select(index => pso.RunPSO().Cost).ToList();

            var average = scores.Average();
            var standardDeviation = Math.Sqrt(scores.Select(s => (s - average) * (s - average)).Average());

            Console.WriteLine($"Average: {average}");
            Console.WriteLine($"Standard Deviation: {standardDeviation}");
        }

        [TestMethod]
        [DynamicData(nameof(StrategyCombinations))]
        public void TestLennaGray(int seedStrategy, int socialStrategy)
        {
            //Arrange
            var image = new Bitmap(Image.FromFile("./testImages/Lenna_Gray.png"));
            var pso = new PSOImageSegmentation(NumberOfClusters, NumberOfParticles, NumberOfIterations);

            pso.GenerateDataSetFromBitmap(image);

            switch (seedStrategy)
            {
                case 0:
                    pso.CentroidSpawner = new SpawnInDomainValues(pso.PointDimensions, pso.DomainLimits);
                    break;
                case 2:
                    pso.CentroidSpawner = new SpawnWithKMeansSeed(pso.DataSet, pso.tmax);
                    break;
                default:
                    pso.CentroidSpawner = new SpawnInDatasetValues(pso.DataSet);
                    break;
            }

            switch (socialStrategy)
            {
                case 1:
                    (pso.VicinityType, pso.VicinitySize) = (SbestType.Social, 3);
                    break;
                case 2:
                    (pso.VicinityType, pso.VicinitySize) = (SbestType.Geographic, 3);
                    break;
                default:
                    (pso.VicinityType, pso.VicinitySize) = (SbestType.Social, 0);
                    break;
            }

            var scores = Enumerable.Range(0, Repetitions).Select(_ => pso.RunPSO().Cost).ToList();

            var average = scores.Average();
            var standardDeviation = Math.Sqrt(scores.Select(s => (s - average) * (s - average)).Average());

            Console.WriteLine($"Average: {average}");
            Console.WriteLine($"Standard Deviation: {standardDeviation}");
        }


        [TestMethod]
        [DynamicData(nameof(StrategyCombinations))]
        public void TestLennaColor(int seedStrategy, int socialStrategy)
        {
            //Arrange
            var image = new Bitmap(Image.FromFile("./testImages/Lenna_Color.png"));
            var pso = new PSOImageSegmentation(NumberOfClusters, NumberOfParticles, NumberOfIterations);

            pso.GenerateDataSetFromBitmap(image);

            switch (seedStrategy)
            {
                case 0:
                    pso.CentroidSpawner = new SpawnInDomainValues(pso.PointDimensions, pso.DomainLimits);
                    break;
                case 2:
                    pso.CentroidSpawner = new SpawnWithKMeansSeed(pso.DataSet, pso.tmax);
                    break;
                default:
                    pso.CentroidSpawner = new SpawnInDatasetValues(pso.DataSet);
                    break;
            }

            switch (socialStrategy)
            {
                case 1:
                    (pso.VicinityType, pso.VicinitySize) = (SbestType.Social, 3);
                    break;
                case 2:
                    (pso.VicinityType, pso.VicinitySize) = (SbestType.Geographic, 3);
                    break;
                default:
                    (pso.VicinityType, pso.VicinitySize) = (SbestType.Social, 0);
                    break;
            }

            var scores = Enumerable.Range(0, Repetitions).Select(_ => pso.RunPSO().Cost).ToList();

            var average = scores.Average();
            var standardDeviation = Math.Sqrt(scores.Select(s => (s - average) * (s - average)).Average());

            Console.WriteLine($"Average: {average}");
            Console.WriteLine($"Standard Deviation: {standardDeviation}");
        }


        [TestMethod]
        [DynamicData(nameof(StrategyCombinations))]
        public void TestPeppers(int seedStrategy, int socialStrategy)
        {
            //Arrange
            var image = new Bitmap(Image.FromFile("./testImages/Peppers_Color.jpg"));
            var pso = new PSOImageSegmentation(NumberOfClusters, NumberOfParticles, NumberOfIterations);

            pso.GenerateDataSetFromBitmap(image);

            switch (seedStrategy)
            {
                case 0:
                    pso.CentroidSpawner = new SpawnInDomainValues(pso.PointDimensions, pso.DomainLimits);
                    break;
                case 2:
                    pso.CentroidSpawner = new SpawnWithKMeansSeed(pso.DataSet, pso.tmax);
                    break;
                default:
                    pso.CentroidSpawner = new SpawnInDatasetValues(pso.DataSet);
                    break;
            }

            switch (socialStrategy)
            {
                case 1:
                    (pso.VicinityType, pso.VicinitySize) = (SbestType.Social, 3);
                    break;
                case 2:
                    (pso.VicinityType, pso.VicinitySize) = (SbestType.Geographic, 3);
                    break;
                default:
                    (pso.VicinityType, pso.VicinitySize) = (SbestType.Social, 0);
                    break;
            }

            var scores = Enumerable.Range(0, Repetitions).Select(index => pso.RunPSO().Cost).ToList();

            var average = scores.Average();
            var standardDeviation = Math.Sqrt(scores.Select(s => (s - average) * (s - average)).Average());

            Console.WriteLine($"Average: {average}");
            Console.WriteLine($"Standard Deviation: {standardDeviation}");
        }

        
        //[TestMethod]
        [DynamicData(nameof(StrategyCombinations))]
        public void TestImportant(int seedStrategy, int socialStrategy)
        {
            //Arrange
            var image = new Bitmap(Image.FromFile(".testImages/Important_Color.jpg"));
            var pso = new PSOImageSegmentation(NumberOfClusters, NumberOfParticles, NumberOfIterations);

            pso.GenerateDataSetFromBitmap(image);

            switch (seedStrategy)
            {
                case 0:
                    pso.CentroidSpawner = new SpawnInDomainValues(pso.PointDimensions, pso.DomainLimits);
                    break;
                case 2:
                    pso.CentroidSpawner = new SpawnWithKMeansSeed(pso.DataSet, pso.tmax);
                    break;
                default:
                    pso.CentroidSpawner = new SpawnInDatasetValues(pso.DataSet);
                    break;
            }

            switch (socialStrategy)
            {
                case 1:
                    (pso.VicinityType, pso.VicinitySize) = (SbestType.Social, 3);
                    break;
                case 2:
                    (pso.VicinityType, pso.VicinitySize) = (SbestType.Geographic, 3);
                    break;
                default:
                    (pso.VicinityType, pso.VicinitySize) = (SbestType.Social, 0);
                    break;
            }

            var scores = Enumerable.Range(0, Repetitions).Select(_ => pso.RunPSO().Cost).ToList();

            var average = scores.Average();
            var standardDeviation = Math.Sqrt(scores.Select(s => (s - average) * (s - average)).Average());

            Console.WriteLine($"Average: {average}");
            Console.WriteLine($"Standard Deviation: {standardDeviation}");
        }


        //[TestMethod]
        [DynamicData(nameof(StrategyCombinations))]
        public void TestStroheim(int seedStrategy, int socialStrategy)
        {
            //Arrange
            var image = new Bitmap(Image.FromFile("./Stroheim_Color.png"));
            var pso = new PSOImageSegmentation(NumberOfClusters, NumberOfParticles, NumberOfIterations);

            pso.GenerateDataSetFromBitmap(image);

            switch (seedStrategy)
            {
                case 0:
                    pso.CentroidSpawner = new SpawnInDomainValues(pso.PointDimensions, pso.DomainLimits);
                    break;
                case 2:
                    pso.CentroidSpawner = new SpawnWithKMeansSeed(pso.DataSet, pso.tmax);
                    break;
                default:
                    pso.CentroidSpawner = new SpawnInDatasetValues(pso.DataSet);
                    break;
            }

            switch (socialStrategy)
            {
                case 1:
                    (pso.VicinityType, pso.VicinitySize) = (SbestType.Social, 3);
                    break;
                case 2:
                    (pso.VicinityType, pso.VicinitySize) = (SbestType.Geographic, 3);
                    break;
                default:
                    (pso.VicinityType, pso.VicinitySize) = (SbestType.Social, 0);
                    break;
            }

            var scores = Enumerable.Range(0, Repetitions).Select(_ => pso.RunPSO().Cost).ToList();

            var average = scores.Average();
            var standardDeviation = Math.Sqrt(scores.Select(s => (s - average) * (s - average)).Average());

            Console.WriteLine($"Average: {average}");
            Console.WriteLine($"Standard Deviation: {standardDeviation}");
        }
    }
}