
using System;
using System.Numerics;

namespace pacmac.random
{
    public class MathUtil
    {
        public static BigInteger BinomialCoefficient(BigInteger k, BigInteger n)
        {
            if (k<0 || n<0)
            {
                throw new System.ArgumentOutOfRangeException("Invalid k or n in binomial coefficient.");
            }
            if (k > n) { return BinomialCoefficient(n,k); }
            if (k > n - k) { k = n - k; }
            BigInteger result = 1;
            for (BigInteger i=1; i <= k; ++i)
            {
                result *= n - k + i;
                result /= i;
            }
            return result;
        }

        public static FloatType ShoreStandardNormalQuantileFunction<FloatType>(Probability p)
            where FloatType : unmanaged
        {
            /*
             * see:
             *https://en.wikipedia.org/wiki/Normal_distribution#Numerical_approximations_for_the_normal_CDF 
             */
            double r, epsilon;
            bool condition = p.GetValue() >= 0.5;
            r = condition ? p : 1 - p;
            epsilon = condition ? 1.0 : 0.0;
            return (FloatType)(object)(epsilon * 5.5556*(1 - Math.Pow(r / (1 - r), 0.1186)));
        }

        public static T[][] Permutations<T>(T[] values, int order)
        {
            /* ??? */
            throw new System.NotImplementedException();
        }
    }
}