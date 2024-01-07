using System;
using System.Collections.Generic;
using System.Linq;


namespace KMeans
{
    public enum KmsState
    {
        Ok = 0,
        PointsArrayNull = 1,
        DataPointsArrayEmpty = 2,
        MoreCategoriesThanPoints = 3,
        NullEntryInDataPoints = 4,
        DimensionMismatch = 5
    }

    public class KMeansClustering
    {
        private const int MaxIterations = 100;
        private readonly DataVec[] _pDataPoints;
        private readonly Cluster[] _mClusters;

        /// <summary>
        /// Create instance of KMeansClustering classifier.
        /// </summary>
        /// <param name="points">All data points</param>
        /// <param name="k">Number of bins</param>
        public KMeansClustering(DataVec[] points, int k)
        {
            var state = CheckData(points, k);
            if (state != KmsState.Ok)
            {
                throw new Exception("Data check failed. Reason: " + state.ToString());
            }

            Cluster.ResetCache();

            _pDataPoints = points;
            _mClusters = new Cluster[k];
            for (var i = 0; i < k; ++i)
            {
                _mClusters[i] = new Cluster();
                _mClusters[i].Initialize(points);
            }
        }

        /// <summary>
        /// Performs clustering.
        /// </summary>
        /// <param name="maxDiv">Mean acceptable divergence</param>
        /// <returns>Array of clusters, each containing centroid and a list of assigned data points.</returns>
        public Cluster[] Compute(double maxDiv = 0.0001d)
        {
            var iterations = 0;
            while (iterations < MaxIterations)
            {
                iterations++;
                //clear points in clusters
                foreach (var cluster in _mClusters)
                {
                    cluster.ClearData();
                }

                //reassign points in clusters
                foreach (var dataPoint in _pDataPoints)
                {
                    var dist = double.PositiveInfinity;
                    var cluster = 0;
                    for (var iCluster = 0; iCluster < _mClusters.Length; ++iCluster)
                    {
                        var d = _mClusters[iCluster].Centroid.GetDistance(dataPoint);
                        if (!(d < dist)) continue;
                        dist = d;
                        cluster = iCluster;
                    }

                    _mClusters[cluster].Points.Add(dataPoint);
                }

                // recalculate centroids
                var distChanged = _mClusters.Sum(t => t.RecalculateCentroid());

                //Console.WriteLine("Mean error = " + distChanged);
                if (distChanged / _mClusters.Length < maxDiv)
                    break;
            }

            return _mClusters;
        }

        /// <summary>
        /// Prints brief summary of cluster info.
        /// </summary>
        public void PrintClusters()
        {
            Console.WriteLine("Centroids" + new string(' ', 50) + "Members");
            foreach (var cluster in _mClusters)
            {
                var ptTex = cluster.Centroid.ToStringFormatted();
                var diff = 60 - ptTex.Length;
                if (diff < 1) diff = 1;
                ptTex += new string(' ', diff);
                Console.WriteLine(ptTex + " " + cluster.Points.Count.ToString());
            }
        }

        private static KmsState CheckData(IReadOnlyList<DataVec> points, int k)
        {
            if (points == null) return KmsState.PointsArrayNull;
            if (points.Count < 1) return KmsState.DataPointsArrayEmpty;
            if (points.Count < k) return KmsState.MoreCategoriesThanPoints;
            if (points[0] == null) return KmsState.NullEntryInDataPoints;
            int dimensions = points[0].Components.Length;
            for (int i = 1; i < points.Count; ++i)
            {
                if (points[i] == null) return KmsState.NullEntryInDataPoints;
                if (points[i].Components.Length != dimensions) return KmsState.DimensionMismatch;
            }

            return KmsState.Ok;
        }
    }
}