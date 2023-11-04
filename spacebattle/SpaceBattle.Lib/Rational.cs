using System;
using System.Collections.Generic;
using System.Linq;
using static System.Linq.Enumerable;
using System.Threading.Tasks;

namespace SpaceBattle
{
    public class Rational
    {
        private int numerator;
        private int denominator;

        public Rational(int num, int denominator=360)
        {
            this.numerator = num % 360;

            this.DevideByZero(denominator);
            this.denominator = denominator;

            this.Sign();
            this.Minimize();
        }

         private void DevideByZero(int input_denum)
        {
            try
            {
                var test = this.numerator / input_denum;
            }
            catch (DivideByZeroException)
            {
                throw new Exception();
            }
        }

         private void Sign()
        {
            if (this.numerator * this.denominator < 0)
            {
                this.numerator = Math.Abs(this.numerator) * (-1);
                this.denominator = Math.Abs(this.denominator);
            }
            else
            {
                this.numerator = Math.Abs(this.numerator);
                this.denominator = Math.Abs(this.denominator);
            }
        }

         public void Minimize()
        {

            var od = Range(1, Math.Min(Math.Abs(this.numerator), Math.Abs(this.denominator))).Where(x => this.denominator % x == 0 && 
                                                                            this.numerator % x == 0).Select(x => x);
            int nod = od.Last();

            this.numerator /= nod;
            this.denominator /= nod;
        }

        public override bool Equals(object? obj)
        {
            return obj is Rational r && this.numerator == r.numerator && this.denominator == r.denominator;
        }                                           
        
        public override int GetHashCode() => HashCode.Combine(numerator, denominator);

        public static Rational operator+(Rational angle, Rational delta)
        {
            angle.numerator = (angle.numerator + delta.numerator) % angle.denominator;
            angle.Minimize();
            return angle;
        }

        public static bool operator==(Rational angle, Rational delta)
        {
            return angle.numerator==delta.numerator && angle.denominator==delta.denominator;
        }

        public static bool operator!=(Rational angle, Rational delta)
        {
            return angle.numerator!=delta.numerator || angle.denominator!=delta.denominator;
        }
    }
}