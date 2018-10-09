using AutoFixture;
using AutoFixture.Dsl;
using AutoFixture.Kernel;

namespace SharpJuice.AutoFixture
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
            return composer.FromFactory(new MethodInvoker(new PartiallySpecifiedMethodQuery(query, methodParameters)));
        }

        public static IPostprocessComposer<T> FromFactory<T>(
            this ICustomizationComposer<T> composer,
            object methodParameters)
        {
            return composer.FromFactory(
                new MethodInvoker(new PartiallySpecifiedMethodQuery(new ModestConstructorQuery(), methodParameters)));
        }

        public static void CustomizeConstructor<T>(this IFixture fixture, object parameters)
        {
            fixture.Customize<T>(c => c.FromFactory(parameters));
        }

        public static void CustomizeGreedyConstructor<T>(this IFixture fixture, object parameters)
        {
            fixture.Customize<T>(c => c.FromFactory(new GreedyConstructorQuery(), parameters));
        }
    }
}