using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace SpaceBattle.Lib.Tests
{
    public class Rational_Tests
    {
        [Fact]
        public void Rational_Equals()
        {
            Rational angle = new Rational(45);
            Rational delta = new Rational(45);
            Assert.True(angle == delta);
        }

        [Fact]
        public void RationalUnequals()
        {
            Rational angle = new Rational(45, -360);
            Rational delta = new Rational(90);
            Assert.True(angle != delta);
        }

        [Fact]
        public void RationalUnequalsNumerator()
        {
            Rational angle = new Rational(13,360);
            Rational delta = new Rational(17);
            Assert.True(angle != delta);
        }

        [Fact]
        public void RationalUnequalsNumeratorAndDenomirator()
        {
            Rational angle = new Rational(1,2);
            Rational delta = new Rational(2,3);
            Assert.True(angle != delta);
        }

        [Fact]
        public void RationalUnequalsDenomirator()
        {
            Rational angle = new Rational(1,2);
            Rational delta = new Rational(1,6);
            Assert.True(angle != delta);
        }

        [Fact]
        public void RationalEqualsNumeratorAndDenomiratorWrong()
        {
            Rational angle = new Rational(1,2);
            Rational delta = new Rational(2,3);
            Assert.False(angle == delta);
        }

        [Fact]
        public void RationalDenominatorIsZero()
        {
           Assert.Throws<Exception>(() => new Rational(17,0));
        }

        [Fact]
        public void Rational_HashCode()
        {
            Rational angle = new Rational(45);
            Rational delta = new Rational(45);
            Assert.Equal(angle.GetHashCode(), delta.GetHashCode());
        }

        [Fact]
        public void Rational_Equals_Null()
        {
            Rational angle = new Rational(45);
            Assert.False(angle.Equals(null));
        }

        [Fact]
        public void RationalEqualsFalseNumerator()
        {
            Rational angle = new Rational(45);
            Rational delta = new Rational(45,360);
            Assert.True(angle.Equals(delta));
        }
    }
}