using System;

namespace Autofac.Features.Decorators
{
    internal abstract class DecoratorAdaptionType
    {
        internal abstract IDecoratorPipelineSection<TService> AddDecoratorPipelineSection<TService>(
            IDecoratorPipelineSection<TService> pipeline,
            DecoratorSpecification decoratorSpecification);

        internal abstract EDecoratorAdaptionType Type { get; }

        internal class NoDecoratorAdaptionType : DecoratorAdaptionType
        {
            internal override IDecoratorPipelineSection<TService> AddDecoratorPipelineSection<TService>(IDecoratorPipelineSection<TService> pipeline, DecoratorSpecification decoratorSpecification)
            {
                return pipeline.AddDecorator(decoratorSpecification.Registration, decoratorSpecification.Service);
            }

            internal override EDecoratorAdaptionType Type => EDecoratorAdaptionType.None;
        }

        internal class FuncDecoratorAdaptionType : DecoratorAdaptionType
        {
            internal override IDecoratorPipelineSection<TService> AddDecoratorPipelineSection<TService>(IDecoratorPipelineSection<TService> pipeline, DecoratorSpecification decoratorSpecification)
            {
                return pipeline.AddFuncDecorator(decoratorSpecification.Registration, decoratorSpecification.Service);
            }

            internal override EDecoratorAdaptionType Type => EDecoratorAdaptionType.Func;
        }

        internal class LazyDecoratorAdaptionType : DecoratorAdaptionType
        {
            internal override IDecoratorPipelineSection<TService> AddDecoratorPipelineSection<TService>(IDecoratorPipelineSection<TService> pipeline, DecoratorSpecification decoratorSpecification)
            {
                return pipeline.AddLazyDecorator(decoratorSpecification.Registration, decoratorSpecification.Service);
            }

            internal override EDecoratorAdaptionType Type => EDecoratorAdaptionType.Lazy;
        }

        internal static DecoratorAdaptionType None { get; } = new NoDecoratorAdaptionType();

        internal static DecoratorAdaptionType Func { get; } = new FuncDecoratorAdaptionType();

        internal static DecoratorAdaptionType Lazy { get; } = new LazyDecoratorAdaptionType();
    }
}