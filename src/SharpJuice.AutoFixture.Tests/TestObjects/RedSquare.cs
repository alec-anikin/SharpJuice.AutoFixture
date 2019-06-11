namespace SharpJuice.AutoFixture.Tests.TestObjects
{
    public sealed class RedSquare
    {
        public int X { get; }
        public long Y { get; }
        public Money Price { get; }
        public BlackMirror SomeClass { get; }

        public RedSquare(int x, long y, Money price, BlackMirror someClass)
        {
            X = x;
            Y = y;
            Price = price;
            SomeClass = someClass;
        }
    }
}