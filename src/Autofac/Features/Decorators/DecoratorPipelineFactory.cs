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
                pipelineSection = decoratorSpecification.Service.AdaptionType.Value().AddDecoratorPipelineSection(
                    pipelineSection,
                    decoratorSpecification);
            }

            return pipelineSection;
        }
    }
}