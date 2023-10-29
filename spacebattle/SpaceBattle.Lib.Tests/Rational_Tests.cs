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
        public void Rational_Unequals()
        {
            Rational angle = new Rational(45);
            Rational delta = new Rational(90);
            Assert.True(angle != delta);
        }

        [Fact]
        public void Rational_HashCode()
        {
            Rational angle = new Rational(45);
            Rational delta = new Rational(45);
            Assert.Equal(angle.GetHashCode(), delta.GetHashCode());
        }
    }
}