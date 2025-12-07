using System;
using System.Collections.Generic;
using UnityEngine;

namespace Core.Services
{
    public static class ServiceLocator
    {
        private static readonly Dictionary<Type, object> Services = new();

        public static void Register<T>(T service)
        {
            var type = typeof(T);
            Services[type] = service;
        }

        public static T Resolve<T>()
        {
            var type = typeof(T);

            if (Services.TryGetValue(type, out var service))
                return (T)service;

            Debug.LogError($"[ServiceLocator] Service not found: {type.Name}");
            return default;
        }
    }
}