using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PSOClusteringAlgorithm
{
    /// <summary>
    /// Naive version(for now) of the Kmeans algorithm
    /// </summary>
    public class KMeansClustering
    {
        /// <summary>
        /// Maximum iterration count
        /// </summary>
        public int tmax { get; set; }
        /// <summary>
        /// The "K" in kmeans
        /// </summary>
        public int ClustersCount { get; set; }
        public List<Point> DataSet { get; set; }

        /// <summary>
        /// Maximum admissible divergence for centroids
        /// </summary>
        public double maxDivergence { get; set; } = 0.0001;

        /// <summary>
        /// Run KMeans with setted parameters.
        /// Using default Kmeans implementation with Forgy init method(in dataset)
        /// </summary>
        public List<Point> RunKMeans()
        {
            Random _rnd = new Random();
            //init centroids
            List<Point> centroids = Enumerable.Range(0, ClustersCount).Select(_ => new Point
            {
                vec = DataSet.ElementAt(_rnd.Next(0, DataSet.Count)).vec.Select(x => x)
            })
                .ToList();

            for (int t = 0; t < tmax; t++)
            {
                //assign dataset to clusters
                var clusters = ClusteringMethods.GetClusters(DataSet, centroids, ClusteringMethods.EuclidianDistance);

                double pointsDifference = 0.0;
                //rellocate centroids
                Parallel.ForEach(clusters, (cluster, state, index) =>
                {
                    //store the mean values for each dimension of Point
                    var mean = Enumerable.Range(0, centroids[(int)index].vec.Count()).Select(x => 0.0).ToArray();
                    //compute the mean value for each dimension
                    for (var i = 0; i < mean.Count(); ++i)
                    {
                        foreach (var point in cluster)
                        {
                            mean[i] += point.vec.ElementAt(i);
                        }
                        mean[i] /= cluster.Count;
                    }

                    //update divergence
                    pointsDifference += ClusteringMethods.EuclidianDistance(centroids[(int)index].vec, mean);

                    //set centroid value as mean value of cluster
                    centroids[(int)index].vec = mean;
                });

                //if divergence is low enough
                if (pointsDifference / clusters.Count < maxDivergence)
                    break;
            }

            return centroids;
        }
    }
}
