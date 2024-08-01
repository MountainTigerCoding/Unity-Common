using UnityEngine;
using Unity.Services.Analytics;
using Unity.Services.Core;
using Runtime.Shared;

namespace Analytics
{
    [AddComponentMenu("UGS/Analytics")]
    public sealed class UGSAnalytics : MonoBehaviour
    {
    #region Fields
        [SerializeField] private bool _logMessages = true;
        [SerializeField, ReadOnlyInInspector] private bool _running = false;
    #endregion

    #region Properties
        public bool Running { get => _running; }
    #endregion

        private async void Start ()
        {
            _running = false;

            try {
                await UnityServices.InitializeAsync();
                GiveConsent(); //Get user consent according to various legislations
                _running = true;
            }
            
            catch (ConsentCheckException error) {
                _running = false;
#if UNITY_EDITOR
                if(_logMessages) Debug.Log("Unity Analytics: Init failed: " + error.ToString());
#endif
            }
        }

        public void GiveConsent ()
        {
            // Call if consent has been given by the user
            AnalyticsService.Instance.StartDataCollection();

#if UNITY_EDITOR
            if (_logMessages) Debug.Log("Unity Analytics: " + $"Consent has been provided. The SDK is now collecting data!");
#endif
        }
    }
}