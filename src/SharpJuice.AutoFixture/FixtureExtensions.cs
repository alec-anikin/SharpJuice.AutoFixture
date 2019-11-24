using AutoFixture.Dsl;
using AutoFixture.Kernel;
using SharpJuice.AutoFixture;

namespace AutoFixture
{
    public static class FixtureExtensions
    {
        public static void UseGreedyConstructor<T>(this IFixture fixture)
        {
            fixture.Customize<T>(c => c.FromFactory(new MethodInvoker(new GreedyConstructorQuery())));
        }

        public static void UseGreedyConstructors(this IFixture fixture)
        {
            fixture.Customizations.Add(new MethodInvoker(new GreedyConstructorQuery()));
        }

        public static IPostprocessComposer<T> FromFactory<T>(
            this ICustomizationComposer<T> composer,
            IMethodQuery query,
            object methodParameters)
        {
            return composer.FromFactory(new MethodInvoker(new PartiallySpecifiedMethodQuery(query, new Parameters(methodParameters))));
        }

        public static IPostprocessComposer<T> FromFactory<T>(
            this ICustomizationComposer<T> composer,
            object methodParameters)
        {
            return composer.FromFactory(
                new MethodInvoker(new PartiallySpecifiedMethodQuery(new ModestConstructorQuery(), new Parameters(methodParameters))));
        }

        public static void CustomizeModestConstructor<T>(this IFixture fixture, object parameters)
        {
            fixture.Customize<T>(c => c.FromFactory(parameters));
        }

        public static void CustomizeGreedyConstructor<T>(this IFixture fixture, object parameters)
        {
            fixture.Customize<T>(c => c.FromFactory(new GreedyConstructorQuery(), parameters));
        }

        public static void CustomizeConstructor<T>(this IFixture fixture, object parameters)
        {
            var param = new Parameters(parameters);
            fixture.Customize<T>(c =>
                c.FromFactory(
                    new MethodInvoker(
                        new PartiallySpecifiedMethodQuery(new BestFitConstructorQuery(param), param))));
        }

        public static T Create<T>(this ISpecimenBuilder builder, object parameters)
        {
            var param = new Parameters(parameters);

            var invoker = new MethodInvoker(
                new PartiallySpecifiedMethodQuery(new BestFitConstructorQuery(param), param));

            return (T) invoker.Create(typeof(T), new SpecimenContext(builder));
        }

        public static TParameter FreezeParameter<TType, TParameter>(this IFixture fixture)
        {
            var context = new SpecimenContext(fixture);
            var specimen = context.Resolve(typeof(TParameter));

            fixture.Customize(
                new FreezeSpecimenOnMatchCustomization(
                    specimen,
                    new DeclaringTypeParameterSpecification(typeof(TType), typeof(TParameter))));

            return (TParameter) specimen;
        }

        public static void Register<TAbstract, TConcrete>(this Fixture fixture) where TConcrete : TAbstract
        {
            fixture.Register<TAbstract>(() => fixture.Create<TConcrete>());
        }
    }
}