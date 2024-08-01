using UnityEngine;

namespace Runtime.Shared
{
    [System.Serializable]
    public struct TransformPreset
    {
        public string Name;
        
        public Vector3 Position;
        public Vector3 EulerAngles;
        public Vector3 Scale;

        [Space(5f)]
        public LeanTweenClip TweenClip;
        public bool OverrideDefaultClip;

        public readonly Quaternion Rotation {get => Quaternion.Euler(EulerAngles); }

        public TransformPreset (Vector3 position, Vector3 eulerAngles, Vector3 scale)
        {
            Name = "New Preset";

            Position = position;
            EulerAngles = eulerAngles;
            Scale = scale;

            TweenClip = new();
            OverrideDefaultClip = false;
        }

        public TransformPreset (Vector3 position, Quaternion rotation, Vector3 scale)
        {
            Name = "New Preset";

            Position = position;
            EulerAngles = rotation.eulerAngles;
            Scale = scale;

            TweenClip = new();
            OverrideDefaultClip = false;
        }
    }
}