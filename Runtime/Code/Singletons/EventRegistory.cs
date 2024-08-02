using UnityEngine;

namespace Runtime.Shared
{
    public delegate void EventRegistoryListener ();

#if UNITY_EDITOR
    [System.Serializable]
#endif
    public sealed class EventRegistory
    {
    #region Fields
        [SerializeField] private int _numListeners = 0;
        private EventRegistoryListener _listeners;
    #endregion

        public EventRegistory ()
        {
            Clear();
        }

        public void Register (EventRegistoryListener call)
        {
            _listeners += call;
            _numListeners++;
        }

        public void Unregister (EventRegistoryListener call)
        {
            _listeners -= call;
            _numListeners--;
        }

        public void Clear ()
        {
            _listeners = null;
            _numListeners = 0;
        }

        public void Invoke () => _listeners?.Invoke();
    }
}