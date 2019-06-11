using System;

namespace SharpJuice.AutoFixture.Tests.TestObjects
{
    public sealed class MultiCtor
    {
        public int X { get; }
        public decimal Y { get; }
        public byte Age { get; }
        public string Text { get; }
        public long LongText { get; }

        public MultiCtor(int x, decimal y, string text)
        {
            X = x;
            Y = y;
            Text = text;
        }

        public MultiCtor(int x, decimal y, long text)
        {
            X = x;
            Y = y;
            LongText = text;
        }

        public MultiCtor(decimal y, string text, byte age)
        {
            Y = y;
            Age = age;
            Text = text.ToString();
        }

        public MultiCtor(decimal y, string text)
        {
            Y = y;
            Text = text;
        }

        public MultiCtor(DateTime x, TimeSpan y, Guid text, byte age)
        {
        }

        public MultiCtor()
        {
            throw new InvalidOperationException();
        }

    }
}