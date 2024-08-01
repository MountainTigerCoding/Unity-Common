using System;
using UnityEngine;

namespace Runtime.Common
{
    public enum EasingType : int
    {
        Lerp          = 0,
        InSine        = 1,
        OutSine       = 2,
        InOutSine     = 3,
        InQuad        = 4,
        OutQuad       = 5,
        InOutQuad     = 6,
        InCubic       = 7,
        OutCubic      = 8,
        InOutCubic    = 9,
        InQuart       = 10,
        OutQuart      = 11, 
        InOutQuart    = 12,
        InQuint       = 13,
        OutQuint      = 14,
        InOutQuint    = 15,
        InExpo        = 16,
        OutExpo       = 17,
        InOutExpo     = 18, 
        InCirc        = 19,
        OutCirc       = 20,
        InOutCirc     = 21,
        InBack        = 22,
        OutBack       = 23,
        InOutBack     = 24,
        InElastic     = 25,
        OutElastic    = 26,
        InOutElastic  = 27,
        InBounce      = 28,
        OutBounce     = 29,
        InOutBounce   = 30,
    }

    [Obsolete("Use LeanTween instead")]
    public static class EasingFunctions
    {
    #region Enum Based
        public static float Ease (AnimationCurve curve, float t, EasingType type)
        {
            return Ease(curve.Evaluate(t), type);
        }

        public static float Ease (float t, EasingType type)
        {
            t = Mathf.Clamp01(t);
            switch (type)
            {
                case EasingType.Lerp:
                    return t;

                case EasingType.InSine:
                    return InSine(t);

                case EasingType.OutSine:
                    return OutSine(t);

                case EasingType.InOutSine:
                    return InOutSine(t);

                case EasingType.InQuad:
                    return InQuad(t);

                case EasingType.OutQuad:
                    return OutQuad(t);

                case EasingType.InOutQuad:
                    return InOutQuad(t);

                case EasingType.InCubic:
                    return InCubic(t);

                case EasingType.OutCubic:
                    return OutCubic(t);

                case EasingType.InOutCubic:
                    return InOutCubic(t);

                case EasingType.InQuart:
                    return InQuart(t);

                case EasingType.OutQuart:
                    return OutQuart(t);

                case EasingType.InOutQuart:
                    return InOutQuart(t);

                case EasingType.InQuint:
                    return InQuint(t);

                case EasingType.OutQuint:
                    return OutQuint(t);

                case EasingType.InOutQuint:
                    return InOutQuint(t);

                case EasingType.InExpo:
                    return InExpo(t);

                case EasingType.OutExpo:
                    return OutExpo(t);

                case EasingType.InOutExpo:
                    return InOutExpo(t);

                case EasingType.InCirc:
                    return InCirc(t);

                case EasingType.OutCirc:
                    return OutCirc(t);

                case EasingType.InOutCirc:
                    return InOutCirc(t);

                case EasingType.InBack:
                    return InBack(t);

                case EasingType.OutBack:
                    return OutBack(t);

                case EasingType.InOutBack:
                    return InOutBack(t);

                case EasingType.InElastic:
                    return InElastic(t);

                case EasingType.OutElastic:
                    return OutElastic(t);

                case EasingType.InOutElastic:
                    return InOutElastic(t);

                case EasingType.InBounce:
                    return InBounce(t);

                case EasingType.OutBounce:
                    return OutBounce(t);

                case EasingType.InOutBounce:
                    return InOutBounce(t);

                default:
                    Debug.Log("Easing function '" + type.ToString() + "' has not been implemented in library!");
                    return -1;
            }
        }
    #endregion Enum Based

    #region Specific
        public static float InSine (float t)
        {
            return 1 - Mathf.Cos(t * Mathf.PI / 2);
        }

        public static float OutSine (float t)
        {
            return Mathf.Sin(t * Mathf.PI / 2);
        }

        public static float InOutSine (float t)
        {
            return -(Mathf.Cos(Mathf.PI * t) - 1) / 2;
        }

        public static float InQuad (float t)
        {
           return t * t;
        }

        public static float OutQuad (float t)
        {
           return 1 - (1 - t) * (1 - t);
        }

        public static float InOutQuad (float t)
        {
           return t < 0.5f ? 2 * t * t : 1 - Mathf.Pow(-2 * t + 2, 2) / 2;
        }

        public static float InCubic (float t)
        {
           return t * t * t;
        }

        public static float OutCubic (float t)
        {
           return 1 - Mathf.Pow(1 - t, 3);
        }
        
        public static float InOutCubic (float t)
        {
           return t < 0.5f ? 4 * t * t * t : 1 - Mathf.Pow(-2 * t + 2, 3) / 2;
        }

        public static float InQuart (float t)
        {
           return t * t * t * t;
        }

        public static float OutQuart (float t)
        {
           return 1 - Mathf.Pow(1f - t, 4);
        }

        public static float InOutQuart (float t)
        {
           return t < 0.5f ? 8 * t * t * t * t : 1 - Mathf.Pow(-2 * t + 2, 4) / 2;
        }

        public static float InQuint (float t)
        {
           return t * t * t * t * t;
        }

        public static float OutQuint (float t)
        {
           return 1 - Mathf.Pow(1 - t, 5);
        }

        public static float InOutQuint (float t)
        {
           return t < 0.5f ? 16 * t * t * t * t * t : 1 - Mathf.Pow(-2 * t + 2, 5) / 2;
        }

        public static float InExpo (float t)
        {
           return t == 0 ? 0 : Mathf.Pow(2, 10 * t - 10);
        }

        public static float OutExpo (float t)
        {
            return t == 1 ? 1 : 1 - Mathf.Pow(2, -10 * t);
        }

        public static float InOutExpo (float t)
        {
            return t == 0
            ? 0
            : t == 1
            ? 1
            : t < 0.5f ? Mathf.Pow(2, 20 * t - 10) / 2
            : (2 - Mathf.Pow(2, -20 * t + 10)) / 2;
        }

        public static float InCirc (float t)
        {
            return 1 - Mathf.Sqrt(1 - Mathf.Pow(t, 2));
        }

        public static float OutCirc (float t)
        {
            return Mathf.Sqrt(1 - Mathf.Pow(t - 1, 2));
        }

        public static float InOutCirc (float t)
        {
            return t < 0.5f
            ? (1 - Mathf.Sqrt(1 - Mathf.Pow(2 * t, 2))) / 2
            : (Mathf.Sqrt(1 - Mathf.Pow(-2 * t + 2, 2)) + 1) / 2;
        }

        public static float InBack (float t)
        {
            float c1 = 1.70158f;
            float c3 = c1 + 1;

            return c3 * t * t * t - c1 * t * t;
        }

        public static float OutBack (float t)
        {
            float c1 = 1.70158f;
            float c3 = c1 + 1;
            return 1 + c3 * Mathf.Pow(t - 1, 3) + c1 * Mathf.Pow(t - 1, 2);
        }

        public static float InOutBack (float t)
        {
            float c1 = 1.70158f;
            float c2 = c1 * 1.525f;
            return t < 0.5f
            ? Mathf.Pow(2 * t, 2) * ((c2 + 1) * 2 * t - c2) / 2
            : (Mathf.Pow(2 * t - 2, 2) * ((c2 + 1) * (t * 2 - 2) + c2) + 2) / 2;
        }

        public static float InElastic (float t)
        {
            float c4 = 2 * Mathf.PI / 3;
            return t == 0
            ? 0
            : t == 1
            ? 1
            : -Mathf.Pow(2, 10 * t - 10) * Mathf.Sin((t * 10 - 10.75f) * c4);
        }

        public static float OutElastic (float t)
        {
            float c4 = 2 * Mathf.PI / 3;
            return t == 0
            ? 0
            : t == 1
            ? 1
            : Mathf.Pow(2, -10 * t) * Mathf.Sin((t * 10 - 0.75f) * c4) + 1;
        }

        public static float InOutElastic (float t)
        {
            float c5 = 2 * Mathf.PI / 4.5f;
            return t == 0
            ? 0 : t == 1
            ? 1 : t < 0.5f
            ? -(Mathf.Pow(2, 20 * t - 10) * Mathf.Sin((20 * t - 11.125f) * c5)) / 2
            : Mathf.Pow(2, -20 * t + 10) * Mathf.Sin((20 * t - 11.125f) * c5) / 2 + 1;
        }

        public static float InBounce (float t)
        {
            return 1 - OutBounce(1 - t);
        }

        public static float OutBounce (float t)
        {
            float n1 = 7.5625f;
            float d1 = 2.75f;

            if (t < 1 / d1) {
                return n1 * t * t;
            } else if (t < 2 / d1) {
                return n1 * (t -= 1.5f / d1) * t + 0.75f;
            } else if (t < 2.5f / d1) {
                return n1 * (t -= 2.25f / d1) * t + 0.9375f;
            } else {
                return n1 * (t -= 2.625f / d1) * t + 0.984375f;
            }
        }

        public static float InOutBounce (float t)
        {
            return t < 0.5f
            ? (1 - OutBounce(1 - 2 * t)) / 2
            : (1 + OutBounce(2 * t - 1)) / 2;
        }
    #endregion Specific
    }
}