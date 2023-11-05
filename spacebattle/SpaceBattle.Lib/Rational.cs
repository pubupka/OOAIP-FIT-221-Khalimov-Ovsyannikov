using System;
using System.Collections.Generic;
using System.Linq;
using static System.Linq.Enumerable;
using System.Threading.Tasks;

namespace SpaceBattle
{
    public class Rational
    {
        private int _numerator;
        private int _denominator;

        public Rational(int num, int denominator=360)
        {
            this._numerator = num % 360;

            this.DevideByZero(denominator);
            this._denominator = denominator;

            this.Sign();
            this.Minimize();
        }

         private void DevideByZero(int input_denum)
        {
            try
            {
                var test = this._numerator / input_denum;
            }
            catch (DivideByZeroException)
            {
                throw new Exception();
            }
        }

         private void Sign()
        {
            if (this._numerator * this._denominator < 0)
            {
                this._numerator = Math.Abs(this._numerator) * (-1);
                this._denominator = Math.Abs(this._denominator);
            }
            else
            {
                this._numerator = Math.Abs(this._numerator);
                this._denominator = Math.Abs(this._denominator);
            }
        }

         public void Minimize()
        {

            var od = Range(1, Math.Min(Math.Abs(this._numerator), Math.Abs(this._denominator))).Where(x => this._denominator % x == 0 && 
                                                                            this._numerator % x == 0).Select(x => x);
            int nod = od.Last();

            this._numerator /= nod;
            this._denominator /= nod;
        }

        public override bool Equals(object? obj)
        {
            return obj is Rational r && this._numerator == r._numerator && this._denominator == r._denominator;
        }                                           
        
        public override int GetHashCode() => HashCode.Combine(_numerator, _denominator);

        public static Rational operator+(Rational angle, Rational delta)
        {
            angle._numerator = (angle._numerator*delta._denominator + delta._numerator*angle._denominator);
            angle._denominator *= delta._denominator; 
            angle.Minimize();
            angle._numerator = angle._numerator % angle._denominator;
            return angle;
        }

        public static bool operator==(Rational angle, Rational delta)
        {
            return angle._numerator==delta._numerator && angle._denominator==delta._denominator;
        }

        public static bool operator!=(Rational angle, Rational delta)
        {
            return angle._numerator!=delta._numerator || angle._denominator!=delta._denominator;
        }
    }
}