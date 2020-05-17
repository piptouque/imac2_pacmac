
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

        public static T[][] ListPermutations<T>(T[] values)
        {
            T[][] perms = new T[values.GetLength(0) * values.GetLength(0)][];
            for(int i=0; i<values.GetLength(0); ++i)
            {
                for(int j=i+1; j<values.GetLength(0); ++j)
                {
                perms[i*j] = GetPermutation(values, i, j); 
                }
            }
            return perms;
        }

        private static T[] GetPermutation<T>(T[] values, int indexBase, int indexSwap)
        {
            T[] perm = (T[])values.Clone();
            perm[indexBase] = values[indexSwap];
            perm[indexSwap] = values[indexBase];
            return perm;
        }
    }
}