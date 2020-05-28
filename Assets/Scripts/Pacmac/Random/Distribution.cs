
using System;
using System.Linq;

namespace pacmac.random
{
    public interface Distribution<T>
    {
        T Distribute(Probability prob);

        double TheoricStandardDiviation();
        double TheoricMean();
    }

    public abstract class FiniteDistribution<T> : Distribution<T>
    {
        private T[] _values;
        protected Probability[] _quantileValues;
        public FiniteDistribution(T[] values)
        {
            _values = (T[]) values.Clone();
            _quantileValues = new Probability[GetNumber()];
        }
        public T Distribute(Probability prob)
        {
            /*
             * see inverse distribution function
             * or quantive function
             */
            for (int i=0; i<GetNumber(); ++i)
            {
                if (prob <= GetQuantileValue(i))
                {
                    /* right one */
                    return GetValue(i);
                }
            }
            throw new System.ArgumentException("Nope, something's wrong with the quantile function " + "dim: " + GetNumber());
        }

        abstract public double TheoricStandardDiviation();
        abstract public double TheoricMean();


        protected T GetValue(int index) { return _values[index]; }
        protected Probability GetQuantileValue(int index) { return _quantileValues[index]; }
        protected int GetNumber() { return _values.GetLength(0); }

    }


    public class CustomFiniteDistribution<T> : FiniteDistribution<T>
    {
        public CustomFiniteDistribution(T[] values, Probability[] ps)
        : base(values)
        {
            PopulateQuantileValues(ps);
        }

        private void PopulateQuantileValues(Probability[] ps)
        {
            for (int i=0; i<GetNumber(); ++i)
            {
                _quantileValues[i] = ps[i];
                if(i > 0)
                {
                    _quantileValues[i] += _quantileValues[i - 1];
                }
            }
        }

        override public double TheoricStandardDiviation()
        {
            throw new System.NotImplementedException();
        }

        override public double TheoricMean()
        {
            throw new System.NotImplementedException();
        }
    }

    public abstract class FiniteRangeIntDistribution : FiniteDistribution<int>
    {
        protected FiniteRangeIntDistribution(int min, int max)
        : base(Enumerable.Range(Math.Min(min, max), Math.Abs(max - min) + 1).ToArray())
        {

        }

        protected int GetMin() { return GetValue(0); }
        protected int GetMax() { return GetValue(GetNumber() - 1); }

    }

    public class UniformRangeIntDistribution : FiniteRangeIntDistribution
    {
        public UniformRangeIntDistribution(int min, int max)
        : base(min, max)
        {
            PopulateQuantileValues();
        }
        private void PopulateQuantileValues()
        {
            for(int i=0; i<GetNumber(); ++i)
            {
                _quantileValues[i] = (double) (i+1) / GetNumber();
            }
        }
        override public double TheoricMean()
        {
            int min = GetMin();
            int max = GetMax();
            return (max + min) / 2.0;
        }

        override public double TheoricStandardDiviation()
        {
            int min = GetMin();
            int max = GetMax();
            int n = max - min + 1;
            return (n * n - 1) / 12.0;

        }

    }


    public class BernoulliDistribution : FiniteDistribution<bool>
    {
        private Probability _p;
        public BernoulliDistribution(Probability p)
        : base(new bool[] {false, true})
        {
            _p = new Probability(p);
            PopulateQuantileValues();
        }
        private void PopulateQuantileValues()
        {
            _quantileValues[0] = _p.GetInverseEventProb();
            _quantileValues[1] = 1.0;
        } 

        override public double TheoricMean()
        {
            return _p.GetValue();
        }

        override public double TheoricStandardDiviation()
        {
            return _p.GetValue() * (1 - _p.GetValue());
        }

    }

    public class BinomialDistribution : FiniteRangeIntDistribution
    {
        private Probability _p;
        public BinomialDistribution(Probability p, int min, int max)
        : base(min, max)
        {
            _p = new Probability(p);
            PopulateQuantileValues();
        }

        private void PopulateQuantileValues()
        {
            for (int k=0; k<GetNumber()-1; ++k)
            {
                _quantileValues[k] = (double)((Double)MathsUtil.BinomialCoefficient(k, GetNumber())
                * Math.Pow(_p.GetValue(), k)
                * Math.Pow(_p.GetInverseEventProb().GetValue(), GetNumber() - k));
                if (k > 0)
                {
                    _quantileValues[k] += _quantileValues[k - 1];
                }
            }
            _quantileValues[GetNumber()-1] = 1.0;
        }

        override public double TheoricMean()
        {
            int min = GetMin();
            int max = GetMax();
            int n = max - min + 1;
            return min + n * _p.GetValue();
        }

        override public double TheoricStandardDiviation()
        {
            int min = GetMin();
            int max = GetMax();
            int n = max - min + 1;
            return n * _p.GetValue() * (1 - _p.GetValue());
        }

    }


    public class HypergeometricDistribution : FiniteRangeIntDistribution
    {
        private int _N;
        private Probability _p;
        public HypergeometricDistribution(Probability p, int min, int max, int N)
        : base(min, max)
        {
            _N = N;
            if(N < min - max + 1)
            {
                throw new System.ArgumentOutOfRangeException("Nope, RTFM.");
            }
            /*
            * we have to have N*p an integer
            * so we may have to tweak p a bit
            */
            int j = (int) (_N * p.GetValue());
            _p = new Probability((double) j / _N);
            PopulateQuantileValues();
        }

        private void PopulateQuantileValues()
        {
            for (int k=0; k<GetNumber()-1; ++k)
            {
                _quantileValues[k] = (double)(MathsUtil.BinomialCoefficient(
                    (int)(_N * _p.GetValue()),
                    k
                    )
                    * MathsUtil.BinomialCoefficient(
                    (int)(_N * _p.GetInverseEventProb().GetValue()),
                    GetNumber() - k
                    )
                    / MathsUtil.BinomialCoefficient(_N, GetNumber())
                );
                if (k > 0)
                {
                    _quantileValues[k] += _quantileValues[k - 1];
                }
            }
            _quantileValues[GetNumber()-1] = 1.0;
        }
        override public double TheoricMean()
        {
            int min = GetMin();
            int max = GetMax();
            int n = max - min + 1;
            return min + n * _p.GetValue();
        }

        override public double TheoricStandardDiviation()
        {
            int min = GetMin();
            int max = GetMax();
            int n = max - min + 1;
            return n * _p.GetValue() * (1 - _p.GetValue()) * (_N - n) / (n - 1);
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
            _ps = (Probability[]) ps.Clone(); 
        }

        private static int[][] GeneratePermutations(int r, int n)
        {
            throw new System.NotImplementedException(); 
        }

        override public double TheoricMean()
        {
            throw new System.NotImplementedException(); 
        }

        override public double TheoricStandardDiviation()
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
            return mu + sigma * MathsUtil.ShoreStandardNormalQuantileFunction<double>(p);
        }

        public double TheoricMean()
        {
            return _mu;
        }

        public double TheoricStandardDiviation()
        {
            return Math.Sqrt(_sigma);
        }
    }

}