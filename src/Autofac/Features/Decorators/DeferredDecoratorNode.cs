using System;
using System.Linq;
using Autofac.Core;

namespace Autofac.Features.Decorators
{
    internal class DeferredDecoratorNode<TService> : DecoratorNode<TService>
    {
        protected virtual Parameter CreateDeferredParameter(Func<TService> func)
        {
            return new TypedParameter(typeof(Func<TService>), func);
        }

        public override DecoratorContext<TService> Decorate(IComponentContext context, Parameter[] parameters)
        {
            Action<DecoratorContext<TService>> updateDeferredContextAction = d => { };
            TService Result()
            {
                var decorated = ChildNode.Decorate(context, parameters);
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

        public DeferredDecoratorNode(
            IDecoratorNode<TService> childNode,
            IComponentRegistration decoratorRegistration,
            DecoratorService decoratorService)
            : base(childNode, decoratorRegistration, decoratorService)
        {
        }
    }
}