using UnityEngine;

namespace Core.Utilities
{
    public static class ValidationUtility
    {
        public static bool NotNull<T>(T obj, Object context, string name = null)
        {
            if (obj != null)
                return true;

            var finalName = name ?? typeof(T).Name;
            Debug.LogError($"{context.GetType().Name} :: Missing reference → {finalName}", context);
            return false;
        }
    }
}