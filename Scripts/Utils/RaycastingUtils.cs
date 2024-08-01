using System;
using System.Collections.Generic;
using UnityEngine;

namespace Runtime.Shared
{
    public enum RaycastMethod
    {
        [Tooltip("Uses normal Physics.Raycast which creates garbage for the GC")] UseGC = 0,
        [Tooltip("Does not create any garbage for the GC but is not as accurate")] NonAlloc = 1,
    }

    public static class RaycastingUtils
    {
    #region Direction Generation
        public static Vector3[] GenerateConeCastDirections (int emissionAngleX, int emissionAngleY, int resolution)
        {
            resolution = Math.Clamp(resolution, 1, 20);
            List<Vector3> directions = new();

            bool breakAll = false;
            for (int angleX = -(emissionAngleX / 2); angleX < (emissionAngleX / 2); angleX += resolution)
            {
                for (int angleY = -(emissionAngleY / 2); angleY < (emissionAngleY / 2); angleY += resolution)
                {
                    float dirYSin = Mathf.Sin(angleY * Mathf.Deg2Rad);
                    float dirYCos = Mathf.Cos(angleY * Mathf.Deg2Rad);

                    float dirX = Mathf.Sin(angleX * Mathf.Deg2Rad) * dirYCos;
                    float dirZ = Mathf.Cos(angleX * Mathf.Deg2Rad) * dirYCos;
                    directions.Add(new Vector3(dirX, dirYSin, dirZ));

                    if (directions.Count > 10000) {
                        breakAll = true;
                        break;
                    }
                }

                if (breakAll) break;
            }
            return directions.ToArray();
        }
    #endregion Direction Generation

    #region Composite Raycasts
        public static bool RaycastBlur (RaycastMethod method, int bufferSize, Vector3 origin, Orientation orientation, float blurSize, float range, LayerMask mask, out float distance, bool debugRays = false)
        {
            if (Mathf.Approximately(blurSize, 0)) {
                if (Raycast(method, bufferSize, new(origin, orientation.Forward), range, mask, out RaycastHit result, debugRays)) {
                    distance = result.distance;
                    return true;
                } else {
                    distance = 0;
                    return false;
                }
            }

#if UNITY_EDITOR
            orientation.DrawDebug(origin, 0, 2);
#endif
            return RaycastAverageDistance
            (
                method, bufferSize, range, mask,
                new Ray[]
                {
                    new(origin, orientation.Forward),
                    new(Utils.TranslateVector(origin, -orientation.Right, blurSize), orientation.Forward),
                    new(Utils.TranslateVector(origin, orientation.Right, blurSize), orientation.Forward),
                    new(Utils.TranslateVector(origin, orientation.Up, blurSize), orientation.Forward),
                    new(Utils.TranslateVector(origin, -orientation.Up, blurSize), orientation.Forward)
                },
                orientation.Forward, out distance, debugRays
            );
        }

        public static bool RaycastAverageDistance (RaycastMethod method, int bufferSize, float range, LayerMask mask, Ray[] rays, Vector3 eulerAngles, out float distance, bool debugRays = false)
        {
            if (rays.Length == 0) {
                distance = 0;
                return false;
            }

            int numHit = 0;
            distance = 0;
            for (int i = 0; i < rays.Length; i++)
            {
                Ray ray = rays[i];
                ray.direction = Quaternion.Euler(eulerAngles) * ray.direction;
    
                if (Raycast(method, bufferSize, ray , range, mask, out RaycastHit closestHit, debugRays)) {
                    numHit++;
                    distance += closestHit.distance;
                }
            }

            if (numHit == 0) return false;
            distance /= numHit;
            return true;
        }

        public static bool RaycastMinDistance (RaycastMethod method, int bufferSize, float range, LayerMask mask, Vector3 origin, Vector3[] directions, Vector3 eulerAngles, out float distance, bool debugRays = false)
        {
            List<Ray> rays = new();
            foreach (Vector3 direction in directions) rays.Add(new(origin, Quaternion.Euler(eulerAngles) * direction));
            return RaycastMinDistance(method, bufferSize, range, mask, rays.ToArray(), out distance, debugRays);
        }

        public static bool RaycastMinDistance (RaycastMethod method, int bufferSize, float range, LayerMask mask, Ray[] rays, out float distance, bool debugRays = false)
        {
            if (rays.Length == 0) {
                distance = 0;
                return false;
            }

            distance = 9999999;
            foreach (Ray ray in rays)
            {
                if (Raycast(method, bufferSize, ray, range, mask, out RaycastHit hit, debugRays)) {
                    distance = Mathf.Min(distance, hit.distance);
                    //if (hit.distance < distance) distance = hit.distance;
                }
            }

            if (Mathf.Approximately(distance, 9999999)) distance = 0;
            return distance > 0;
        }
    #endregion Composite Raycasts

    #region Raycasting Methods
        public static bool Raycast (RaycastMethod method, int bufferSize, Ray ray, float range, LayerMask mask, out RaycastHit hit, bool debugRay = false)
        {
            bool didHit = method switch
            {
                RaycastMethod.NonAlloc => RaycastNonAllocClosestDistance(bufferSize, ray, range, mask, out hit),
                _ => Physics.Raycast(ray, out hit, range, mask)
            };

#if UNITY_EDITOR
            if (didHit && debugRay) {
                Debug.DrawLine(ray.origin, hit.point, Color.yellow);
            }
#endif
            return didHit;
        }

        [Obsolete("Use method 'RaycastNonAllocClosestDistance' with ray argument instead. Due to 'origin' and 'direction' being vector3s, they could be mixed up")]
        public static bool RaycastNonAllocClosestDistance (int bufferSize, Vector3 origin, Vector3 direction, float range, LayerMask mask, out RaycastHit closestHit)
        {
            return RaycastNonAllocClosestDistance(bufferSize, new Ray(origin, direction), range, mask, out closestHit);
        }

        public static bool RaycastNonAllocClosestDistance (int bufferSize, Ray ray, float range, LayerMask mask, out RaycastHit closestHit)
        {
            RaycastHit[] hitResults = new RaycastHit[bufferSize];
            int numHits = Physics.RaycastNonAlloc(ray, hitResults, range, mask);
            float closestDist = Mathf.Infinity; // Get the Closest Hit
            int filteredHits = 0; //The Number of Valid Hits

            for (int i = 0; i < numHits; i++)
            {
                if (hitResults[i].distance < closestDist)
                {
                    filteredHits++;
                    closestDist = hitResults[i].distance;
                }
            }

            // Assign the closest hit
            closestHit = filteredHits > 0 ? hitResults[filteredHits - 1] : hitResults[0];
            return filteredHits > 0;
        }
    #endregion Raycasting Methods
    }
}