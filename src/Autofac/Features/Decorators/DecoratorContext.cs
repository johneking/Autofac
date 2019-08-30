using System;
using System.Collections.Generic;
using System.Linq;

namespace Autofac.Features.Decorators
{
    public sealed class DecoratorContext<TService> : IDecoratorContext
    {
        public TService Decorated { get; private set; }

        public TService Undecorated { get; private set; }

        public Type ImplementationType => Undecorated.GetType();

        public Type ServiceType => typeof(TService);

        /// <inheritdoc />
        public IReadOnlyList<Type> AppliedDecoratorTypes { get; private set; }

        /// <inheritdoc />
        public IReadOnlyList<object> AppliedDecorators { get; private set; }

        public object CurrentInstance => Decorated;

        public IDecoratorContext DeferredContext { get; private set; }

        internal void UpdateDeferredContext(IDecoratorContext context)
        {
            DeferredContext = context;
        }

        internal static DecoratorContext<TService> Create(TService implementationInstance)
        {
            var context = new DecoratorContext<TService>
            {
                Decorated = implementationInstance,
                Undecorated = implementationInstance,
                AppliedDecorators = new List<object>(0),
                AppliedDecoratorTypes = new List<Type>(0),
            };

            return context;
        }

        internal static DecoratorContext<TService> CreateNewForDeferred(TService newDecoratorInstance)
        {
            var context = new DecoratorContext<TService>
            {
                Decorated = newDecoratorInstance,
                Undecorated = default(TService),
                AppliedDecorators = new List<object>(0),
                AppliedDecoratorTypes = new List<Type>(0),
            };

            return context;
        }

        internal DecoratorContext<TService> UpdateContext(TService decoratorInstance)
        {
            var appliedDecorators = AppliedDecorators.ToList();
            appliedDecorators.Add(decoratorInstance);

            var appliedDecoratorTypes = AppliedDecoratorTypes.ToList();
            appliedDecoratorTypes.Add(decoratorInstance.GetType());

            var context = new DecoratorContext<TService>
            {
                Decorated = decoratorInstance,
                Undecorated = Undecorated,
                AppliedDecorators = appliedDecorators,
                AppliedDecoratorTypes = appliedDecoratorTypes,
            };

            return context;
        }
    }
}