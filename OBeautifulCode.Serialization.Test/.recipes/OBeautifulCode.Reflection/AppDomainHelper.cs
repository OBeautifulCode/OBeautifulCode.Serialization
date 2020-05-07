﻿// --------------------------------------------------------------------------------------------------------------------
// <copyright file="AppDomainHelper.cs" company="OBeautifulCode">
//   Copyright (c) OBeautifulCode 2018. All rights reserved.
// </copyright>
// <auto-generated>
//   Sourced from NuGet package. Will be overwritten with package update except in OBeautifulCode.Reflection.Recipes source.
// </auto-generated>
// --------------------------------------------------------------------------------------------------------------------

namespace OBeautifulCode.Reflection.Recipes
{
    using System;
    using System.Diagnostics.CodeAnalysis;
    using System.Security.Policy;

    using OBeautifulCode.Assertion.Recipes;
    using OBeautifulCode.Assertion.Recipes.Internal;

    /// <summary>
    /// Provides useful methods for creating and executing code within an <see cref="AppDomain"/>.
    /// </summary>
    /// <remarks>
    /// Adapted from <a href="https://malvinly.com/2012/04/08/executing-code-in-a-new-application-domain/" />.
    /// </remarks>
#if !OBeautifulCodeReflectionRecipesProject
    [System.Diagnostics.CodeAnalysis.ExcludeFromCodeCoverage]
    [System.CodeDom.Compiler.GeneratedCode("OBeautifulCode.Reflection.Recipes", "See package version number")]
    internal
#else
    public
#endif
    static class AppDomainHelper
    {
        /// <summary>
        /// Creates a disposable <see cref="AppDomain"/>.
        /// </summary>
        /// <param name="name">OPTIONAL friendly name of the domain.  DEFAULT uses <see cref="Guid.NewGuid"/>.</param>
        /// <param name="securityInfo">OPTIONAL object that contains evidence mapped through the security policy to establish a top-of-stack permission set.  DEFAULT to create a new <see cref="Evidence"/> using the <see cref="AppDomain.Evidence"/> of the <see cref="AppDomain.CurrentDomain"/>.</param>
        /// <param name="appDomainInfo">OPTIONAL object that contains application domain initialization information.  DEFAULT is to use a new <see cref="AppDomainSetup"/>, setting <see cref="AppDomainSetup.ApplicationBase"/> using <see cref="AppDomainSetup.ApplicationBase"/> of the <see cref="AppDomain.CurrentDomain"/>.</param>
        /// <returns>
        /// A disposable <see cref="AppDomain"/>.
        /// </returns>
        public static DisposableAppDomain CreateDisposableAppDomain(
            string name = null,
            Evidence securityInfo = null,
            AppDomainSetup appDomainInfo = null)
        {
            name = name ?? Guid.NewGuid().ToString();

            securityInfo = securityInfo ?? new Evidence(AppDomain.CurrentDomain.Evidence);

            appDomainInfo = appDomainInfo ?? new AppDomainSetup
            {
                ApplicationBase = AppDomain.CurrentDomain.SetupInformation.ApplicationBase,
            };

            var appDomain = AppDomain.CreateDomain(name, securityInfo, appDomainInfo);

            var result = new DisposableAppDomain(appDomain);

            return result;
        }

        /// <summary>
        /// Executes the specified <see cref="Action"/> in new Domain created by this method.
        /// </summary>
        /// <param name="action">The action to execute.</param>
        public static void ExecuteInNewAppDomain(
            this Action action)
        {
            new { action }.AsArg().Must().NotBeNull();

            using (var disposableAppDomain = CreateDisposableAppDomain())
            {
                disposableAppDomain.Execute(action);
            }
        }

        /// <summary>
        /// Executes the specified <see cref="Action"/> in the specified Domain.
        /// </summary>
        /// <param name="action">The action to execute.</param>
        /// <param name="disposableAppDomain">The Domain within which to execute the specified action.</param>
        public static void ExecuteInAppDomain(
            this Action action,
            DisposableAppDomain disposableAppDomain)
        {
            new { action }.AsArg().Must().NotBeNull();
            new { disposableAppDomain }.AsArg().Must().NotBeNull();

            disposableAppDomain.Execute(action);
        }

        /// <summary>
        /// Executes the specified <see cref="Action"/> in the specified Domain.
        /// </summary>
        /// <param name="disposableAppDomain">The Domain within which to execute the specified action.</param>
        /// <param name="action">The action to execute.</param>
        public static void Execute(
            this DisposableAppDomain disposableAppDomain,
            Action action)
        {
            new { disposableAppDomain }.AsArg().Must().NotBeNull();
            new { action }.AsArg().Must().NotBeNull();

            var domainDelegate = disposableAppDomain.BuildAppDomainDelegate();

            domainDelegate.Execute(action);
        }

        /// <summary>
        /// Executes the specified <see cref="Action{T}"/> in a new Domain created by this method.
        /// </summary>
        /// <typeparam name="T">The type of the parameter of the method that <paramref name="action"/> encapsulates.</typeparam>
        /// <param name="action">The action to execute.</param>
        /// <param name="parameter">The parameter to pass to the method that <paramref name="action"/> encapsulates.</param>
        public static void ExecuteInNewAppDomain<T>(
            this Action<T> action,
            T parameter)
        {
            new { action }.AsArg().Must().NotBeNull();

            using (var disposableAppDomain = CreateDisposableAppDomain())
            {
                disposableAppDomain.Execute(action, parameter);
            }
        }

        /// <summary>
        /// Executes the specified <see cref="Action{T}"/> in the specified Domain.
        /// </summary>
        /// <typeparam name="T">The type of the parameter of the method that <paramref name="action"/> encapsulates.</typeparam>
        /// <param name="action">The action to execute.</param>
        /// <param name="parameter">The parameter to pass to the method that <paramref name="action"/> encapsulates.</param>
        /// <param name="disposableAppDomain">The Domain within which to execute the specified action.</param>
        public static void ExecuteInAppDomain<T>(
            this Action<T> action,
            T parameter,
            DisposableAppDomain disposableAppDomain)
        {
            new { action }.AsArg().Must().NotBeNull();
            new { disposableAppDomain }.AsArg().Must().NotBeNull();

            disposableAppDomain.Execute(action, parameter);
        }

        /// <summary>
        /// Executes the specified <see cref="Action{T}"/> in the specified Domain.
        /// </summary>
        /// <typeparam name="T">The type of the parameter of the method that <paramref name="action"/> encapsulates.</typeparam>
        /// <param name="disposableAppDomain">The Domain within which to execute the specified action.</param>
        /// <param name="action">The action to execute.</param>
        /// <param name="parameter">The parameter to pass to the method that <paramref name="action"/> encapsulates.</param>
        public static void Execute<T>(
            this DisposableAppDomain disposableAppDomain,
            Action<T> action,
            T parameter)
        {
            new { disposableAppDomain }.AsArg().Must().NotBeNull();
            new { action }.AsArg().Must().NotBeNull();

            var domainDelegate = disposableAppDomain.BuildAppDomainDelegate();

            domainDelegate.Execute(action, parameter);
        }

        /// <summary>
        /// Executes the specified <see cref="Func{TResult}"/> in a new Domain created by this method.
        /// </summary>
        /// <typeparam name="TResult">The type of the return value of the method that <paramref name="func"/> encapsulates.</typeparam>
        /// <param name="func">The func to execute.</param>
        /// <returns>
        /// The return value from executing the specified <see cref="Func{TResult}"/> in a new Domain created by this method.
        /// </returns>
        public static TResult ExecuteInNewAppDomain<TResult>(
            this Func<TResult> func)
        {
            new { func }.AsArg().Must().NotBeNull();

            using (var disposableAppDomain = CreateDisposableAppDomain())
            {
                var result = disposableAppDomain.Execute(func);

                return result;
            }
        }

        /// <summary>
        /// Executes the specified <see cref="Func{TResult}"/> in the specified Domain.
        /// </summary>
        /// <typeparam name="TResult">The type of the return value of the method that <paramref name="func"/> encapsulates.</typeparam>
        /// <param name="func">The func to execute.</param>
        /// <param name="disposableAppDomain">The Domain within which to execute the specified func.</param>
        /// <returns>
        /// The return value from executing the specified <see cref="Func{TResult}"/> in the specified Domain.
        /// </returns>
        public static TResult ExecuteInAppDomain<TResult>(
            this Func<TResult> func,
            DisposableAppDomain disposableAppDomain)
        {
            new { func }.AsArg().Must().NotBeNull();
            new { disposableAppDomain }.AsArg().Must().NotBeNull();

            var result = disposableAppDomain.Execute(func);

            return result;
        }

        /// <summary>
        /// Executes the specified <see cref="Func{TResult}"/> in the specified Domain.
        /// </summary>
        /// <typeparam name="TResult">The type of the return value of the method that <paramref name="func"/> encapsulates.</typeparam>
        /// <param name="disposableAppDomain">The Domain within which to execute the specified func.</param>
        /// <param name="func">The func to execute.</param>
        /// <returns>
        /// The return value from executing the specified <see cref="Func{TResult}"/> in the specified Domain.
        /// </returns>
        public static TResult Execute<TResult>(
            this DisposableAppDomain disposableAppDomain,
            Func<TResult> func)
        {
            new { disposableAppDomain }.AsArg().Must().NotBeNull();
            new { func }.AsArg().Must().NotBeNull();

            var domainDelegate = disposableAppDomain.BuildAppDomainDelegate();

            var result = domainDelegate.Execute(func);

            return result;
        }

        /// <summary>
        /// Executes the specified <see cref="Func{T, TResult}"/> in a new Domain created by this method.
        /// </summary>
        /// <typeparam name="T">The type of the parameter of the method that <paramref name="func"/> encapsulates.</typeparam>
        /// <typeparam name="TResult">The type of the return value of the method that <paramref name="func"/> encapsulates.</typeparam>
        /// <param name="func">The func to execute.</param>
        /// <param name="parameter">The parameter to pass to the method that <paramref name="func"/> encapsulates.</param>
        /// <returns>
        /// The return value from executing the specified <see cref="Func{T, TResult}"/> in a new Domain created by this method.
        /// </returns>
        public static TResult ExecuteInNewAppDomain<T, TResult>(
            this Func<T, TResult> func,
            T parameter)
        {
            new { func }.AsArg().Must().NotBeNull();

            using (var disposableAppDomain = CreateDisposableAppDomain())
            {
                var result = disposableAppDomain.Execute(func, parameter);

                return result;
            }
        }

        /// <summary>
        /// Executes the specified <see cref="Func{T, TResult}"/> in the specified Domain.
        /// </summary>
        /// <typeparam name="T">The type of the parameter of the method that <paramref name="func"/> encapsulates.</typeparam>
        /// <typeparam name="TResult">The type of the return value of the method that <paramref name="func"/> encapsulates.</typeparam>
        /// <param name="func">The func to execute.</param>
        /// <param name="parameter">The parameter to pass to the method that <paramref name="func"/> encapsulates.</param>
        /// <param name="disposableAppDomain">The Domain within which to execute the specified func.</param>
        /// <returns>
        /// The return value from executing the specified <see cref="Func{T, TResult}"/> in the specified Domain.
        /// </returns>
        public static TResult ExecuteInAppDomain<T, TResult>(
            this Func<T, TResult> func,
            T parameter,
            DisposableAppDomain disposableAppDomain)
        {
            new { func }.AsArg().Must().NotBeNull();
            new { disposableAppDomain }.AsArg().Must().NotBeNull();

            var result = disposableAppDomain.Execute(func, parameter);

            return result;
        }

        /// <summary>
        /// Executes the specified <see cref="Func{T, TResult}"/> in the specified Domain.
        /// </summary>
        /// <typeparam name="T">The type of the parameter of the method that <paramref name="func"/> encapsulates.</typeparam>
        /// <typeparam name="TResult">The type of the return value of the method that <paramref name="func"/> encapsulates.</typeparam>
        /// <param name="disposableAppDomain">The Domain within which to execute the specified func.</param>
        /// <param name="func">The func to execute.</param>
        /// <param name="parameter">The parameter to pass to the method that <paramref name="func"/> encapsulates.</param>
        /// <returns>
        /// The return value from executing the specified <see cref="Func{T, TResult}"/> in the specified Domain.
        /// </returns>
        public static TResult Execute<T, TResult>(
            this DisposableAppDomain disposableAppDomain,
            Func<T, TResult> func,
            T parameter)
        {
            new { disposableAppDomain }.AsArg().Must().NotBeNull();
            new { func }.AsArg().Must().NotBeNull();

            var domainDelegate = disposableAppDomain.BuildAppDomainDelegate();

            var result = domainDelegate.Execute(func, parameter);

            return result;
        }

        /// <summary>
        /// Executes the specified <see cref="Func{T1, T2, TResult}"/> in a new Domain created by this method.
        /// </summary>
        /// <typeparam name="T1">The type of the first parameter of the method that <paramref name="func"/> encapsulates.</typeparam>
        /// <typeparam name="T2">The type of the second parameter of the method that <paramref name="func"/> encapsulates.</typeparam>
        /// <typeparam name="TResult">The type of the return value of the method that <paramref name="func"/> encapsulates.</typeparam>
        /// <param name="func">The func to execute.</param>
        /// <param name="parameter1">The first parameter to pass to the method that <paramref name="func"/> encapsulates.</param>
        /// <param name="parameter2">The second parameter to pass to the method that <paramref name="func"/> encapsulates.</param>
        /// <returns>
        /// The return value from executing the specified <see cref="Func{T1, T2, TResult}"/> in a new Domain created by this method.
        /// </returns>
        public static TResult ExecuteInNewAppDomain<T1, T2, TResult>(
            this Func<T1, T2, TResult> func,
            T1 parameter1,
            T2 parameter2)
        {
            new { func }.AsArg().Must().NotBeNull();

            using (var disposableAppDomain = CreateDisposableAppDomain())
            {
                var result = disposableAppDomain.Execute(func, parameter1, parameter2);

                return result;
            }
        }

        /// <summary>
        /// Executes the specified <see cref="Func{T1, T2, TResult}"/> in the specified Domain.
        /// </summary>
        /// <typeparam name="T1">The type of the first parameter of the method that <paramref name="func"/> encapsulates.</typeparam>
        /// <typeparam name="T2">The type of the second parameter of the method that <paramref name="func"/> encapsulates.</typeparam>
        /// <typeparam name="TResult">The type of the return value of the method that <paramref name="func"/> encapsulates.</typeparam>
        /// <param name="func">The func to execute.</param>
        /// <param name="parameter1">The first parameter to pass to the method that <paramref name="func"/> encapsulates.</param>
        /// <param name="parameter2">The second parameter to pass to the method that <paramref name="func"/> encapsulates.</param>
        /// <param name="disposableAppDomain">The Domain within which to execute the specified func.</param>
        /// <returns>
        /// The return value from executing the specified <see cref="Func{T1, T2, TResult}"/> in the specified Domain.
        /// </returns>
        public static TResult ExecuteInAppDomain<T1, T2, TResult>(
            this Func<T1, T2, TResult> func,
            T1 parameter1,
            T2 parameter2,
            DisposableAppDomain disposableAppDomain)
        {
            new { func }.AsArg().Must().NotBeNull();
            new { disposableAppDomain }.AsArg().Must().NotBeNull();

            var result = disposableAppDomain.Execute(func, parameter1, parameter2);

            return result;
        }

        /// <summary>
        /// Executes the specified <see cref="Func{T1, T2, TResult}"/> in the specified Domain.
        /// </summary>
        /// <typeparam name="T1">The type of the first parameter of the method that <paramref name="func"/> encapsulates.</typeparam>
        /// <typeparam name="T2">The type of the first parameter of the method that <paramref name="func"/> encapsulates.</typeparam>
        /// <typeparam name="TResult">The type of the return value of the method that <paramref name="func"/> encapsulates.</typeparam>
        /// <param name="disposableAppDomain">The Domain within which to execute the specified func.</param>
        /// <param name="func">The func to execute.</param>
        /// <param name="parameter1">The first parameter to pass to the method that <paramref name="func"/> encapsulates.</param>
        /// <param name="parameter2">The second parameter to pass to the method that <paramref name="func"/> encapsulates.</param>
        /// <returns>
        /// The return value from executing the specified <see cref="Func{T1, T2, TResult}"/> in the specified Domain.
        /// </returns>
        public static TResult Execute<T1, T2, TResult>(
            this DisposableAppDomain disposableAppDomain,
            Func<T1, T2, TResult> func,
            T1 parameter1,
            T2 parameter2)
        {
            new { disposableAppDomain }.AsArg().Must().NotBeNull();
            new { func }.AsArg().Must().NotBeNull();

            var domainDelegate = disposableAppDomain.BuildAppDomainDelegate();

            var result = domainDelegate.Execute(func, parameter1, parameter2);

            return result;
        }

        private static AppDomainDelegate BuildAppDomainDelegate(
            this DisposableAppDomain disposableAppDomain)
        {
            // ReSharper disable once AssignNullToNotNullAttribute
            var result = (AppDomainDelegate)disposableAppDomain.AppDomain.CreateInstanceAndUnwrap(
                typeof(AppDomainDelegate).Assembly.FullName,
                typeof(AppDomainDelegate).FullName);

            return result;
        }

        private class AppDomainDelegate : MarshalByRefObject
        {
            [SuppressMessage("Microsoft.Performance", "CA1822:MarkMembersAsStatic", Justification = "This is how the method was specified in the blog post.  Will making it static somehow break the consumer because of MarshalByRefObject?")]
            public void Execute(
                Action action)
            {
                action();
            }

            [SuppressMessage("Microsoft.Performance", "CA1822:MarkMembersAsStatic", Justification = "This is how the method was specified in the blog post.  Will making it static somehow break the consumer because of MarshalByRefObject?")]
            public void Execute<T>(
                Action<T> action,
                T parameter)
            {
                action(parameter);
            }

            [SuppressMessage("Microsoft.Performance", "CA1822:MarkMembersAsStatic", Justification = "This is how the method was specified in the blog post.  Will making it static somehow break the consumer because of MarshalByRefObject?")]
            public T Execute<T>(
                Func<T> func)
            {
                var result = func();

                return result;
            }

            [SuppressMessage("Microsoft.Performance", "CA1822:MarkMembersAsStatic", Justification = "This is how the method was specified in the blog post.  Will making it static somehow break the consumer because of MarshalByRefObject?")]
            public TResult Execute<T, TResult>(
                Func<T, TResult> func,
                T parameter)
            {
                var result = func(parameter);

                return result;
            }

            [SuppressMessage("Microsoft.Performance", "CA1822:MarkMembersAsStatic", Justification = "This is how the method was specified in the blog post.  Will making it static somehow break the consumer because of MarshalByRefObject?")]
            public TResult Execute<T1, T2, TResult>(
                Func<T1, T2, TResult> func,
                T1 parameter1,
                T2 parameter2)
            {
                var result = func(parameter1, parameter2);

                return result;
            }
        }
    }
}