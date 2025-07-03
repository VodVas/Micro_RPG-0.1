using UnityEngine;

public readonly struct MouseRaycastResult
{
    public readonly bool HasHit;
    public readonly Vector3 HitPoint;
    public readonly IClickable Clickable;
    public readonly bool IsGround;

    public MouseRaycastResult(bool hasHit, Vector3 hitPoint, IClickable clickable, bool isGround)
    {
        HasHit = hasHit;
        HitPoint = hitPoint;
        Clickable = clickable;
        IsGround = isGround;
    }
}