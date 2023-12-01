using static System.Linq.Enumerable;

namespace SpaceBattle
{
    public class Rational
    {
        private int _numerator;
        private int _denominator;

        public Rational(int num, int denominator = 360)
        {
            _numerator = num % 360;

            DevideByZero(denominator);
            _denominator = denominator;

            Sign();
            Minimize();
        }

        private void DevideByZero(int input_denum)
        {
            try
            {
                var test = _numerator / input_denum;
            }
            catch (DivideByZeroException)
            {
                throw new Exception();
            }
        }

        private void Sign()
        {
            if (_numerator * _denominator < 0)
            {
                _numerator = Math.Abs(_numerator) * (-1);
                _denominator = Math.Abs(_denominator);
            }
            else
            {
                _numerator = Math.Abs(_numerator);
                _denominator = Math.Abs(_denominator);
            }
        }

        public void Minimize()
        {

            var od = Range(1, Math.Min(Math.Abs(_numerator), Math.Abs(_denominator))).Where(x => _denominator % x == 0 &&
                                                                            _numerator % x == 0).Select(x => x);
            var nod = od.Last();

            _numerator /= nod;
            _denominator /= nod;
        }

        public override bool Equals(object? obj)
        {
            return obj is Rational r && _numerator == r._numerator && _denominator == r._denominator;
        }

        public override int GetHashCode() => HashCode.Combine(_numerator, _denominator);

        public static Rational operator +(Rational angle, Rational delta)
        {
            angle._numerator = (angle._numerator * delta._denominator + delta._numerator * angle._denominator);
            angle._denominator *= delta._denominator;
            angle.Minimize();
            angle._numerator = angle._numerator % angle._denominator;
            return angle;
        }

        public static bool operator ==(Rational angle, Rational delta)
        {
            return angle._numerator == delta._numerator && angle._denominator == delta._denominator;
        }

        public static bool operator !=(Rational angle, Rational delta)
        {
            return angle._numerator != delta._numerator || angle._denominator != delta._denominator;
        }
    }
}
