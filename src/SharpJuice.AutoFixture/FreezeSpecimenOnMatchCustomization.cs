using System;
using AutoFixture;
using AutoFixture.Kernel;

namespace SharpJuice.AutoFixture
{
    public sealed class FreezeSpecimenOnMatchCustomization : ICustomization
    {
        private readonly object _specimen;
        private readonly IRequestSpecification _matcher;

        public FreezeSpecimenOnMatchCustomization(
            object specimen,
            IRequestSpecification matcher)
        {
            _specimen = specimen;
            _matcher = matcher ?? throw new ArgumentNullException(nameof(matcher));
        }

        public void Customize(IFixture fixture)
        {
            if (fixture == null) throw new ArgumentNullException(nameof(fixture));

            FreezeSpecimenForMatchingRequests(fixture);
        }

        private void FreezeSpecimenForMatchingRequests(IFixture fixture)
        {
            fixture.Customizations.Insert(
                0,
                new FilteringSpecimenBuilder(
                    new FixedBuilder(_specimen),
                    _matcher));
        }
    }
}