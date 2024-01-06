using System;
using System.Globalization;
using System.Linq;

namespace KMeans
{
    /// <summary>
    /// A data vector class with some distance calculation and helper methods.
    /// 
    /// If you need to use different/custom data structure or distance calculation, it should be fairly easy. 
    /// Since algorithm only cares about the distance function and not about the underlying data structure, 
    /// your custom class can just inherit from DataVec and provide new implementation of GetDistance() method via override.
    /// </summary>
    public class DataVec
    {

        public double[] Components { get; protected set; }

        /// <summary>
        /// Performs a deep copy of DataVec class. 
        /// </summary>
        /// <param name="source">DataVec object to copy</param>
        /// <returns></returns>
        public static DataVec DeepCopy(DataVec source)
        {
            var data = new double[source.Components.Length];
            Array.Copy(source.Components, data, source.Components.Length);
            var ret = new DataVec(data);
            return ret;
        }

        public DataVec()
        {
            Components = Array.Empty<double>();
        }

        public DataVec(double[] data)
        {
            Components = data;
        }

        public DataVec(int dimensions)
        {
            Components = new double[dimensions];
        }

        /// <summary>
        /// Calculates distance between two data points.
        /// </summary>
        /// <param name="other">Other data point</param>
        /// <returns></returns>
        public virtual double GetDistance(DataVec other)
        {
            if (other.Components.Length != Components.Length)
            {
                throw new Exception("Dimension mismatch");
            }
            var diff = new double[Components.Length];
            for (var i = 0; i < diff.Length; ++i)
            {
                diff[i] = other.Components[i] - Components[i];
            }

            return CalculateMagnitude(diff);
        }
        /// <summary>
        /// Print data point. For Debug.
        /// </summary>
        public void Print()
        {
            Console.WriteLine("E: " + ToString());
        }

        /// <summary>
        /// Obtain string representation of component values separated by commas.
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            var str = Components.Aggregate("", (current, t) => current + (t + ","));

            return str.Substring(0, str.Length - 1);
        }

        /// <summary>
        /// Obtain string representation of component values, separated by spaces and aligned.
        /// </summary>
        /// <returns></returns>
        public string ToStringFormatted()
        {
            var str = "";
            const int digits = 25;
            foreach (var t in Components)
            {
                var ln = t.ToString(CultureInfo.InvariantCulture).Length;
                str += t + new string(' ', digits - ln);
            }
            return str.Substring(0, str.Length - 1);
        }


        protected double CalculateMagnitude(double[] data)
        {
            var sumSquared = data.Sum(t => (t * t));

            return Math.Sqrt(sumSquared);
        }
    }


}
