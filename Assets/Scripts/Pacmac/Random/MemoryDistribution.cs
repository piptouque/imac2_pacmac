
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
            _dist = dist;
            _results = new List<T>();
        }

        public void AddResult(T val)
        {
            _results.Add(val);
        }

        public double MemoryStandardDiviation()
        {
            return _results.Aggregate(0.0, (acc, val) => acc + _dist.Diviation(val));
        }

        public double MemoryMean()
        {
            return _results.Aggregate(0.0, (acc, val) => acc + Distribution<T>.ValToDouble(val)) / _results.Count;
        }
        public double TheoreticalStandardDiviation()
        {
            /* theoritically, there's nothing to do. */
            return _dist.TheoreticalStandardDiviation();
        }
        public double TheoreticalMean()
        {
            return _dist.TheoreticalStandardDiviation();
        }

        public double DiffToMean()
        {
            return MemoryMean() - TheoreticalMean();
        }

        public double DiffToStandardDiviation()
        {
            return MemoryStandardDiviation() - TheoreticalStandardDiviation();
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

        override public double TheoreticalStandardDiviation()
        {
            /* theoritically, there's nothing to do. */
            return _dist.TheoreticalStandardDiviation();
        }
        override public double TheoreticalMean()
        {
            return _dist.TheoreticalMean();
        }

    }
}