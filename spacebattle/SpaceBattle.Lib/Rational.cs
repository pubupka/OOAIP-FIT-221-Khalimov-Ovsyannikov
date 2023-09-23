using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace 
{
    public class Rational
    {
        private int numerator;
        private int denominator = 360;

        public Rational(int num)
        {
            this.numerator = num;
        }

        public static Rational operator+(Rational angle, Rational delta)
        {
            angle.numerator = (angle.numerator + delta.numerator) % angle.denominator;
            return angle;
        }
    }
}