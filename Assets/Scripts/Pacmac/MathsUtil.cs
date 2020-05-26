
using System;

using UnityEngine;

namespace pacmac
{
    public class MathsUtil
    {
        public static System.Numerics.BigInteger BinomialCoefficient(System.Numerics.BigInteger k, System.Numerics.BigInteger n)
        {
            if (k<0 || n<0)
            {
                throw new System.ArgumentOutOfRangeException("Invalid k or n in binomial coefficient.");
            }
            if (k > n) { return BinomialCoefficient(n,k); }
            if (k > n - k) { k = n - k; }
            System.Numerics.BigInteger result = 1;
            for (System.Numerics.BigInteger i=1; i <= k; ++i)
            {
                result *= n - k + i;
                result /= i;
            }
            return result;
        }

        public static FloatType ShoreStandardNormalQuantileFunction<FloatType>(random.Probability p)
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