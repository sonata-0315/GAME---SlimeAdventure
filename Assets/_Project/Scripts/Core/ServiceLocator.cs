using System;
using System.Collections.Generic;
using UnityEngine;

namespace PrecisionPlatformer.Core
{
    /// <summary>
    /// Service Locator pattern for dependency injection.
    /// Provides centralized service registration and retrieval.
    /// Students use this pattern but should not modify this file.
    /// </summary>
    public static class ServiceLocator
    {
        private static readonly Dictionary<Type, object> services = new Dictionary<Type, object>();

        /// <summary>
        /// Register a service instance.
        /// </summary>
        /// <typeparam name="T">Service type (typically an interface or base class)</typeparam>
        /// <param name="service">Service instance to register</param>
        public static void Register<T>(T service) where T : class
        {
            Type type = typeof(T);

            if (services.ContainsKey(type))
            {
                Debug.LogWarning($"ServiceLocator: Service of type {type.Name} is already registered. Replacing...");
                services[type] = service;
            }
            else
            {
                services.Add(type, service);
                Debug.Log($"ServiceLocator: Registered service {type.Name}");
            }
        }

        /// <summary>
        /// Retrieve a registered service.
        /// </summary>
        /// <typeparam name="T">Service type to retrieve</typeparam>
        /// <returns>Service instance, or null if not found</returns>
        public static T Get<T>() where T : class
        {
            Type type = typeof(T);

            if (services.TryGetValue(type, out object service))
            {
                return service as T;
            }

            Debug.LogWarning($"ServiceLocator: Service of type {type.Name} not found!");
            return null;
        }

        /// <summary>
        /// Unregister a service (typically called on scene unload or service destruction).
        /// </summary>
        /// <typeparam name="T">Service type to unregister</typeparam>
        public static void Unregister<T>() where T : class
        {
            Type type = typeof(T);

            if (services.Remove(type))
            {
                Debug.Log($"ServiceLocator: Unregistered service {type.Name}");
            }
            else
            {
                Debug.LogWarning($"ServiceLocator: Attempted to unregister service {type.Name}, but it was not registered.");
            }
        }

        /// <summary>
        /// Clear all registered services (typically called on game reset or scene transitions).
        /// </summary>
        public static void ClearAll()
        {
            Debug.Log($"ServiceLocator: Clearing all {services.Count} registered services");
            services.Clear();
        }

        /// <summary>
        /// Check if a service is registered.
        /// </summary>
        /// <typeparam name="T">Service type to check</typeparam>
        /// <returns>True if service is registered</returns>
        public static bool IsRegistered<T>() where T : class
        {
            return services.ContainsKey(typeof(T));
        }
    }
}
