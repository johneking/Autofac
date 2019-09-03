using Autofac.Core;
using Autofac.Core.Resolving;

namespace Autofac.Features.Decorators
{
    internal interface IDecoratorPipelineSection<TService>
    {
        DecoratorContext<TService> Decorate(IComponentContext context, Parameter[] parameters, IComponentRegistration registration, InstanceLookup instanceLookup);
    }
}