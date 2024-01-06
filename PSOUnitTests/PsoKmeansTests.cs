using KMeans;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using PSOImageSegmentation;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace PSOUnitTests
{
    [TestClass]
    public class PsoKmeansTests
    {
        [TestMethod]
        public void TestMethod1()
        {
            //Arrange
            var image = new Bitmap(Image.FromFile("./toji.jpg"));
            const int numberOfClusters = 3;

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
            //TODO: run PSO

            var psoScore = 0.0;
            var kmeanScore = PSOimage.FitnessFunction(centroidList, clusterList);

            //Assert
            Assert.IsTrue(psoScore <= kmeanScore, "PSO worse than Kmean");
            Console.WriteLine($"KmeanScore: {kmeanScore}");
        }
    }
}