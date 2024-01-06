using System;
using System.Collections.Generic;

namespace KMeans
{
    /// <summary>
    /// This class represents data point cluster. 
    /// It contains centroid and a list of references to member data points.
    /// </summary>
    public class Cluster
    {
        private static readonly List<int> OccupiedCentroidPositions = new List<int>(); // This is to keep track of which centroid positions are 
                                                                                       // already occupied during random placement

        /// <summary>
        /// Clear cached centroid indices
        /// </summary>
        public static void ResetCache()
        {
            OccupiedCentroidPositions.Clear();
        }
        private DataVec _mLastCentroid;

        /// <summary>
        /// Centre point of a cluster
        /// </summary>
        public DataVec Centroid { get; set; }
        /// <summary>
        /// Members of this cluster
        /// </summary>
        public List<DataVec> Points { get; set; }


        public Cluster()
        {
            const int defaultN = 2;
            Centroid = new DataVec(defaultN);
            Points = new List<DataVec>();
        }

        /// <summary>
        /// Clears references to data points used for centroid recalculation
        /// </summary>
        public void ClearData()
        {
            Points.Clear();
        }

        /// <summary>
        /// Places centroid at the position of a point randomly selected from the data.  
        /// </summary>
        /// <param name="allData"></param>
        public void Initialize(DataVec[] allData)
        {
            int index;
            var cnt = 0;
            var rnd = new Random();
            do
            {
                cnt++;
                if (cnt > 100)
                {
                    throw new Exception("Cannot do centroid placement.");
                }

                index = rnd.Next(allData.Length);

            } while (OccupiedCentroidPositions.Contains(index));

            Centroid = DataVec.DeepCopy(allData[index]);
            OccupiedCentroidPositions.Add(index);
            _mLastCentroid = DataVec.DeepCopy(Centroid);

        }

        /// <summary>
        /// Updates centroid position in respect to cluster data data points
        /// </summary>
        /// <returns></returns>
        public double RecalculateCentroid()
        {

            if (Points.Count == 0)
            {
                return 0;
            }
            var mean = new double[Centroid.Components.Length];
            for (var i = 0; i < mean.Length; ++i)
            {
                foreach (var dataPoint in Points)
                {
                    mean[i] += dataPoint.Components[i];
                }

                mean[i] /= Points.Count;
            }
            _mLastCentroid = DataVec.DeepCopy(Centroid);
            Centroid = new DataVec(mean);
            return Centroid.GetDistance(_mLastCentroid);
        }

        /// <summary>
        /// Dave results as CSV file
        /// </summary>
        /// <param name="path"></param>
        public void SaveAsCsv(string path)
        {
            using (var writer = new System.IO.StreamWriter(path))
            {
                foreach (var point in Points)
                {
                    writer.WriteLine(point.ToString());
                }

                writer.Close();
            }
        }

    }
}
