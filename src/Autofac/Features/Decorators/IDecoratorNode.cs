using Autofac.Core;

namespace Autofac.Features.Decorators
{
    internal interface IDecoratorNode<TService>
    {
        DecoratorContext<TService> Decorate(IComponentContext context, Parameter[] parameters);
    }
}