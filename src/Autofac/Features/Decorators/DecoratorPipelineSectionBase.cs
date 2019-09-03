using Autofac.Core;
using Autofac.Core.Resolving;

namespace Autofac.Features.Decorators
{
    internal abstract class DecoratorPipelineSectionBase<TService> : IDecoratorPipelineSection<TService>
    {
        protected IDecoratorPipelineSection<TService> ChildPipelineSection { get; }

        protected DecoratorPipelineSectionBase(IDecoratorPipelineSection<TService> childPipelineSection)
        {
            ChildPipelineSection = childPipelineSection;
        }

        public abstract DecoratorContext<TService> Decorate(
            IComponentContext context,
            Parameter[] parameters,
            IComponentRegistration registration,
            InstanceLookup instanceLookup);
    }
}