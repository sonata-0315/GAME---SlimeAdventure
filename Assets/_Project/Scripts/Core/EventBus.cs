using System;
using System.Collections.Generic;
using UnityEngine;

namespace PrecisionPlatformer.Core
{
    /// <summary>
    /// Event Bus pattern for decoupled communication between systems.
    /// Uses strongly-typed events with readonly structs for zero-allocation messaging.
    /// Students use this pattern but should not modify this file.
    /// </summary>
    public static class EventBus
    {
        // Dictionary mapping event types to their subscriber lists
        private static readonly Dictionary<Type, Delegate> eventDictionary = new Dictionary<Type, Delegate>();

        /// <summary>
        /// Subscribe to an event of type T.
        /// </summary>
        /// <typeparam name="T">Event type (must be a struct)</typeparam>
        /// <param name="handler">Callback to invoke when event is published</param>
        public static void Subscribe<T>(Action<T> handler) where T : struct
        {
            Type eventType = typeof(T);

            if (eventDictionary.TryGetValue(eventType, out Delegate existingDelegate))
            {
                // Add to existing delegate chain
                eventDictionary[eventType] = Delegate.Combine(existingDelegate, handler);
            }
            else
            {
                // Create new entry
                eventDictionary[eventType] = handler;
            }
        }

        /// <summary>
        /// Unsubscribe from an event of type T.
        /// IMPORTANT: Always unsubscribe in OnDestroy to prevent memory leaks!
        /// </summary>
        /// <typeparam name="T">Event type (must be a struct)</typeparam>
        /// <param name="handler">Callback to remove from subscribers</param>
        public static void Unsubscribe<T>(Action<T> handler) where T : struct
        {
            Type eventType = typeof(T);

            if (eventDictionary.TryGetValue(eventType, out Delegate existingDelegate))
            {
                Delegate newDelegate = Delegate.Remove(existingDelegate, handler);

                if (newDelegate == null)
                {
                    // No more subscribers, remove entry
                    eventDictionary.Remove(eventType);
                }
                else
                {
                    eventDictionary[eventType] = newDelegate;
                }
            }
        }

        /// <summary>
        /// Publish an event to all subscribers.
        /// </summary>
        /// <typeparam name="T">Event type (must be a struct)</typeparam>
        /// <param name="eventData">Event data to pass to subscribers</param>
        public static void Publish<T>(T eventData) where T : struct
        {
            Type eventType = typeof(T);

            if (eventDictionary.TryGetValue(eventType, out Delegate del))
            {
                // Invoke all subscribers
                Action<T> callback = del as Action<T>;
                callback?.Invoke(eventData);
            }
        }

        /// <summary>
        /// Clear all event subscribers (typically called on scene transitions).
        /// IMPORTANT: Call this to prevent memory leaks when changing scenes!
        /// </summary>
        public static void ClearAll()
        {
            Debug.Log($"EventBus: Clearing all {eventDictionary.Count} event subscriptions");
            eventDictionary.Clear();
        }

        /// <summary>
        /// Get the number of subscribers for a specific event type.
        /// Useful for debugging.
        /// </summary>
        /// <typeparam name="T">Event type</typeparam>
        /// <returns>Number of subscribers</returns>
        public static int GetSubscriberCount<T>() where T : struct
        {
            Type eventType = typeof(T);

            if (eventDictionary.TryGetValue(eventType, out Delegate del))
            {
                return del.GetInvocationList().Length;
            }

            return 0;
        }
    }
}
