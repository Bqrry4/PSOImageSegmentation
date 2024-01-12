using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PSOClusteringAlgorithm
{

    /// <summary>
    /// Point vector in n space dimension.
    /// Delegates an enumerable to keep the code clearer
    /// </summary>
    public class Point
    {
        //keeping the point more generic for more dimensions
        public IEnumerable<double> vec;
    }

    public class ClusteringMethods
    {
        #region StaticMethods
        /// <summary>
        /// Computes euclidian distance between two points in n space dimension
        /// </summary>
        /// <param name="zp">First vector</param>
        /// <param name="zw">Second vector</param>
        /// <exception cref="ArgumentException"></exception>
        public static double EuclidianDistance(IEnumerable<double> zp, IEnumerable<double> zw)
        {
            if (zp.Count() != zw.Count())
            {
                throw new ArgumentException("vectors of different dimensions");
            }

            double summ = 0;
            for (int i = 0; i < zp.Count(); ++i)
            {
                var p = zp.ElementAt(i) - zw.ElementAt(i);
                summ += p * p;
            }
            return Math.Sqrt(summ);
        }
        /// <summary>
        /// Compute quantization error for given Centroids and the patterns associated to its clusters
        /// </summary>
        /// <param name="centroids">The clusters Centroids</param>
        /// <param name="clusters">The clusters asociated data</param>
        public static double QuantizationError(IEnumerable<Point> centroids, IEnumerable<IEnumerable<Point>> clusters)
        {
            double summ = 0;
            for (int k = 0; k < clusters.Count(); ++k)
            {
                summ += clusters.ElementAt(k)
                    .Sum(pattern => EuclidianDistance(pattern.vec, centroids.ElementAt(k).vec))
                    / clusters.ElementAt(k).Count();
            }
            return summ / clusters.Count();
        }
        /// <summary>
        /// Maximum average Euclidean distance of Centroids and patterns associated to clusters
        /// </summary>
        /// <param name="centroids">The clusters Centroids</param>
        /// <param name="clusters">The clusters asociated data</param>
        public static double Dmax(IEnumerable<Point> centroids, IEnumerable<IEnumerable<Point>> clusters)
        {
            return centroids
                .Select((centroid, k) => clusters.ElementAt(k).Sum(point => EuclidianDistance(point.vec, centroid.vec)) / clusters.ElementAt(k).Count())
                .Max();
        }
        /// <summary>
        /// Minimum Euclidean distance between any pair of Centroids
        /// </summary>
        /// <param name="centroids">The clusters Centroids</param>
        public static double Dmin(IEnumerable<Point> centroids)
        {
            var distanceMin = Double.MaxValue;
            for (int i = 0; i < centroids.Count(); ++i)
            {
                for (int j = i + 1; j < centroids.Count(); ++j)
                {
                    var distanceBetween = EuclidianDistance(centroids.ElementAt(i).vec, centroids.ElementAt(j).vec);
                    if (distanceBetween < distanceMin)
                        distanceMin = distanceBetween;
                }
            }

            return distanceMin;
        }

        /// <summary>
        /// Minimisation of non-parametric fitness function
        /// </summary>
        /// <param name="centroids">The clusters Centroids</param>
        /// <param name="clusters">The clusters asociated data</param>
        public static double FitnessFunction(IEnumerable<Point> centroids, IEnumerable<IEnumerable<Point>> clusters)
        {
            return (Dmax(centroids, clusters) + QuantizationError(centroids, clusters)) / Dmin(centroids);
        }

        /// <summary>
        /// Get a list of clusters from given Centroids and dataset 
        /// </summary>
        /// <param name="dataset"></param>
        /// <param name="centroids"></param>
        /// <param name="distanceFunc">Function that should compute distance(similarity) between 2 patterns</param>
        /// <returns></returns>
        public static List<List<Point>> GetClusters(IEnumerable<Point> dataset, IEnumerable<Point> centroids, Func<IEnumerable<double>, IEnumerable<double>, double> distanceFunc)
        {
            //clusterCount x pointsInCluster, holds the associated points to a cluster
            var clusters = new List<List<Point>>();
            for (int i = 0; i < centroids.Count(); ++i)
            {
                clusters.Add(new List<Point>());
            }

            //assign each pattern to a cluster
            foreach (var point in dataset)
            {
                //find the closest centroid to pattern
                int idk = centroids
                    .Select((centroid, index) => (id: index, distance: distanceFunc(point.vec, centroid.vec)))
                    .Aggregate((min, current) => min.distance < current.distance ? min : current).id;

                //assign pattern to the closest cluster
                clusters[idk].Add(new Point { vec = point.vec });
            }

            return clusters;
        }
        #endregion
    }
}
