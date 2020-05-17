
using System;
using System.Linq;

namespace pacmac.random
{
    public interface Distribution<T>
    {
        T Distribute(Probability prob);
    }

    public abstract class FiniteDistribution<T> : Distribution<T>
    {
        private T[] _values;
        protected Probability[] _weights;
        public FiniteDistribution(T[] values)
        {
            _values = (T[]) values.Clone();
        }
        public T Distribute(Probability prob)
        {
            Probability it = prob;
            for (int i=0; i<GetNumber(); ++i)
            {
                if (it <= GetWeight(i))
                {
                    /* right one */
                    return GetValue(i);
                }
                it -= GetWeight(i);
            }
            throw new System.ArgumentException("Nope, something's wrong in weights.");
        }


        protected T GetValue(int index) { return _values[index]; }
        protected Probability GetWeight(int index) { return _weights[index]; }
        protected int GetNumber() { return _values.GetLength(0); }
    }


    public class CustomFiniteDistribution<T> : FiniteDistribution<T>
    {
    CustomFiniteDistribution(T[] values, Probability[] weights)
    : base(values)
    {
        _weights = weights;
    }

    }

    public abstract class FiniteRangeIntDistribution : FiniteDistribution<int>
    {
        protected FiniteRangeIntDistribution(int min, int max)
        : base(Enumerable.Range(GetNumber(min, max), min).ToArray())
        {

        }
        protected static int GetNumber(int min, int max) => max - min + 1;
    }


    public class UniformDistribution<T> : FiniteDistribution<T>
    {
        public UniformDistribution(T[] values)
        : base(values)
        {
            PopulateWeights();
        }
        private void PopulateWeights()
        {
            for(int i=0; i<GetNumber(); ++i)
            {
                _weights[i] = (double) 1 / GetNumber();
            }
        }
    }


    public class BernoulliDistribution : FiniteRangeIntDistribution 
    {
        private Probability _p;
        public BernoulliDistribution(Probability p)
        : base(0, 1)
        {
            _p = p;
            PopulateWeights();
        }
        private void PopulateWeights()
        {
            _weights[0] = _p.GetInverseEventProb();
            _weights[1] = _p;
        } 
    }

    public class BinomialDistribution : FiniteRangeIntDistribution
    {
        private Probability _p;
        public BinomialDistribution(Probability p, int n)
        : base(0, n)
        {
            _p = p;
            PopulateWeights();
        }

        private void PopulateWeights()
        {
            for(int k=0; k<GetNumber(); ++k)
            {
                _weights[k] = (double)((Double)MathUtil.BinomialCoefficient(k, GetNumber())
                * Math.Pow(_p.GetValue(), k)
                * Math.Pow(_p.GetInverseEventProb().GetValue(), GetNumber() - k));
            }
        }

    }


    public class HypergeometricDistribution : FiniteRangeIntDistribution
    {
        private int _N;
        private Probability _p;
        public HypergeometricDistribution(Probability p, int n, int N)
        : base(0, n)
        {
            _N = N;
            /*
            * we have to have N*p an integer
            * so we may have to tweak p a bit
            */
            int j = (int) (_N * p.GetValue());
            _p = new Probability(_N / (double)j);
            PopulateWeights();
        }

        private void PopulateWeights()
        {
            for(int k=0; k<GetNumber(); ++k)
            {
                _weights[k] = (double)(MathUtil.BinomialCoefficient(
                    (int)(_N * _p.GetValue()),
                    k
                    )
                    * MathUtil.BinomialCoefficient(
                    (int)(_N * _p.GetInverseEventProb().GetValue()),
                    GetNumber() - k
                    )
                    / MathUtil.BinomialCoefficient(_N, GetNumber())
                );
            }
        }
    }

}