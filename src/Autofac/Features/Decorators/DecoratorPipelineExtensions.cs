using Autofac.Core;

namespace Autofac.Features.Decorators
{
    internal static class DecoratorPipelineExtensions
    {
        public static IDecoratorPipelineSection<TService> Create<TService>()
        {
            return new ComponentDecoratorPipelineSection<TService>();
        }

        public static IDecoratorPipelineSection<TService> AddDecorator<TService>(this IDecoratorPipelineSection<TService> pipeline, IComponentRegistration decoratorRegistration, DecoratorService decoratorService)
        {
            return new DecoratorPipelineSection<TService>(pipeline, decoratorRegistration, decoratorService);
        }

        public static IDecoratorPipelineSection<TService> AddFuncDecorator<TService>(this IDecoratorPipelineSection<TService> pipeline, IComponentRegistration decoratorRegistration, DecoratorService decoratorService)
        {
            return new DeferredDecoratorPipelineSection<TService>(pipeline, decoratorRegistration, decoratorService);
        }

        public static IDecoratorPipelineSection<TService> AddLazyDecorator<TService>(this IDecoratorPipelineSection<TService> pipeline, IComponentRegistration decoratorRegistration, DecoratorService decoratorService)
        {
            return new LazyDecoratorPipelineSection<TService>(pipeline, decoratorRegistration, decoratorService);
        }
    }
}