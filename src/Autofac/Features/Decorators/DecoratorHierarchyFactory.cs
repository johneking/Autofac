using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Autofac.Core;
using Autofac.Core.Resolving;

namespace Autofac.Features.Decorators
{
    internal static class DecoratorHierarchyFactory
    {
        public static IDecoratorNode<TService> GetDecoratorHierarchy<TService>(
            IEnumerable<DecoratorSpecification> specifications,
            IComponentRegistration componentRegistration,
            InstanceLookup instanceLookup)
        {
            var rootNode = new ComponentDecoratorNode<TService>(componentRegistration, instanceLookup);
            return GetDecoratorHierarchy(specifications, rootNode);
        }

        private static IDecoratorNode<TService> GetDecoratorHierarchy<TService>(IEnumerable<DecoratorSpecification> specifications, IDecoratorNode<TService> componentNode)
        {
            var currentNode = componentNode;
            foreach (var decoratorSpecification in specifications)
            {
                currentNode = GetDecoratorNode(
                    currentNode,
                    GetValidConstructorTypes(typeof(TService), decoratorSpecification),
                    decoratorSpecification);
            }

            return currentNode;
        }

        private static DecoratorNodeBase<TService> GetDecoratorNode<TService>(
            IDecoratorNode<TService> currentNode,
            ConstructorType[] constructorTypes,
            DecoratorSpecification decoratorSpecification)
        {
            switch (constructorTypes.First())
            {
                case ConstructorType.Lambda:
                case ConstructorType.Normal:
                    return new DecoratorNode<TService>(currentNode, decoratorSpecification.Registration, decoratorSpecification.Service);
                case ConstructorType.Func:
                    return new DeferredDecoratorNode<TService>(currentNode, decoratorSpecification.Registration, decoratorSpecification.Service);
                case ConstructorType.Lazy:
                    return new LazyDecoratorNode<TService>(currentNode, decoratorSpecification.Registration, decoratorSpecification.Service);
                default:
                    throw new ArgumentOutOfRangeException($"Invalid {nameof(constructorTypes)}");
            }
        }

        private enum ConstructorType
        {
            Normal,
            Lambda,
            Func,
            Lazy
        }

        private static ConstructorType[] GetValidConstructorTypes(Type serviceType, DecoratorSpecification decoratorSpecification)
        {
            if (decoratorSpecification.Registration.Activator.LimitType == serviceType)
                return new[] { ConstructorType.Lambda };
            var constructorTypes = AnalyzeConstructorsForType(serviceType, decoratorSpecification.Registration.Activator.LimitType);
            var validConstructors = constructorTypes.Where(c => c.Length == 1) // exclude constructors with > 1 type of dependency on the target service
                .Select(c => c.First())
                .OrderBy(c => c)
                .ToArray();
            if (validConstructors.Length < 1)
                throw new Exception($"None of the constructors for {decoratorSpecification.Service.ServiceType.Name} contain a single reference to the decorator target {serviceType.Name}");
            return validConstructors;
        }

        private static IEnumerable<ConstructorType[]> AnalyzeConstructorsForType(Type dependency, Type dependent)
        {
            return dependent.GetTypeInfo()
                .DeclaredConstructors.Where(c => c.IsPublic)
                .Select(c => GetConstructorTypePresences(c, dependency));
        }

        private static ConstructorType[] GetConstructorTypePresences(ConstructorInfo constructorInfo, Type dependencyType)
        {
            var constructorTypes = new List<ConstructorType>();
            if (ConstructorContainsType(constructorInfo, dependencyType))
                constructorTypes.Add(ConstructorType.Normal);
            if (ConstructorContainsFuncType(constructorInfo, dependencyType))
                constructorTypes.Add(ConstructorType.Func);
            if (ConstructorContainsLazyType(constructorInfo, dependencyType))
                constructorTypes.Add(ConstructorType.Lazy);
            return constructorTypes.ToArray();
        }

        private static bool ConstructorContainsType(ConstructorInfo constructorInfo, Type dependencyType)
        {
            return constructorInfo.GetParameters()
                .Any(p => p.ParameterType == dependencyType);
        }

        private static bool ConstructorContainsFuncType(ConstructorInfo constructorInfo, Type dependencyType)
        {
            var targetType = typeof(Func<>).MakeGenericType(dependencyType);
            return constructorInfo.GetParameters()
                .Any(p => p.ParameterType == targetType);
        }

        private static bool ConstructorContainsLazyType(ConstructorInfo constructorInfo, Type dependencyType)
        {
            var targetType = typeof(Lazy<>).MakeGenericType(dependencyType);
            return constructorInfo.GetParameters()
                .Any(p => p.ParameterType == targetType);
        }
    }
}