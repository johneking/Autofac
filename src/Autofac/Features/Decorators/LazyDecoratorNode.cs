using System;
using Autofac.Core;

namespace Autofac.Features.Decorators
{
    internal class LazyDecoratorNode<TService> : DeferredDecoratorNode<TService>
    {
        public LazyDecoratorNode(IDecoratorNode<TService> childNode, IComponentRegistration decoratorRegistration, DecoratorService decoratorService)
            : base(childNode, decoratorRegistration, decoratorService)
        {
        }

        protected override Parameter CreateDeferredParameter(Func<TService> func)
        {
            return new TypedParameter(typeof(Lazy<TService>), new Lazy<TService>(func));
        }
    }
}