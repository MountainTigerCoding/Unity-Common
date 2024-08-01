#if UNITY_EDITOR
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Runtime.Shared
{
    /// <summary>
    /// Should only be used in the editor when non-serialized fields can be reset causing null reference errors.
    /// Use EventRegistory instead for runtime code
    /// </summary>
    [Serializable]
    public sealed class EventRegistoryNullForgiving
    {
    #region Fields
        [SerializeField] private int _numListeners = 0;
        private List<Action> _listeners = new();
    #endregion

        public void Register (Action call)
        {
            if (Application.isPlaying) throw new("Cannot register to EventRegistoryNullForgiving in play mode");
            _listeners.Add(call);
            _numListeners++;
        }

        public void Unregister (Action call)
        {
            _listeners.Remove(call);
            _numListeners--;
        }

        public void Clear ()
        {
            _listeners.Clear();
            _numListeners = 0;
        }

        public void Invoke ()
        {
            if (_listeners == null) return;
            if (_listeners.Count == 0) return;

            for (int i = 0; i < _listeners.Count; i++)
            {
                Action listener = _listeners[i];
                if (listener == null) {
                    _listeners.RemoveAt(i);
                    i--;
                    continue;
                }

                listener.Invoke();
            }
        }
    }
}
#endif