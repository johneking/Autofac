// This software is part of the Autofac IoC container
// Copyright © 2018 Autofac Contributors
// https://autofac.org
//
// Permission is hereby granted, free of charge, to any person
// obtaining a copy of this software and associated documentation
// files (the "Software"), to deal in the Software without
// restriction, including without limitation the rights to use,
// copy, modify, merge, publish, distribute, sublicense, and/or sell
// copies of the Software, and to permit persons to whom the
// Software is furnished to do so, subject to the following
// conditions:
//
// The above copyright notice and this permission notice shall be
// included in all copies or substantial portions of the Software.
//
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND,
// EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES
// OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND
// NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT
// HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY,
// WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING
// FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR
// OTHER DEALINGS IN THE SOFTWARE.

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