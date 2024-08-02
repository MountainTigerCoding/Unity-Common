using UnityEngine;

namespace Runtime.Shared
{
    /// <summary>
    /// Represents a transforms Forward, up and right vectors
    /// </summary>
    [System.Serializable]
    public readonly struct Orientation
    {
    #region Fields
        public readonly Vector3 Forward;
        public readonly Vector3 Up;
        public readonly Vector3 Right;
    #endregion

        public Orientation (Transform transform)
        {
            Forward = transform.forward;
            Up = transform.up;
            Right = transform.right;
        }

        public Orientation (Vector3 forward, Vector3 up, Vector3 right)
        {
            Forward = forward;
            Up = up;
            Right = right;
        }

        public Orientation (Vector3 forward, Vector3 up)
        {
            // Normalization could be simplified
            Forward = forward.normalized;
            Up = up.normalized;
            Right = Vector3.Cross(Forward, Up).normalized;
        }

#if UNITY_EDITOR
        public readonly void DrawGizmos (Vector3 position, float length = 2)
        {
            Gizmos.color = Color.blue;
            Gizmos.DrawRay(position, Forward * length);

            Gizmos.color = Color.green;
            Gizmos.DrawRay(position, Up * length);

            Gizmos.color = Color.red;
            Gizmos.DrawRay(position, Right * length);
        }

        public readonly void DrawDebug (Vector3 position, float duration, float length = 2)
        {
            Debug.DrawRay(position, Forward * length, Color.blue, duration);
            Debug.DrawRay(position, Up * length, Color.green, duration);
            Debug.DrawRay(position, Right * length, Color.red, duration);
        }
#endif
    }
}