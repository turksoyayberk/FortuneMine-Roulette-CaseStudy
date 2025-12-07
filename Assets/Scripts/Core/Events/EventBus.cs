using System;
using System.Collections.Generic;
using Debug = UnityEngine.Debug;

namespace Core.Events
{
    public static class EventBus
    {
        private static readonly Dictionary<Type, List<(int order, Delegate action)>> InvocationDict = new();

        public static void UnSubscribe<T>(Action<T> action) where T : IEvent
        {
            UnSubscribe(typeof(T), action);
        }

        public static void UnSubscribe<T>(Action action) where T : IEvent
        {
            UnSubscribe(typeof(T), action);
        }

        private static void UnSubscribe(Type type, Delegate action)
        {
            if (action == null) return;

            if (!InvocationDict.TryGetValue(type, out var actionList)) return;

            for (var i = actionList.Count - 1; i >= 0; i--)
            {
                if (actionList[i].action.Equals(action))
                {
                    actionList.RemoveAt(i);
                    break;
                }
            }

            if (actionList.Count == 0)
            {
                InvocationDict.Remove(type);
            }
        }


        public static void Subscribe<T>(Action<T> action, int invocationOrder = 0) where T : IEvent
        {
            Subscribe(typeof(T), action, invocationOrder);
        }

        public static void Subscribe<T>(Action action, int invocationOrder = 0) where T : IEvent
        {
            Subscribe(typeof(T), action, invocationOrder);
        }

        private static void Subscribe(Type type, Delegate action, int invocationOrder = 0)
        {
            if (action == null) return;

            if (!InvocationDict.TryGetValue(type, out var actionList))
            {
                actionList = new List<(int, Delegate)>();
                InvocationDict[type] = actionList;
            }
            
            var insertIndex = 0;
            for (var i = 0; i < actionList.Count; i++)
            {
                if (actionList[i].order > invocationOrder)
                {
                    insertIndex = i;
                    break;
                }

                insertIndex = i + 1;
            }

            actionList.Insert(insertIndex, (invocationOrder, action));
        }

        public static void Publish<T>(T eventInstance) where T : IEvent
        {
            var eventType = typeof(T);

            if (!InvocationDict.TryGetValue(eventType, out var actionList)) return;
            
            var count = actionList.Count;
            for (var i = 0; i < count; i++)
            {
                var (_, action) = actionList[i];
                
                try
                {
                    switch (action)
                    {
                        case Action<T> typedAction:
                            typedAction.Invoke(eventInstance);
                            break;
                        case Action typelessAction:
                            typelessAction.Invoke();
                            break;
                    }
                }
#if UNITY_EDITOR || DEVELOPMENT_BUILD
                catch (Exception ex)
                {
                    Debug.LogError($"EventBus error on event {eventType.Name}: {ex.Message}");
                }
#else
                catch
                {
                    // Ignore exceptions in release builds for performance
                }
#endif
            }
        }

        public static void GetAllEventsName()
        {
            foreach (var events in InvocationDict)
            {
                Debug.Log($"Event name: {events.Key.Name}");
            }
        }

        public static void RemoveAllListeners<T>() where T : IEvent
        {
            var eventType = typeof(T);
            InvocationDict.Remove(eventType);
        }

        // Optional: Clear all listeners for cleanup
        public static void Clear()
        {
            InvocationDict.Clear();
        }
    }
}