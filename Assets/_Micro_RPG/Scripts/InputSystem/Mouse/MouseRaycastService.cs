//using System;
//using System.Collections.Generic;
//using UnityEngine;

//public sealed class MouseRaycastService : IMouseRaycastService
//{
//    private class RaycastHitComparer : IComparer<RaycastHit>
//    {
//        public int Compare(RaycastHit x, RaycastHit y)
//        {
//            int distanceComparison = x.distance.CompareTo(y.distance);
//            if (distanceComparison != 0) return distanceComparison;

//            var xClickable = x.collider?.GetComponent<IClickable>();
//            var yClickable = y.collider?.GetComponent<IClickable>();

//            if (xClickable != null && yClickable != null)
//                return yClickable.Priority.CompareTo(xClickable.Priority);

//            return 0;
//        }
//    }

//    private const float MaxRaycastDistance = 100f;
//    private readonly LayerMask _clickableLayers;
//    private readonly LayerMask _groundLayer;
//    private readonly RaycastHit[] _raycastHits = new RaycastHit[10];
//    private readonly RaycastHitComparer _hitComparer = new RaycastHitComparer();

//    public MouseRaycastService(LayerMask clickableLayers, LayerMask groundLayer)
//    {
//        _clickableLayers = clickableLayers;
//        _groundLayer = groundLayer;
//    }

//    public MouseRaycastResult PerformRaycast(Vector3 screenPosition, Camera camera)
//    {
//        var ray = camera.ScreenPointToRay(screenPosition);

//        // Check clickables first
//        int hitCount = Physics.RaycastNonAlloc(ray, _raycastHits, MaxRaycastDistance, _clickableLayers);

//        if (hitCount > 0)
//        {
//            Array.Sort(_raycastHits, 0, hitCount, _hitComparer);

//            for (int i = 0; i < hitCount; i++)
//            {
//                var clickable = _raycastHits[i].collider.GetComponent<IClickable>();
//                if (clickable != null)
//                {
//                    return new MouseRaycastResult(true, _raycastHits[i].point, clickable, false);
//                }
//            }
//        }

//        // Check ground
//        if (Physics.Raycast(ray, out RaycastHit groundHit, MaxRaycastDistance, _groundLayer))
//        {
//            return new MouseRaycastResult(true, groundHit.point, null, true);
//        }

//        return new MouseRaycastResult(false, Vector3.zero, null, false);
//    }
//}

using UnityEngine;

public class MouseRaycastService : IMouseRaycastService
{
    private readonly LayerMask _interactableLayers;
    private readonly LayerMask _groundLayer;
    private readonly RaycastHit[] _hits = new RaycastHit[10];

    public MouseRaycastService(LayerMask interactableLayers, LayerMask groundLayer)
    {
        _interactableLayers = interactableLayers;
        _groundLayer = groundLayer;
    }

    public RaycastResult PerformRaycast(Vector2 screenPosition, Camera camera)
    {
        var ray = camera.ScreenPointToRay(screenPosition);
        var result = new RaycastResult();

        // Check for interactables
        int hitCount = Physics.RaycastNonAlloc(ray, _hits, 100f, _interactableLayers);

        if (hitCount > 0)
        {
            var targets = new IInteractionTarget[hitCount];
            int validTargets = 0;

            for (int i = 0; i < hitCount; i++)
            {
                var target = _hits[i].collider.GetComponent<IInteractionTarget>();
                if (target != null && target.CanInteract)
                {
                    targets[validTargets++] = target;
                }
            }

            if (validTargets > 0)
            {
                System.Array.Resize(ref targets, validTargets);
                System.Array.Sort(targets, (a, b) => b.Priority.CompareTo(a.Priority));

                result.HasHit = true;
                result.HitPoint = _hits[0].point;
                result.Targets = targets;
                return result;
            }
        }

        // Check for ground
        if (Physics.Raycast(ray, out RaycastHit groundHit, 100f, _groundLayer))
        {
            result.HasHit = true;
            result.IsGround = true;
            result.HitPoint = groundHit.point;
            result.Targets = System.Array.Empty<IInteractionTarget>();
        }

        return result;
    }
}
