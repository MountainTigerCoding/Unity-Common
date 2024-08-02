using UnityEngine;

namespace Runtime.Shared
{
    [AddComponentMenu("Shared/Array Aligner")]
    public sealed class ArrayAligner : MonoBehaviour
    {
    #region Fields
        public Space Space;
        public bool Active = false;
        public bool UseStartingTransform = false;
        [ReadOnlyInInspector] public bool Bend = false;

        [Space(10f)]
        [SerializeField] private Vector3 startPosition;
        [SerializeField] private Vector3 startEulerAngles;
        [SerializeField] private Vector3 startScale = Vector3.one;

        [Space(10f)]
        [SerializeField] private Vector3 _positionOffset;
        [SerializeField] private Vector3 _eulerAnglesOffset;
        [SerializeField] private Vector3 _scaleOffset;

        [SerializeField, Tooltip("O = auto")] private int _startControlTranformIndex = 0;
        [SerializeField, Tooltip("O = auto")] private int _endControlTranformIndex = 0;
    #endregion Fields

    #region Properties
        public Vector3 StartPosition { get => startPosition; set => startPosition = value; }
        public Vector3 StartEulerAngles { get => startEulerAngles; set => startEulerAngles = value; }
        public Vector3 StartScale { get => startScale; set => startScale = value; }

        public Vector3 PositionOffset { get => _positionOffset; set => _positionOffset = value; }
        public Vector3 EulerAnglesOffset { get => _eulerAnglesOffset; set => _eulerAnglesOffset = value; }
        public Vector3 ScaleOffset { get => _scaleOffset; set => _scaleOffset = value; }
    #endregion Properties


        private void OnValidate ()
        {
            Modify();
        }

        private void Start ()
        {
            Modify();
        }

        public void Modify ()
        {
            if (!Active || !enabled) return;

            Vector3 origPosition = Vector3.zero;
            Vector3 origEulerAngles = Vector3.zero;
            Vector3 origScale = transform.localScale;

            if (UseStartingTransform) {
                origPosition = StartPosition;
                origEulerAngles = StartEulerAngles;
                origScale = StartScale;
            } else if (Space == Space.World) {
                origPosition = transform.position;
                origEulerAngles = transform.eulerAngles;
            }
            
            int startIndex = _startControlTranformIndex;
            int endIndex = _endControlTranformIndex;
            if (endIndex == 0) endIndex = transform.childCount;

            for (int i = startIndex; i < endIndex; i ++)
            {
                Transform child = transform.GetChild(i);

                Vector3 position;
                Vector3 eulerAngles;

                if (Space == Space.World) {
                    position = origPosition + (i * PositionOffset);
                    eulerAngles = origEulerAngles + (i * EulerAnglesOffset);
                } else {
                    position = origPosition + (i * PositionOffset);
                    eulerAngles = origEulerAngles + (i * EulerAnglesOffset);
                }
                child.localScale = origScale + (i * ScaleOffset);

                if (Space == Space.World) {
                    child.position = position;
                    child.eulerAngles = eulerAngles;
                } else {
                    child.localPosition = position;
                    child.localEulerAngles = eulerAngles;
                }
            }
        }
    }
}