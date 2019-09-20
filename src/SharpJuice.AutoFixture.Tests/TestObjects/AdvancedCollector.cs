using System.Collections.Generic;

namespace SharpJuice.AutoFixture.Tests.TestObjects
{
    public class AdvancedCollector
    {
        public int Id { get; }
        public IReadOnlyCollection<IItem> Items { get; }

        public AdvancedCollector(int id)
        {
            Id = id;
        }

        public AdvancedCollector(int id, IReadOnlyCollection<IItem> items)
        {
            Id = id;
            Items = items;
        }
    }
}