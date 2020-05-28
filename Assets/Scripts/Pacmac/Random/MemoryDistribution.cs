
using System;
using System.Linq;
using System.Collections.Generic;

namespace pacmac.random
{
    /* DECORATOR */

    public class RandomMemoryResult<T>
    {
        private List<T> _results;
        private Distribution<T> _dist;

        public RandomMemoryResult(Distribution<T> dist)
        {
            ResetDistribution(dist);
            _results = new List<T>();
        }

        public void ResetDistribution(Distribution<T> dist)
        {
            _dist = dist;
        }

        public void AddResult(T val)
        {
            _results.Add(val);
        }

        public double MemoryStandardDeviation()
        {
            if(!_results.Any())
            {
                return double.NaN;
            }
            double variation = _results.Aggregate(0.0, (acc, val) => acc + _dist.Variation(val));
            return Math.Sqrt(variation);
        }

        public double MemoryMean()
        {
            if(!_results.Any())
            {
                return double.NaN;
            }
            double mean = _results.Aggregate(0.0, (acc, val) => acc + Distribution<T>.ValToDouble(val)) / _results.Count;
            return mean;
        }
        public double TheoreticalStandardDeviation()
        {
            /* theoritically, there's nothing to do. */
            return _dist.TheoreticalStandardDeviation();
        }
        public double TheoreticalMean()
        {
            return _dist.TheoreticalStandardDeviation();
        }

        public double DiffToMean()
        {
            return MemoryMean() - TheoreticalMean();
        }

        public double DiffToStandardDeviation()
        {
            return MemoryStandardDeviation() - TheoreticalStandardDeviation();
        }

    }
    public class MemoryDistribution<T> : Distribution<T>
    {
        private Distribution<T> _dist;
        private RandomMemoryResult<T> _memory;

        public MemoryDistribution(Distribution<T> dist)
        {
            _dist = dist;
            _memory = new RandomMemoryResult<T>(_dist);
        }

        public MemoryDistribution()
        : this(null)
        {

        }

        public void ResetDistribution(Distribution<T> dist)
        {
            _dist = dist;
            _memory.ResetDistribution(dist);
        }

        override public T Distribute(Probability prob)
        {
            T val = _dist.Distribute(prob);
            /* remember it! */
            _memory.AddResult(val);
            return val;
        }

        public RandomMemoryResult<T> GetMemory()
        {
            return _memory;
        }

        override public double TheoreticalStandardDeviation()
        {
            return _dist.TheoreticalStandardDeviation();
        }
        override public double TheoreticalMean()
        {
            return _dist.TheoreticalMean();
        }

    }
}