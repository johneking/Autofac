using System;
using System.Collections.Generic;

namespace Autofac.Features.Decorators
{
    internal static class DecoratorPipelineFactory
    {
        public static IDecoratorPipelineSection<TService> GetDecoratorPipeline<TService>(
            IEnumerable<DecoratorSpecification> specifications)
        {
            var pipeline = DecoratorPipelineExtensions.Create<TService>();
            return GetDecoratorPipeline(specifications, pipeline);
        }

        private static IDecoratorPipelineSection<TService> GetDecoratorPipeline<TService>(IEnumerable<DecoratorSpecification> specifications, IDecoratorPipelineSection<TService> componentPipelineSection)
        {
            var pipelineSection = componentPipelineSection;
            foreach (var decoratorSpecification in specifications)
            {
                pipelineSection = AddDecoratorPipelineSection(
                    pipelineSection,
                    decoratorSpecification);
            }

            return pipelineSection;
        }

        private static IDecoratorPipelineSection<TService> AddDecoratorPipelineSection<TService>(
            IDecoratorPipelineSection<TService> currentPipelineSection,
            DecoratorSpecification decoratorSpecification)
        {
            switch (decoratorSpecification.Service.AdaptionType)
            {
                case DecoratorAdaptionType.None:
                    return currentPipelineSection.AddDecorator(decoratorSpecification.Registration, decoratorSpecification.Service);
                case DecoratorAdaptionType.Func:
                    return currentPipelineSection.AddFuncDecorator(decoratorSpecification.Registration, decoratorSpecification.Service);
                case DecoratorAdaptionType.Lazy:
                    return currentPipelineSection.AddLazyDecorator(decoratorSpecification.Registration, decoratorSpecification.Service);
                default:
                    throw new ArgumentOutOfRangeException($"Invalid {nameof(DecoratorAdaptionType)}");
            }
        }
    }
}