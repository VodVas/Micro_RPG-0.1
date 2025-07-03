using UnityEngine;

public sealed class MouseTargetingSystem : IMouseTargetingSystem
{
    private readonly float _stoppingDistance;
    private Vector3? _targetPosition;
    private IClickable _currentTarget;

    public Vector3? TargetPosition => _targetPosition;
    public IClickable CurrentTarget => _currentTarget;
    public bool HasValidTarget => _targetPosition.HasValue;

    public MouseTargetingSystem(float stoppingDistance)
    {
        _stoppingDistance = stoppingDistance;
    }

    public void SetTarget(Vector3 position, IClickable clickable = null)
    {
        _targetPosition = position;
        _currentTarget = clickable;
    }

    public void ClearTarget()
    {
        _targetPosition = null;
        _currentTarget = null;
    }

    public Vector2 GetMovementDirection(Vector3 currentPosition, Camera camera)
    {
        if (ShouldStopMoving(currentPosition))
            return Vector2.zero;

        return CalculateDirection(currentPosition, camera);
    }

    //public Vector2 GetMovementDirection(Vector3 currentPosition, Camera camera)
    //{
    //    if (!_targetPosition.HasValue)
    //        return Vector2.zero;

    //    var directionToTarget = _targetPosition.Value - currentPosition;
    //    directionToTarget.y = 0;

    //    if (directionToTarget.magnitude <= _stoppingDistance)
    //    {
    //        if (_currentTarget == null)
    //            ClearTarget();
    //        return Vector2.zero;
    //    }

    //    var cameraForward = camera.transform.forward;
    //    var cameraRight = camera.transform.right;
    //    cameraForward.y = 0;
    //    cameraRight.y = 0;
    //    cameraForward.Normalize();
    //    cameraRight.Normalize();

    //    var direction = directionToTarget.normalized;
    //    return new Vector2(
    //        Vector3.Dot(direction, cameraRight),
    //        Vector3.Dot(direction, cameraForward)
    //    ).normalized;
    //}

    private bool ShouldStopMoving(Vector3 currentPosition)
    {
        if (!_targetPosition.HasValue)
            return true;

        var directionToTarget = _targetPosition.Value - currentPosition;
        directionToTarget.y = 0;

        bool isTargetReached = directionToTarget.magnitude <= _stoppingDistance;

        if (isTargetReached && _currentTarget == null)
            ClearTarget();

        return isTargetReached;
    }

    private Vector2 CalculateDirection(Vector3 currentPosition, Camera camera)
    {
        var directionToTarget = (_targetPosition.Value - currentPosition).normalized;
        directionToTarget.y = 0;

        var cameraForward = camera.transform.forward;
        var cameraRight = camera.transform.right;
        cameraForward.y = 0;
        cameraRight.y = 0;
        cameraForward.Normalize();
        cameraRight.Normalize();

        return new Vector2(
            Vector3.Dot(directionToTarget, cameraRight),
            Vector3.Dot(directionToTarget, cameraForward)
        ).normalized;
    }
}