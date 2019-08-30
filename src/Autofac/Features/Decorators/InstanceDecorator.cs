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
using System.Reflection;
using Autofac.Core;
using Autofac.Core.Registration;
using Autofac.Core.Resolving;

namespace Autofac.Features.Decorators
{
    internal static class InstanceDecorator
    {
        internal static DecorationResult TryDecorateRegistration(
            IComponentRegistration registration,
            IComponentContext context,
            IEnumerable<Parameter> parameters,
            InstanceLookup instanceLookup)
        {
            // Issue #965: Do not apply the decorator if the registration is for an adapter.
            if (registration.IsAdapting()) return DecorationResult.UndecoratedResult;

            var decoratorRegistrations = context.ComponentRegistry.DecoratorsFor(registration);

            // ReSharper disable once PossibleMultipleEnumeration
            if (!decoratorRegistrations.Any()) return DecorationResult.UndecoratedResult;

            // ReSharper disable once PossibleMultipleEnumeration
            var decorators = decoratorRegistrations
                .Select(r => new DecoratorSpecification(r, r.Services.OfType<DecoratorService>().First()))
                .ToArray();

            if (decorators.Length == 0) return DecorationResult.UndecoratedResult;

            var serviceType = decorators[0].Service.ServiceType;
            var resolveParameters = parameters as Parameter[] ?? parameters.ToArray();

            return DecorateByType(serviceType, decorators, registration, instanceLookup, context, resolveParameters);
        }

        private static readonly MethodInfo DecorateMethod = typeof(InstanceDecorator).GetTypeInfo().GetDeclaredMethod(nameof(Decorate));

        private static DecorationResult DecorateByType(
            Type serviceType,
            IEnumerable<DecoratorSpecification> specifications,
            IComponentRegistration componentRegistration,
            InstanceLookup instanceLookup,
            IComponentContext componentContext,
            Parameter[] parameters)
        {
            return (DecorationResult)DecorateMethod.MakeGenericMethod(serviceType)
                .Invoke(null, new object[] { specifications, componentRegistration, instanceLookup, componentContext, parameters });
        }

        private static DecorationResult Decorate<TService>(
            IEnumerable<DecoratorSpecification> specifications,
            IComponentRegistration componentRegistration,
            InstanceLookup instanceLookup,
            IComponentContext componentContext,
            Parameter[] parameters)
        {
            var decoratorNode = DecoratorHierarchyFactory.GetDecoratorHierarchy<TService>(specifications, componentRegistration, instanceLookup);
            var resultContext = decoratorNode.Decorate(componentContext, parameters);
            return resultContext.Undecorated == null
                ? DecorationResult.GetDeferredDecoratedResult(resultContext.Decorated)
                : DecorationResult.GetDecoratedResult(resultContext.Undecorated, resultContext.Decorated);
        }
    }
}
