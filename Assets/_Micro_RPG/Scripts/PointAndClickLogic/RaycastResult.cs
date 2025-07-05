using UnityEngine;

public struct RaycastResult
{
    public bool HasHit { get; set; }
    public Vector3 HitPoint { get; set; }
    public bool IsGround { get; set; }
    public IInteractionTarget[] Targets { get; set; }
}
