using System;
using System.Linq;
using Autofac.Core;
using Autofac.Core.Resolving;

namespace Autofac.Features.Decorators
{
    internal class DeferredDecoratorPipelineSection<TService> : DecoratorPipelineSection<TService>
    {
        protected virtual Parameter CreateDeferredParameter(Func<TService> func)
        {
            return new TypedParameter(typeof(Func<TService>), func);
        }

        public override DecoratorContext<TService> Decorate(
            IComponentContext context,
            Parameter[] parameters,
            IComponentRegistration registration,
            InstanceLookup instanceLookup)
        {
            Action<DecoratorContext<TService>> updateDeferredContextAction = d => { };
            TService Result()
            {
                var decorated = ChildPipelineSection.Decorate(context, parameters, registration, instanceLookup);
                updateDeferredContextAction(decorated);
                return decorated.Decorated;
            }

            var nextDecorator = ResolveNextDecoratorDeferred(DecoratorRegistration, Result, context, parameters);
            var newContext = DecoratorContext<TService>.CreateNewForDeferred(nextDecorator);
            updateDeferredContextAction = newContext.UpdateDeferredContext;

            return newContext;
        }

        private TService ResolveNextDecoratorDeferred(
            IComponentRegistration decoratorRegistration,
            Func<TService> childInstanceFunc,
            IComponentContext context,
            Parameter[] parameters)
        {
            var serviceParameter = CreateDeferredParameter(childInstanceFunc);
            var invokeParameters = parameters.Concat(new[] { serviceParameter });
            return (TService)context.ResolveComponent(new ResolveRequest(DecoratorService, decoratorRegistration, invokeParameters));
        }

        public DeferredDecoratorPipelineSection(
            IDecoratorPipelineSection<TService> childPipelineSection,
            IComponentRegistration decoratorRegistration,
            DecoratorService decoratorService)
            : base(childPipelineSection, decoratorRegistration, decoratorService)
        {
        }
    }
}