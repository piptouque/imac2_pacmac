
using System;

namespace pacmac.random
{
     public class RandomGenerator
     {
        private Random _rng;
        public RandomGenerator()
        {
            _rng = new Random();
        } 

        public T Random<T>(Distribution<T> dist)
        {
            return dist.Distribute(RandomProb());
        }

        public double Gaussian(double mu, double sigma)
        {
            return new GaussianDistribution(mu, sigma).Distribute(RandomProb());
        }

        public int Bernoulli(Probability p)
        {
            return new BernoulliDistribution(p).Distribute(RandomProb());
        }

        public int Binomial(Probability p, int n)
        {
            return new BinomialDistribution(p, n).Distribute(RandomProb());
        }

        public int Hypergeometric(Probability p, int n, int N)
        {
            return new HypergeometricDistribution(p, n, N).Distribute(RandomProb());
        }

        public int UniformRangeInt(int min, int max)
        {
            return new UniformRangeIntDistribution(min, max).Distribute(RandomProb());
        }

        private Probability RandomProb()
        {
            return new Probability(_rng.NextDouble());
        }
     }  
}