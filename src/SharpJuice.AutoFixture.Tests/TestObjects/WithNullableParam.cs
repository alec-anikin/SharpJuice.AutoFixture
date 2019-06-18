﻿namespace SharpJuice.AutoFixture.Tests.TestObjects
{
    public readonly struct WithNullableParam
    {
        public decimal? X { get; }
        public Money? Y { get; }
        public int? Z { get; }

        public WithNullableParam(decimal? x, Money? y, int? z)
        {
            X = x;
            Y = y;
            Z = z;
        }
    }
}