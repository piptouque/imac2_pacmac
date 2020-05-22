
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

        private Probability RandomProb()
        {
            return new Probability(_rng.NextDouble());
        }
     }  
}