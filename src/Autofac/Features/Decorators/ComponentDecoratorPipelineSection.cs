using Autofac.Core;
using Autofac.Core.Resolving;

namespace Autofac.Features.Decorators
{
    internal class ComponentDecoratorPipelineSection<TService> : IDecoratorPipelineSection<TService>
    {
        public DecoratorContext<TService> Decorate(
            IComponentContext context,
            Parameter[] parameters,
            IComponentRegistration registration,
            InstanceLookup instanceLookup)
        {
            var rootInstance = (TService)registration.Activator.ActivateInstance(instanceLookup, parameters);
            return DecoratorContext<TService>.Create(rootInstance);
        }
    }
}