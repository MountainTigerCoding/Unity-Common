using System;
using UnityEngine;

namespace Runtime.Shared
{
    [ExecuteAlways]
    [DisallowMultipleComponent]
    [AddComponentMenu("Shared/Unity API Call Distributer")]
    public sealed class UnityAPICallDistributer : SingletonBehaviour<UnityAPICallDistributer>
    {
    #region Fields
        [SerializeField] private bool _running = true;
        
        [SerializeField] private EventRegistory _onUpdate = new();
        [SerializeField] private EventRegistory _onLateUpdate = new();
        [SerializeField] private EventRegistory _onFixedUpdate = new();

#if UNITY_EDITOR
        [SerializeField] private EventRegistoryNullForgiving _onUpdateEditor = new();
#endif
    #endregion Fields

    #region Properties
        public EventRegistory OnUpdate { get => _onUpdate; }
        public EventRegistory OnLateUpdate { get => _onLateUpdate; }
        public EventRegistory OnFixedUpdate { get => _onFixedUpdate; }

#if UNITY_EDITOR
        public EventRegistoryNullForgiving OnUpdateEditor { get => _onUpdateEditor; }
#endif
    #endregion Properties

        protected override void Awake ()
        {
            base.Awake();
#if UNITY_EDITOR
            _onUpdateEditor.Clear();
#endif
            _onUpdate.Clear();
            _onFixedUpdate.Clear();
        }

        private void Update ()
        {
            if (!_running) return;

#if UNITY_EDITOR
            if (Application.isPlaying) _onUpdate.Invoke();
            else _onUpdateEditor.Invoke();
#else
            _onUpdate.Invoke();
#endif
        }

        private void LateUpdate ()
        {
            if (!_running) return;
            _onLateUpdate.Invoke();
        }

        private void FixedUpdate ()
        {
            if (!_running) return;
            _onFixedUpdate.Invoke();
        }

    #region Always Execute
        public void RegisterOnUpdateAlwaysExecute (EventRegistoryListener call)
        {
#if UNITY_EDITOR
            if (Application.isPlaying) _onUpdate.Register(call);
            else _onUpdateEditor.Register((Action)call.Target);
#else
            _onUpdate.Register(call);
#endif
        }

        public void UnregisterOnUpdateAlwaysExecute (EventRegistoryListener call)
        {
            _onUpdate.Unregister(call);
#if UNITY_EDITOR
            _onUpdateEditor.Unregister((Action)call.Target);
#endif
        }
    #endregion Always Execute

    #region Clearing
#if UNITY_EDITOR
        [ContextMenu("Clear Update")]
        private void ClearUpdateListeners ()
        {
            _onUpdate.Clear();
        }

        [ContextMenu("Clear Late Update")]
        private void ClearLateUpdateListeners ()
        {
            _onLateUpdate.Clear();
        }

        [ContextMenu("Clear Fixed Update")]
        private void ClearFixedUpdateListeners ()
        {
            _onFixedUpdate.Clear();
        }


        [ContextMenu("Clear Editor Update")]
        private void ClearEditorUpdateListeners ()
        {
            _onUpdateEditor.Clear();
        }
#endif
    #endregion Clearing
    }
}