/* Consider supporting mutliple services.
 */

using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using CommonServiceLocator;
using RichPackage.GuardClauses;
using UnityEngine;

namespace RichPackage
{
    public delegate object Provider();

    /// <summary>
    /// A common method of finding services within the Unity Game Engine.
    /// </summary>
    public sealed class UnityServiceLocator : ServiceLocatorImplBase
    {
        /// <summary>
        /// Default implementation of a service provider.
        /// </summary>
        public readonly static UnityServiceLocator Instance;

        private readonly Dictionary<Type, object> services;
        private readonly Dictionary<Type, Provider> providers;

        #region Constructors

        static UnityServiceLocator()
        {
            Instance = new UnityServiceLocator();
        }

        public UnityServiceLocator()
        {
            services = new Dictionary<Type, object>();
            providers = new Dictionary<Type, Provider>();
        }

        #endregion Constructors

        /// <summary>
        /// Not implemented. There should only be one service, unless I say otherwise.
        /// </summary>
        /// <param name="serviceType"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        protected override IEnumerable<object> DoGetAllInstances(Type serviceType)
        {
            throw new NotImplementedException("There is never more than one service in existance.");
        }

        protected override object DoGetInstance(Type serviceType, string key)
        {
            object service; // return value
            Provider provider; // temp value
            GameObject gameObject; // temp value

            // get the service through common means
            if (services.TryGetValue(serviceType, out service))
            {
                // got it
            }
            else if (providers.TryGetValue(serviceType, out provider))
            {
                // get it
                service = provider();
            }
            else if (serviceType.FullName.Contains("UnityEngine.Object"))
            {
                if (!string.IsNullOrEmpty(key)
                    && ((gameObject = GameObject.FindGameObjectWithTag(key)) != null
                        || (gameObject = GameObject.Find(key)) != null)) // string lo
                {
                    // get it
                    service = gameObject.GetComponent(serviceType);
                }
                else
                {
                    // get it
                    service = GameObject.FindObjectOfType(serviceType);
                }
            }

            // return or throw
            return service ?? throw new NullReferenceException(
                $"Could not locate a {serviceType} service."); ;
        }

        /// <summary>
        /// Register a method of locating a service.
        /// </summary>
        /// <typeparam name="TService">The type of service <paramref name="service"/> is.</typeparam>
        /// <param name="service">A non-null instance of the service to provide.</param>
        /// <exception cref="ArgumentException"></exception>
        /// <exception cref="InvalidOperationException"></exception>
        public void RegisterProvider<TService>(Provider provider)
        {
            // validate
            GuardAgainst.ArgumentIsNull(provider, nameof(provider));

            if (providers.ContainsKey(typeof(TService)))
            {
                // error
                throw new InvalidOperationException($"There is already a {typeof(TService).Name}" +
                    $" service provider registered to {nameof(UnityServiceLocator)}.");
            }
            else
            {
                providers.Add(typeof(TService), provider);
            }
        }

        /// <summary>
        /// Register a new service.
        /// </summary>
        /// <typeparam name="TService">The type of service <paramref name="service"/> is.</typeparam>
        /// <param name="service">A non-null instance of the service to provide.</param>
        /// <exception cref="ArgumentException"></exception>
        /// <exception cref="InvalidOperationException"></exception>
        public void RegisterService<TService>(TService service)
        {
            // validate
            GuardAgainst.ArgumentIsNull(service, nameof(service));

            if (services.ContainsKey(typeof(TService)))
            {
                // error
                throw new InvalidOperationException($"There is already a {typeof(TService).Name}" +
                    $" service registered to {nameof(UnityServiceLocator)}.");
            }
            else
            {
                services.Add(typeof(TService), service);
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void DeregisterProvider<TService>(TService _) // handy way of avoiding <Type>
        {
            DeregisterProvider<TService>();
        }

        public void DeregisterProvider<TService>()
        {
            providers.Remove(typeof(TService));
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void DeregisterService<TService>(TService _) // handy way of avoiding <Type>
        {
            DeregisterService<TService>();
        }

        public void DeregisterService<TService>()
        {
            services.Remove(typeof(TService));
        }
    }
}
