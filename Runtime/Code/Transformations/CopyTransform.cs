using System;
using UnityEngine;

namespace Runtime.Shared
{
    [ExecuteInEditMode()]
    [AddComponentMenu("Shared/Copy Transform")]
    public sealed class CopyTransform : MonoBehaviour
    {
        [Flags]
        public enum TransformComponents
        {
            Position,
            Rotation,
            Scale,
            All
        }

        public Transform CopyObject;
        public TransformComponents Components = TransformComponents.All;
        public Space CopySpace = Space.Self;
        public Axis EffectAxis;

        [Space(10f)]
        public Vector3 PositionOffset = Vector3.zero;

        private void Update ()
        {
            OnUpdate();
        }

        public void OnUpdate ()
        {
            if (CopyObject == null) return;

            if (Components == TransformComponents.Position || Components == TransformComponents.All) {
                if (CopySpace == Space.World) transform.position = CopyObject.position + PositionOffset;
                else transform.localPosition = CopyObject.localPosition + PositionOffset;
            }

            if (Components == TransformComponents.Rotation || Components == TransformComponents.All) {
                if (CopySpace == Space.World) transform.rotation = CopyObject.rotation;
                else transform.localRotation = CopyObject.localRotation;
            }

            if (Components == TransformComponents.Scale || Components == TransformComponents.All) {
                transform.localScale = CopyObject.localScale;
            }
        }
    }
}