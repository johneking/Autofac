using Autofac.Core;

namespace Autofac.Features.Decorators
{
    internal abstract class DecoratorNodeBase<TService> : IDecoratorNode<TService>
    {
        protected IDecoratorNode<TService> ChildNode { get; }

        protected DecoratorNodeBase(IDecoratorNode<TService> childNode)
        {
            ChildNode = childNode;
        }

        public abstract DecoratorContext<TService> Decorate(IComponentContext context, Parameter[] parameters);
    }
}