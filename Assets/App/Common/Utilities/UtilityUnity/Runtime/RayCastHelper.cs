using UnityEngine;

namespace App.Common.Utilities.UtilityUnity.Runtime
{
    public class RayCastHelper
    {
        private static readonly RaycastHit[] _hits = new RaycastHit[64];
        private static readonly Collider[] _colliders = new Collider[64];

        public static int SphereCastNonAlloc(
            Ray ray,
            float radius,
            out RaycastHit[] results,
            float maxDistance,
            int layerMask)
        {
            var count = Physics.SphereCastNonAlloc(ray, radius, _hits, maxDistance, layerMask,
                QueryTriggerInteraction.UseGlobal);
            results = _hits;
            return count;
        }

        public static int OverlapSphereNonAlloc(Vector3 position, float radius, out Collider[] results, int layerMask)
        {
            int count = Physics.OverlapSphereNonAlloc(position, radius, _colliders, layerMask);
            results = _colliders;
            return count;
        }

        public static bool RaycastNonAllocSingle(
            Ray ray,
            out RaycastHit hit,
            float maxDistance = Mathf.Infinity,
            int layerMask = Physics.DefaultRaycastLayers,
            QueryTriggerInteraction queryTriggerInteraction = QueryTriggerInteraction.UseGlobal)
        {
            int hitCount =
                Physics.RaycastNonAlloc(ray, _hits, maxDistance, layerMask, queryTriggerInteraction);
            if (hitCount > 0)
            {
                int closest = -1;
                for (int i = 0; i < hitCount; ++i)
                {
                    if (closest == -1 || _hits[i].distance < _hits[closest].distance)
                    {
                        closest = i;
                    }
                }

                if (closest == -1)
                {
                    hit = new RaycastHit();
                    return false;
                }

                hit = _hits[closest];
                return true;
            }
            else
            {
                hit = new RaycastHit();
                return false;
            }
        }

        public static int OverlapBoxNonAlloc(
            Vector3 center,
            float side,
            out Collider[] results,
            int layerMask,
            Quaternion orientation = default,
            QueryTriggerInteraction queryTriggerInteraction = QueryTriggerInteraction.UseGlobal)
        {
            if (orientation == default)
            {
                orientation = Quaternion.identity;
            }

            Vector3 halfExtents = new Vector3(side, side, side) * 0.5f;
            int count = Physics.OverlapBoxNonAlloc(center, halfExtents, _colliders, orientation, layerMask,
                queryTriggerInteraction);
            results = _colliders;
            return count;
        }

        public static int OverlapBoxNonAlloc(
            Vector3 center,
            Vector3 size,
            out Collider[] results,
            int layerMask,
            Quaternion orientation = default,
            QueryTriggerInteraction queryTriggerInteraction = QueryTriggerInteraction.UseGlobal)
        {
            if (orientation == default)
            {
                orientation = Quaternion.identity;
            }

            Vector3 halfExtents = size * 0.5f;
            int count = Physics.OverlapBoxNonAlloc(center, halfExtents, _colliders, orientation, layerMask,
                queryTriggerInteraction);
            results = _colliders;
            return count;
        }
    }
}