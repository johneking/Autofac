using Autofac.Core;
using Autofac.Core.Resolving;

namespace Autofac.Features.Decorators
{
    internal class ComponentDecoratorNode<TService> : IDecoratorNode<TService>
    {
        private readonly IComponentRegistration _registration;
        private readonly InstanceLookup _instanceLookup;

        public ComponentDecoratorNode(IComponentRegistration registration, InstanceLookup instanceLookup)
        {
            _registration = registration;
            _instanceLookup = instanceLookup;
        }

        public DecoratorContext<TService> Decorate(IComponentContext context, Parameter[] parameters)
        {
            var rootInstance = (TService)_registration.Activator.ActivateInstance(_instanceLookup, parameters);
            return DecoratorContext<TService>.Create(rootInstance);
        }
    }
}