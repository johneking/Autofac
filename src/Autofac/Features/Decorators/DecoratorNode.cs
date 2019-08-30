using System.Linq;
using Autofac.Core;

namespace Autofac.Features.Decorators
{
    internal class DecoratorNode<TService> : DecoratorNodeBase<TService>
    {
        protected IComponentRegistration DecoratorRegistration { get; }

        protected DecoratorService DecoratorService { get; }

        public DecoratorNode(IDecoratorNode<TService> childNode, IComponentRegistration decoratorRegistration, DecoratorService decoratorService)
            : base(childNode)
        {
            DecoratorRegistration = decoratorRegistration;
            DecoratorService = decoratorService;
        }

        public override DecoratorContext<TService> Decorate(IComponentContext context, Parameter[] parameters)
        {
            var result = ChildNode.Decorate(context, parameters);
            if (DecoratorService.Condition(result))
            {
                var nextDecorator = ResolveNextDecorator(DecoratorRegistration, result.Decorated, result, context, parameters);
                result = result.UpdateContext(nextDecorator);
            }

            return result;
        }

        private TService ResolveNextDecorator(
            IComponentRegistration decoratorRegistration,
            TService childInstance,
            DecoratorContext<TService> currentContext,
            IComponentContext context,
            Parameter[] parameters)
        {
            var serviceParameter = new TypedParameter(typeof(TService), childInstance);
            var contextParameter = new TypedParameter(typeof(IDecoratorContext), currentContext);
            var invokeParameters = parameters.Concat(new Parameter[] { serviceParameter, contextParameter });
            return (TService)context.ResolveComponent(new ResolveRequest(DecoratorService, decoratorRegistration, invokeParameters));
        }
    }
}