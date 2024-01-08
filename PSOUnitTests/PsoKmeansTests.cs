using KMeans;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using PSOImageSegmentation;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;

namespace PSOUnitTests
{
    [TestClass]
    public class PsoKmeansTests
    {
        [TestMethod]
        [DataRow(4)]
        [DataRow(8)]
        [DataRow(12)]
        public void TestToji(int numberOfClusters)
        {
            //Arrange
            var image = new Bitmap(Image.FromFile("./toji.jpg"));
            image = PSOimage.MakeGrayscale3(image);
            var dataPoints = new List<DataVec>();
            for (var x = 0; x < image.Width; ++x)
            {
                for (var y = 0; y < image.Height; ++y)
                {
                    var pixel = image.GetPixel(x, y);
                    dataPoints.Add(new DataVec(new double[] { x, y, pixel.R, pixel.G, pixel.B }));
                }
            }

            var kmeans = new KMeansClustering(dataPoints.ToArray(), numberOfClusters);
            var pso = new PSOimage(numberOfClusters);
            pso.GenerateDataSetFromBitmap(image);

            //Act
            var clusters = kmeans.Compute();
            //convert Kmeans clusters
            var centroidList = new List<PSOimage.Point>();
            var clusterList = new List<List<PSOimage.Point>>();
            foreach (var cluster in clusters)
            {
                centroidList.Add(new PSOimage.Point
                {
                    vec = cluster.Centroid.Components
                });
                clusterList.Add(cluster.Points.Select(x => new PSOimage.Point { vec = x.Components }).ToList());
            }


            var (psoImage, psoScore) = pso.RunPSO();
            var kmeanScore = PSOimage.FitnessFunction(centroidList, clusterList);

            //write image
            var kmeanImage = new Bitmap(image.Width, image.Height);
            foreach (var cluster in clusters)
            {
                foreach (var clusterPoint in cluster.Points)
                {
                    kmeanImage.SetPixel(
                        (int)clusterPoint.Components[0], 
                        (int)clusterPoint.Components[1],
                        Color.FromArgb(
                            (int)cluster.Centroid.Components[2], 
                            (int)cluster.Centroid.Components[3],
                            (int)cluster.Centroid.Components[4]
                            ));
                }
            }
            System.IO.Directory.CreateDirectory("./TestResults/Toji");
            kmeanImage.Save($"./TestResults/Toji/KmeanToji{numberOfClusters}.png", ImageFormat.Png);
            psoImage.Save($"./TestResults/Toji/PSOToji{numberOfClusters}.png", ImageFormat.Png);

            //Assert
            Assert.IsTrue(psoScore <= kmeanScore, $"PSO worse than Kmean : {psoScore} > {kmeanScore}");
            Console.WriteLine($"KmeanScore: {kmeanScore}");
            Console.WriteLine($"PsoScore: {psoScore}");
        }

        [TestMethod]
        [DataRow(4)]
        [DataRow(8)]
        [DataRow(12)]
        public void TestLennaGray(int numberOfClusters)
        {
            //Arrange
            var image = new Bitmap(Image.FromFile("./Lenna_Gray.png"));
            var dataPoints = new List<DataVec>();
            for (var x = 0; x < image.Width; ++x)
            {
                for (var y = 0; y < image.Height; ++y)
                {
                    var pixel = image.GetPixel(x, y);
                    dataPoints.Add(new DataVec(new double[] { x, y, pixel.R, pixel.G, pixel.B }));
                }
            }

            var kmeans = new KMeansClustering(dataPoints.ToArray(), numberOfClusters);
            var pso = new PSOimage(numberOfClusters);
            pso.GenerateDataSetFromBitmap(image);

            //Act
            var clusters = kmeans.Compute();
            //convert Kmeans clusters
            var centroidList = new List<PSOimage.Point>();
            var clusterList = new List<List<PSOimage.Point>>();
            foreach (var cluster in clusters)
            {
                centroidList.Add(new PSOimage.Point
                {
                    vec = cluster.Centroid.Components
                });
                clusterList.Add(cluster.Points.Select(x => new PSOimage.Point { vec = x.Components }).ToList());
            }


            var (psoImage, psoScore) = pso.RunPSO();
            var kmeanScore = PSOimage.FitnessFunction(centroidList, clusterList);

            //write image
            var kmeanImage = new Bitmap(image.Width, image.Height);
            foreach (var cluster in clusters)
            {
                foreach (var clusterPoint in cluster.Points)
                {
                    kmeanImage.SetPixel(
                        (int)clusterPoint.Components[0], 
                        (int)clusterPoint.Components[1],
                        Color.FromArgb(
                            (int)cluster.Centroid.Components[2], 
                            (int)cluster.Centroid.Components[3],
                            (int)cluster.Centroid.Components[4]
                            ));
                }
            }
            System.IO.Directory.CreateDirectory("./TestResults/Lenna_Gray");
            kmeanImage.Save($"./TestResults/Lenna_Gray/KmeanLenna{numberOfClusters}.png", ImageFormat.Png);
            psoImage.Save($"./TestResults/Lenna_Gray/PsoLenna{numberOfClusters}.png", ImageFormat.Png);

            //Assert
            Assert.IsTrue(psoScore <= kmeanScore, $"PSO worse than Kmean : {psoScore} > {kmeanScore}");
            Console.WriteLine($"KmeanScore: {kmeanScore}");
            Console.WriteLine($"PsoScore: {psoScore}");
        }

        [TestMethod]
        [DataRow(4)]
        [DataRow(8)]
        [DataRow(12)]
        public void TestLennaColor(int numberOfClusters)
        {
            //Arrange
            var image = new Bitmap(Image.FromFile("./Lenna_Color.png"));
            var dataPoints = new List<DataVec>();
            for (var x = 0; x < image.Width; ++x)
            {
                for (var y = 0; y < image.Height; ++y)
                {
                    var pixel = image.GetPixel(x, y);
                    dataPoints.Add(new DataVec(new double[] { x, y, pixel.R, pixel.G, pixel.B }));
                }
            }

            var kmeans = new KMeansClustering(dataPoints.ToArray(), numberOfClusters);
            var pso = new PSOimage(numberOfClusters);
            pso.GenerateDataSetFromBitmap(image);

            //Act
            var clusters = kmeans.Compute();
            //convert Kmeans clusters
            var centroidList = new List<PSOimage.Point>();
            var clusterList = new List<List<PSOimage.Point>>();
            foreach (var cluster in clusters)
            {
                centroidList.Add(new PSOimage.Point
                {
                    vec = cluster.Centroid.Components
                });
                clusterList.Add(cluster.Points.Select(x => new PSOimage.Point { vec = x.Components }).ToList());
            }


            var (psoImage, psoScore) = pso.RunPSO();
            var kmeanScore = PSOimage.FitnessFunction(centroidList, clusterList);

            //write image
            var kmeanImage = new Bitmap(image.Width, image.Height);
            foreach (var cluster in clusters)
            {
                foreach (var clusterPoint in cluster.Points)
                {
                    kmeanImage.SetPixel(
                        (int)clusterPoint.Components[0], 
                        (int)clusterPoint.Components[1],
                        Color.FromArgb(
                            (int)cluster.Centroid.Components[2], 
                            (int)cluster.Centroid.Components[3],
                            (int)cluster.Centroid.Components[4]
                            ));
                }
            }
            System.IO.Directory.CreateDirectory("./TestResults/Lenna_Color");
            kmeanImage.Save($"./TestResults/Lenna_Color/KmeanLenna{numberOfClusters}.png", ImageFormat.Png);
            psoImage.Save($"./TestResults/Lenna_Color/PsoLenna{numberOfClusters}.png", ImageFormat.Png);

            //Assert
            Assert.IsTrue(psoScore <= kmeanScore, $"PSO worse than Kmean : {psoScore} > {kmeanScore}");
            Console.WriteLine($"KmeanScore: {kmeanScore}");
            Console.WriteLine($"PsoScore: {psoScore}");
        }

        [TestMethod]
        [DataRow(4)]
        [DataRow(8)]
        [DataRow(12)]
        public void TestPeppers(int numberOfClusters)
        {
            //Arrange
            var image = new Bitmap(Image.FromFile("./Peppers_Color.jpg"));
            var dataPoints = new List<DataVec>();
            for (var x = 0; x < image.Width; ++x)
            {
                for (var y = 0; y < image.Height; ++y)
                {
                    var pixel = image.GetPixel(x, y);
                    dataPoints.Add(new DataVec(new double[] { x, y, pixel.R, pixel.G, pixel.B }));
                }
            }

            var kmeans = new KMeansClustering(dataPoints.ToArray(), numberOfClusters);
            var pso = new PSOimage(numberOfClusters);
            pso.GenerateDataSetFromBitmap(image);

            //Act
            var clusters = kmeans.Compute();
            //convert Kmeans clusters
            var centroidList = new List<PSOimage.Point>();
            var clusterList = new List<List<PSOimage.Point>>();
            foreach (var cluster in clusters)
            {
                centroidList.Add(new PSOimage.Point
                {
                    vec = cluster.Centroid.Components
                });
                clusterList.Add(cluster.Points.Select(x => new PSOimage.Point { vec = x.Components }).ToList());
            }


            var (psoImage, psoScore) = pso.RunPSO();
            var kmeanScore = PSOimage.FitnessFunction(centroidList, clusterList);

            //write image
            var kmeanImage = new Bitmap(image.Width, image.Height);
            foreach (var cluster in clusters)
            {
                foreach (var clusterPoint in cluster.Points)
                {
                    kmeanImage.SetPixel(
                        (int)clusterPoint.Components[0], 
                        (int)clusterPoint.Components[1],
                        Color.FromArgb(
                            (int)cluster.Centroid.Components[2], 
                            (int)cluster.Centroid.Components[3],
                            (int)cluster.Centroid.Components[4]
                            ));
                }
            }
            System.IO.Directory.CreateDirectory("./TestResults/Peppers");
            kmeanImage.Save($"./TestResults/Peppers/KmeanPeppers{numberOfClusters}.png", ImageFormat.Png);
            psoImage.Save($"./TestResults/Peppers/PsoPeppers{numberOfClusters}.png", ImageFormat.Png);

            //Assert
            Assert.IsTrue(psoScore <= kmeanScore, $"PSO worse than Kmean : {psoScore} > {kmeanScore}");
            Console.WriteLine($"KmeanScore: {kmeanScore}");
            Console.WriteLine($"PsoScore: {psoScore}");
        }

        [TestMethod]
        [DataRow(4)]
        [DataRow(8)]
        [DataRow(12)]
        [DataRow(16)]
        public void TestImportant(int numberOfClusters)
        {
            //Arrange
            var image = new Bitmap(Image.FromFile("./Important_Color.jpg"));
            //image = PSOimage.MakeGrayscale3(image);
            var dataPoints = new List<DataVec>();
            for (var x = 0; x < image.Width; ++x)
            {
                for (var y = 0; y < image.Height; ++y)
                {
                    var pixel = image.GetPixel(x, y);
                    dataPoints.Add(new DataVec(new double[] { x, y, pixel.R, pixel.G, pixel.B }));
                }
            }

            var kmeans = new KMeansClustering(dataPoints.ToArray(), numberOfClusters);
            var pso = new PSOimage(numberOfClusters);
            pso.GenerateDataSetFromBitmap(image);

            //Act
            var clusters = kmeans.Compute();
            //convert Kmeans clusters
            var centroidList = new List<PSOimage.Point>();
            var clusterList = new List<List<PSOimage.Point>>();
            foreach (var cluster in clusters)
            {
                centroidList.Add(new PSOimage.Point
                {
                    vec = cluster.Centroid.Components
                });
                clusterList.Add(cluster.Points.Select(x => new PSOimage.Point { vec = x.Components }).ToList());
            }


            var (psoImage, psoScore) = pso.RunPSO();
            var kmeanScore = PSOimage.FitnessFunction(centroidList, clusterList);

            //write image
            var kmeanImage = new Bitmap(image.Width, image.Height);
            foreach (var cluster in clusters)
            {
                foreach (var clusterPoint in cluster.Points)
                {
                    kmeanImage.SetPixel(
                        (int)clusterPoint.Components[0], 
                        (int)clusterPoint.Components[1],
                        Color.FromArgb(
                            (int)cluster.Centroid.Components[2], 
                            (int)cluster.Centroid.Components[3],
                            (int)cluster.Centroid.Components[4]
                            ));
                }
            }
            System.IO.Directory.CreateDirectory("./TestResults/Important");
            kmeanImage.Save($"./TestResults/Important/KmeanImportant{numberOfClusters}.png", ImageFormat.Png);
            psoImage.Save($"./TestResults/Important/PSOImportant{numberOfClusters}.png", ImageFormat.Png);

            //Assert
            Assert.IsTrue(psoScore <= kmeanScore, $"PSO worse than Kmean : {psoScore} > {kmeanScore}");
            Console.WriteLine($"KmeanScore: {kmeanScore}");
            Console.WriteLine($"PsoScore: {psoScore}");
        }

        
        [TestMethod]
        [DataRow(4)]
        [DataRow(8)]
        [DataRow(12)]
        [DataRow(16)]
        [DataRow(20)]
        public void TestStroheim(int numberOfClusters)
        {
            //Arrange
            var image = new Bitmap(Image.FromFile("./Stroheim_Color.png"));
            //image = PSOimage.MakeGrayscale3(image);
            var dataPoints = new List<DataVec>();
            for (var x = 0; x < image.Width; ++x)
            {
                for (var y = 0; y < image.Height; ++y)
                {
                    var pixel = image.GetPixel(x, y);
                    dataPoints.Add(new DataVec(new double[] { x, y, pixel.R, pixel.G, pixel.B }));
                }
            }

            var kmeans = new KMeansClustering(dataPoints.ToArray(), numberOfClusters);
            var pso = new PSOimage(numberOfClusters);
            pso.GenerateDataSetFromBitmap(image);

            //Act
            var clusters = kmeans.Compute();
            //convert Kmeans clusters
            var centroidList = new List<PSOimage.Point>();
            var clusterList = new List<List<PSOimage.Point>>();
            foreach (var cluster in clusters)
            {
                centroidList.Add(new PSOimage.Point
                {
                    vec = cluster.Centroid.Components
                });
                clusterList.Add(cluster.Points.Select(x => new PSOimage.Point { vec = x.Components }).ToList());
            }


            var (psoImage, psoScore) = pso.RunPSO();
            var kmeanScore = PSOimage.FitnessFunction(centroidList, clusterList);

            //write image
            var kmeanImage = new Bitmap(image.Width, image.Height);
            foreach (var cluster in clusters)
            {
                foreach (var clusterPoint in cluster.Points)
                {
                    kmeanImage.SetPixel(
                        (int)clusterPoint.Components[0], 
                        (int)clusterPoint.Components[1],
                        Color.FromArgb(
                            (int)cluster.Centroid.Components[2], 
                            (int)cluster.Centroid.Components[3],
                            (int)cluster.Centroid.Components[4]
                            ));
                }
            }
            System.IO.Directory.CreateDirectory("./TestResults/Stroheim");
            kmeanImage.Save($"./TestResults/Stroheim/KmeanStroheim{numberOfClusters}.png", ImageFormat.Png);
            psoImage.Save($"./TestResults/Stroheim/PSOStroheim{numberOfClusters}.png", ImageFormat.Png);

            //Assert
            Assert.IsTrue(psoScore <= kmeanScore, $"PSO worse than Kmean : {psoScore} > {kmeanScore}");
            Console.WriteLine($"KmeanScore: {kmeanScore}");
            Console.WriteLine($"PsoScore: {psoScore}");
        }
    }
}