
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
        protected Probability[] _quantileValues;
        public FiniteDistribution(T[] values)
        {
            _values = (T[]) values.Clone();
        }
        public T Distribute(Probability prob)
        {
            /*
             * see inverse distribution function
             * or quantive function
             */
            for (int i=0; i<GetNumber(); ++i)
            {
                if (prob > GetQuantileValue(i))
                {
                    /* right one */
                    return GetValue(i);
                }
            }
            throw new System.ArgumentException("Nope, something's wrong with the quantile function.");
        }


        protected T GetValue(int index) { return _values[index]; }
        protected Probability GetQuantileValue(int index) { return _quantileValues[index]; }
        protected int GetNumber() { return _values.GetLength(0); }

    }


    public class CustomFiniteDistribution<T> : FiniteDistribution<T>
    {
        CustomFiniteDistribution(T[] values, Probability[] weights)
        : base(values)
        {
            _quantileValues = weights;
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

    public class UniformRangeIntDistribution : FiniteRangeIntDistribution
    {
        public UniformRangeIntDistribution(int min, int max)
        : base(min, max)
        {

        }
        private void Distribute()
        {
            for(int i=0; i<GetNumber(); ++i)
            {
                _quantileValues[i] = (double) (i+1) / GetNumber();
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
            PopulateQuantileValues();
        }
        private void PopulateQuantileValues()
        {
            _quantileValues[0] = _p.GetInverseEventProb();
            _quantileValues[1] = 1.0;
        } 
    }

    public class BinomialDistribution : FiniteRangeIntDistribution
    {
        private Probability _p;
        public BinomialDistribution(Probability p, int n)
        : base(0, n)
        {
            _p = p;
            PopulateQuantileValues();
        }

        private void PopulateQuantileValues()
        {
            for (int k=0; k<GetNumber() - 1; ++k)
            {
                _quantileValues[k] = (double)((Double)MathUtil.BinomialCoefficient(k, GetNumber())
                * Math.Pow(_p.GetValue(), k)
                * Math.Pow(_p.GetInverseEventProb().GetValue(), GetNumber() - k));
                if (k > 0)
                {
                    _quantileValues[k] += _quantileValues[k - 1];
                }
            }
            _quantileValues[GetNumber()-1] = 1.0;
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
            PopulateQuantileValues();
        }

        private void PopulateQuantileValues()
        {
            for (int k=0; k<GetNumber()-1; ++k)
            {
                _quantileValues[k] = (double)(MathUtil.BinomialCoefficient(
                    (int)(_N * _p.GetValue()),
                    k
                    )
                    * MathUtil.BinomialCoefficient(
                    (int)(_N * _p.GetInverseEventProb().GetValue()),
                    GetNumber() - k
                    )
                    / MathUtil.BinomialCoefficient(_N, GetNumber())
                );
                if (k > 1)
                {
                    _quantileValues[k] += _quantileValues[k - 1];
                }
            }
            _quantileValues[GetNumber()-1] = 1.0;
        }
    }

    public class MultinomialDistribution : FiniteDistribution<int[]>
    {
        private int _n;
        private Probability[] _ps;

        MultinomialDistribution(Probability[] ps, int n)
        : base(GeneratePermutations(ps.GetLength(0), n))
        {
            _n = n;
            _ps = ps; 
        }

        private static int[][] GeneratePermutations(int r, int n)
        {
            throw new System.NotImplementedException(); 
        }
    }

    public class GaussianDistribution : Distribution<double>
    {
        private double _mu;
        private double _sigma;
        public GaussianDistribution(double mu, double sigma)
        {
            _mu = mu;
            _sigma = sigma;
        }
        public double Distribute(Probability p)
        {
            return QuantileFunction(_mu, _sigma, p);
        }

        private static double QuantileFunction(double mu, double sigma, Probability p)
        {
            /*
             * see:
             * https://en.wikipedia.org/wiki/Normal_distribution#Quantile_function*
             */
            return mu + sigma * MathUtil.ShoreStandardNormalQuantileFunction(p);
        }
    }

}