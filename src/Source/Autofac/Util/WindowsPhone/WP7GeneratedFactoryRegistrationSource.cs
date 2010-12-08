﻿// This software is part of the Autofac IoC container
// Copyright (c) 2010 Autofac Contributors
// http://autofac.org
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
using Autofac.Builder;
using Autofac.Core;
using Autofac.Util;
using System.Diagnostics;

namespace Autofac.Features.GeneratedFactories
{
    class GeneratedFactoryRegistrationSource : IRegistrationSource
    {
        static readonly MethodInfo[] delegateFuncRegistrations = typeof(GeneratedFactoryRegistrationSource)
            .GetMethods(BindingFlags.NonPublic | BindingFlags.Static)
            .Where(x => x.Name == "DelegateFuncRegistration")
            .ToArray();

        public IEnumerable<IComponentRegistration> RegistrationsFor(
            Service service,
            Func<Service, IEnumerable<IComponentRegistration>> registrationAccessor)
        {
            Debug.WriteLine("Querying source.");

            var swt = service as IServiceWithType;
            if (swt != null &&
                swt.ServiceType.IsGenericType &&
                swt.ServiceType.Name.StartsWith("Func"))
            {
                var genericArguments = swt.ServiceType.GetGenericArguments();

                var args = genericArguments
                    .Append(swt.ServiceType)
                    .ToList();

                //Find a func method that matches all args
                var funcRegistration = delegateFuncRegistrations
                    .FirstOrDefault(x => x.GetGenericArguments().Count() == args.Count());

                if (funcRegistration != null)
                {
                    var creator = funcRegistration
                        .MakeGenericMethod(args.ToArray());

                    return registrationAccessor(new TypedService(genericArguments.Last()))
                        .Select(cr => (IComponentRegistration)creator.Invoke(null, new object[] { cr }));
                }

            }
            else if (swt != null && swt.ServiceType.IsDelegate())
            {
                var delegateMethod = swt.ServiceType.GetMethods().First();

                var args = delegateMethod
                    .GetParameters()
                    .Select(x => x.ParameterType)
                    .ToList();
                args.Add(delegateMethod.ReturnType);
                args.Add(swt.ServiceType);

                //Find a func method that matches all args
                var funcRegistration = delegateFuncRegistrations
                    .FirstOrDefault(x => x.GetGenericArguments().Count() == args.Count()); ;

                if (funcRegistration != null)
                {
                    var creator = funcRegistration.MakeGenericMethod(args.ToArray());

                    return registrationAccessor(new TypedService(delegateMethod.ReturnType))
                        .Select(cr => (IComponentRegistration)creator.Invoke(null, new object[] { cr }));
                }
            }

            return Enumerable.Empty<IComponentRegistration>();
        }

        public bool IsAdapterForIndividualComponents
        {
            get { return true; }
        }

        static IComponentRegistration DelegateFuncRegistration<TResult, TDelegate>(IComponentRegistration target)
        {
            return RegistrationBuilder
                .ForDelegate(typeof(TDelegate), (c, p) =>
                {
                    var ls = c.Resolve<ILifetimeScope>();
                    Func<TResult> del1 = () =>
                    {
                        Debug.WriteLine("Invoking Delegate");
                        var r = ls.ResolveComponent(target, null);
                        return (TResult)r;
                    };
                    var del = Delegate.CreateDelegate(typeof(TDelegate), del1.Target, del1.Method, true);
                    return del;
                })
                .InstancePerLifetimeScope()
                .ExternallyOwned()
                .Targeting(target)
                .CreateRegistration();
        }

        static IComponentRegistration DelegateFuncRegistration<TArg0, TResult, TDelegate>(IComponentRegistration target)
        {
            return RegistrationBuilder
                .ForDelegate(typeof(TDelegate), (c, p) =>
                {
                    var ls = c.Resolve<ILifetimeScope>();
                    Func<TArg0, TResult> del1 = a0 =>
                    {
                        Debug.WriteLine("Invoking Delegate");
                        IEnumerable<Parameter> parameterCollection;
                        if (typeof(TDelegate).IsGenericType)
                            parameterCollection = new[] { TypedParameter.From(a0) };
                        else
                        {
                            var parameters = typeof(TDelegate).GetMethods().First().GetParameters();
                            parameterCollection = new[]
                                {
                                    new NamedParameter(parameters[0].Name, a0)
                                };
                        }

                        var r = ls.ResolveComponent(target, parameterCollection);
                        return (TResult)r;
                    };
                    var del = Delegate.CreateDelegate(typeof(TDelegate), del1.Target, del1.Method, true);
                    return del;
                })
                .InstancePerLifetimeScope()
                .ExternallyOwned()
                .Targeting(target)
                .CreateRegistration();
        }

        static IComponentRegistration DelegateFuncRegistration<TArg0, TArg1, TResult, TDelegate>(IComponentRegistration target)
        {
            return RegistrationBuilder
                .ForDelegate(typeof(TDelegate), (c, p) =>
                {
                    var ls = c.Resolve<ILifetimeScope>();
                    Func<TArg0, TArg1, TResult> del1 = (a0, a1) =>
                    {
                        Debug.WriteLine("Invoking Delegate");
                        IEnumerable<Parameter> parameterCollection;
                        if (typeof(TDelegate).IsGenericType)
                            parameterCollection = new[] { TypedParameter.From(a0), TypedParameter.From(a1) };
                        else
                        {
                            var parameters = typeof(TDelegate).GetMethods().First().GetParameters();
                            parameterCollection = new[]
                                {
                                    new NamedParameter(parameters[0].Name, a0),
                                    new NamedParameter(parameters[1].Name, a1)
                                };
                        }

                        var r = ls.ResolveComponent(target, parameterCollection);
                        return (TResult)r;
                    };
                    var del = Delegate.CreateDelegate(typeof(TDelegate), del1.Target, del1.Method, true);
                    return del;
                })
                .InstancePerLifetimeScope()
                .ExternallyOwned()
                .Targeting(target)
                .CreateRegistration();
        }

        static IComponentRegistration DelegateFuncRegistration<TArg0, TArg1, TArg2, TResult, TDelegate>(IComponentRegistration target)
        {
            return RegistrationBuilder
                .ForDelegate(typeof(TDelegate), (c, p) =>
                {
                    var ls = c.Resolve<ILifetimeScope>();
                    Func<TArg0, TArg1, TArg2, TResult> del1 = (a0, a1, a2) =>
                    {
                        Debug.WriteLine("Invoking Delegate");
                        IEnumerable<Parameter> parameterCollection;
                        if (typeof(TDelegate).IsGenericType)
                            parameterCollection = new[] { TypedParameter.From(a0), TypedParameter.From(a1), TypedParameter.From(a2) };
                        else
                        {
                            var parameters = typeof(TDelegate).GetMethods().First().GetParameters();
                            parameterCollection = new[]
                                {
                                    new NamedParameter(parameters[0].Name, a0),
                                    new NamedParameter(parameters[1].Name, a1),
                                     new NamedParameter(parameters[2].Name, a2)
                                };
                        }

                        var r = ls.ResolveComponent(target, parameterCollection);
                        return (TResult)r;
                    };
                    var del = Delegate.CreateDelegate(typeof(TDelegate), del1.Target, del1.Method, true);
                    return del;
                })
                .InstancePerLifetimeScope()
                .ExternallyOwned()
                .Targeting(target)
                .CreateRegistration();
        }
    }
}
