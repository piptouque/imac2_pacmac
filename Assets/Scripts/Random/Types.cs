
using System;

namespace pacmac.random
{
    public class Probability
    {
        private double _p;
        public Probability(double p)
        {
            if(p < 0 || p > 1)
            {
                throw new System.ArgumentOutOfRangeException("Probability must be between 0 and 1.");
            }
            _p = p;
        }

        public double GetValue() { return _p; }

        public Probability GetInverseEventProb() => new Probability(1 - _p);

        public static implicit operator Probability(double p) => new Probability(p);
        public static implicit operator double(Probability prob) => prob.GetValue();

        public static Probability operator +(Probability prob1, Probability prob2)
        {
            return new Probability(prob1.GetValue() + prob2.GetValue());
        }
        public static Probability operator -(Probability prob1, Probability prob2)
        {
            return new Probability(prob1.GetValue() - prob2.GetValue());
        }

        public static bool operator <=(Probability prob1, Probability prob2)
        {
            return prob1.GetValue() <= prob2.GetValue();
        }
        public static bool operator >=(Probability prob1, Probability prob2)
        {
            return prob1.GetValue() >= prob2.GetValue();
        }
    }

}

