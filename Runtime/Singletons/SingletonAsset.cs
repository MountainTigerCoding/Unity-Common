using UnityEngine;

namespace Runtime.Shared
{
    public class SingletonAsset <TMaster> : ScriptableObject where TMaster : ScriptableObject
    {
    #region Fields
        private static TMaster _instance;
        public EventRegistory Listeners { private set; get; } = new();
    #endregion

    #region Properties
        public static TMaster Instance
        {
            set => _instance = value;
            get {
                if (_instance == null) {
                    _instance = FindObjectOfType<TMaster>(true);
                    if (_instance == null) throw new SingletonNotFoundException<TMaster>();
                }
                return _instance;
            }
        }
    #endregion Properties

        protected virtual void Awake ()
        {
            if (_instance == null) _instance = this as TMaster;
        }
    }
}