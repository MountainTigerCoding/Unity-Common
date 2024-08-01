using UnityEngine;

namespace Runtime.Shared
{
    [ExecuteInEditMode]
    [DisallowMultipleComponent]
    public class SingletonControlledBehaviour<TMaster> : MonoBehaviour where TMaster : SingletonBehaviour<TMaster>
    {
    #region Fields
        /// <summary>
        /// Must be implemented in inherited class in order to work
        /// </summary>
        [SerializeField] private bool _isListening = true;
        protected TMaster _master { private set; get; }
    #endregion

#if UNITY_EDITOR
        protected virtual void OnValidate ()
        {
			if (UnityEditor.EditorUtility.IsPersistent(this)) return;

            if (_master == null) return;
            if (_master.Listeners == null) return;
            if (_isListening) BeginSingletonListening();
            else EndSingletonListening();
            OnCall();
        }
#endif

        protected virtual void OnEnable ()
        {
            InitReferences();
            BeginSingletonListening();
        }

        protected virtual void OnDisable ()
        {
            EndSingletonListening();
        }

        protected virtual void InitReferences ()
        {
            if (_master == null) _master = SingletonBehaviour<TMaster>.Instance;
        }

        protected void BeginSingletonListening ()
        {
            _isListening = true;
            _master.Listeners.Register(OnCall);
        }

        protected void EndSingletonListening ()
        {
            _isListening = false;
            _master.Listeners.Unregister(OnCall);
        }

        private void OnCall ()
        {
            if (!enabled) return;

            if (!_isListening && _master == null) {
                InitReferences();
                if (_master == null) throw new SingletonNotFoundException<TMaster>(gameObject);
            }

            if (_isListening) OnUpdateListening();
            else OnUpdateIgnore();
        }

        protected virtual void OnUpdateListening () { }
        protected virtual void OnUpdateIgnore () { }
    }
}