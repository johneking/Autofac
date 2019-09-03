using System;
using Autofac.Core;

namespace Autofac.Features.Decorators
{
    internal class LazyDecoratorPipelineSection<TService> : DeferredDecoratorPipelineSection<TService>
    {
        public LazyDecoratorPipelineSection(IDecoratorPipelineSection<TService> childPipelineSection, IComponentRegistration decoratorRegistration, DecoratorService decoratorService)
            : base(childPipelineSection, decoratorRegistration, decoratorService)
        {
        }

        protected override Parameter CreateDeferredParameter(Func<TService> func)
        {
            return new TypedParameter(typeof(Lazy<TService>), new Lazy<TService>(func));
        }
    }
}