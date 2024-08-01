using UnityEngine;

using Runtime.Shared;

namespace Runtime.Shared
{
    [RequireComponent(typeof(Camera))]
    [AddComponentMenu("Shared/Camera Manager")]
    public class CameraManager : MonoBehaviour
    {

#region Singleton
        private static CameraManager _instance;
        public static CameraManager Instance
        {
            get
            {
                if (_instance == null) {
                    _instance = FindObjectOfType<CameraManager>(true);
                    
                    if (_instance == null) {
                        Debug.LogError("The singleton '" + nameof(CameraManager) + "' is trying to be accesed but is null. No component could be found.");
                        return null;
                    }

                    _instance.FindReferences();
                }
                return _instance;
            }
            private set
            {
                _instance = value;
            }
        }
#endregion Singleton


#region Fields
        [SerializeField, ReadOnlyInInspector] private Transform _lookAtObject;
        [SerializeField] private float _lookAtObjectSearchDistance = 5f;
        [SerializeField] private float _lookAtOriginOffset = 0.1f;
        [SerializeField] private float _searchSphereRadius = 0.2f;
        private Camera _mainCamera;
#endregion


#region Properties
        public Camera MainCamera { get => _mainCamera; }
        public Transform LookAtObject { get => _lookAtObject; }
#endregion


        private void Awake ()
        {
            if (_instance == null) {
                _instance = this;
            }
            
            FindReferences();
        }

        private void Start ()
        {
            UnityAPICallDistributer.Instance.OnUpdate.Register(OnUpdate);
        }

        public void FindReferences ()
        {
            _mainCamera = transform.GetComponent<Camera>();
        }

        private void OnUpdate ()
        {
            Vector3 forward = _mainCamera.transform.forward;
            Ray ray = new(_mainCamera.transform.position + forward * _lookAtOriginOffset, forward);

            if (Physics.SphereCast(ray, _searchSphereRadius, out RaycastHit result, _lookAtObjectSearchDistance)) {
                _lookAtObject = result.transform;
            } else {
                _lookAtObject = null;
            }

            Debug.DrawRay(ray.origin, ray.direction * result.distance, Color.red);
        }
    }
}