using UnityEngine;

namespace Runtime.Shared
{
    [CreateAssetMenu(fileName = "Time_Manager", menuName = "State/Time Manager")]
    public sealed class TimeManager : ScriptableObject
    {
    #region Singleton
        private static TimeManager _instance;
        public static TimeManager Instance
        {
            get
            {
                if (_instance == null) {
                    _instance = FindObjectOfType<TimeManager>();
                    if (_instance == null) {
                        Debug.LogError("The singleton '" + nameof(TimeManager) + "' is trying to be accesed but is null. No component could be found.");
                        return null;
                    }
                }
                return _instance;
            }
        }
    #endregion Singleton

    #region Fields
        [SerializeField] private float _targetTimeScale = 1f;
        [SerializeField, ReadOnlyInInspector] private float _timeScale = 1f;
    #endregion

    #region Properties
        public float TargetTimeScale { get => _targetTimeScale; }
    #endregion

        private void OnEnable ()
        {
            _instance = this;
            SetTargetTimeScale(1f);
        }

        public void OnValidate ()
        {
            if (Application.isPlaying) SetTargetTimeScale(1f);
            else SetTargetTimeScale(_targetTimeScale);
        }

        public void SetTargetTimeScale (float scale)
        {
            if (scale < 0f) scale = 0f;
            _targetTimeScale = scale;
            SetTimeScale(_targetTimeScale);
        }

        private void SetTimeScale (float scale)
        {
            if (scale < 0f) scale = 0f;
            Time.timeScale = scale;
            Time.fixedDeltaTime = scale * 0.02f;

            _timeScale = scale;
        }

    #region Shortcut Actions
        public void Pause ()
        {
            if (!Application.isPlaying) return;
            
            Debug.Log("Time paused");
            SetTimeScale(0f);
        }

        public void Resume ()
        {
            if (!Application.isPlaying) return;

            Debug.Log("Time resumed");
            SetTimeScale(TargetTimeScale);
        }
    #endregion Shortcut Actions
    }
}