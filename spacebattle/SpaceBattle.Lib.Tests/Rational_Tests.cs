namespace SpaceBattle.Lib.Tests
{
    public class Rational_Tests
    {
        [Fact]
        public void Rational_Equals()
        {
            var angle = new Rational(45);
            var delta = new Rational(45);
            Assert.True(angle == delta);
        }

        [Fact]
        public void RationalUnequals()
        {
            var angle = new Rational(45, -360);
            var delta = new Rational(90);
            Assert.True(angle != delta);
        }

        [Fact]
        public void RationalUnequalsNumerator()
        {
            var angle = new Rational(13, 360);
            var delta = new Rational(17);
            Assert.True(angle != delta);
        }

        [Fact]
        public void RationalUnequalsNumeratorAndDenomirator()
        {
            var angle = new Rational(1, 2);
            var delta = new Rational(2, 3);
            Assert.True(angle != delta);
        }

        [Fact]
        public void RationalUnequalsDenomirator()
        {
            var angle = new Rational(1, 2);
            var delta = new Rational(1, 6);
            Assert.True(angle != delta);
        }

        [Fact]
        public void RationalEqualsNumeratorAndDenomiratorWrong()
        {
            var angle = new Rational(1, 2);
            var delta = new Rational(2, 3);
            Assert.False(angle == delta);
        }

        [Fact]
        public void RationalDenominatorIsZero()
        {
            Assert.Throws<Exception>(() => new Rational(17, 0));
        }

        [Fact]
        public void Rational_HashCode()
        {
            var angle = new Rational(45);
            var delta = new Rational(45);
            Assert.Equal(angle.GetHashCode(), delta.GetHashCode());
        }

        [Fact]
        public void Rational_Equals_Null()
        {
            var angle = new Rational(45);
            Assert.False(angle.Equals(null));
        }

        [Fact]
        public void RationalEqualsFalseNumerator()
        {
            var angle = new Rational(45);
            var delta = new Rational(45, 360);
            Assert.True(angle.Equals(delta));
        }
    }
}
