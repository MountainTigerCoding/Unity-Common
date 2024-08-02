using System;
using UnityEngine;

namespace Runtime.Shared
{
    [Serializable]
    internal sealed class SingletonNotFoundException<T> : NullReferenceException
    {
        public SingletonNotFoundException () : base ("Singleton of type '" + typeof(T) + "' could not be found") {}
        public SingletonNotFoundException (UnityEngine.Object context) : base ("Singleton of type '" + typeof(T) + "' could not be found")
        {
#if UNITY_EDITOR
            Debug.LogError(Message, context);
#endif
        }
    }
}