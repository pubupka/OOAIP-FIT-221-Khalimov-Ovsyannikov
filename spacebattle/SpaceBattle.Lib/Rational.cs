using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SpaceBattle
{
    public class Rational
    {
        private int numerator;
        private int denominator = 360;

        public Rational(int num)
        {
            this.numerator = num % 360;
        }

        public override bool Equals(object? obj)
        {
            return obj is Rational r && this.numerator == r.numerator;
        }                                           
        
        public override int GetHashCode() => HashCode.Combine(numerator, denominator);

        public static Rational operator+(Rational angle, Rational delta)
        {
            angle.numerator = (angle.numerator + delta.numerator) % angle.denominator;
            return angle;
        }

        public static bool operator==(Rational angle, Rational delta)
        {
            return angle.numerator==delta.numerator;
        }

        public static bool operator!=(Rational angle, Rational delta)
        {
            return angle.numerator!=delta.numerator;
        }
    }
}