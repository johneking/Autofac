using Autofac.Core;

namespace Autofac.Features.Decorators
{
    internal class DecoratorSpecification
    {
        public DecoratorSpecification(IComponentRegistration registration, DecoratorService service)
        {
            Registration = registration;
            Service = service;
        }

        public IComponentRegistration Registration { get; }

        public DecoratorService Service { get; }
    }
}