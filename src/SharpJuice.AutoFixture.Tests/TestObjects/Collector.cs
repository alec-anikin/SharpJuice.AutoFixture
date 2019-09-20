using System.Collections.Generic;

namespace SharpJuice.AutoFixture.Tests.TestObjects
{
    public class Collector
    {
        public int Id { get; }
        public string Name { get; }
        public IReadOnlyCollection<int> Items { get; }

        public Collector(int id)
        {
            Id = id;
        }

        public Collector(int id, string name, IReadOnlyCollection<int> items)
        {
            Id = id;
            Name = name;
            Items = items;
        }
    }
}