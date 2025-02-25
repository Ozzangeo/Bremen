using UnityEngine;

public class CameraDirectionChecker : MonoBehaviour
{
    /// <summary>
    /// 현재 카메라가 보고 있는 방향을 반환
    /// </summary>
    public Vector3 GetNearestDirection()
    {
        return GetDirection(Vector3.forward);
    }

    /// <summary>
    /// 현재 카메라 기준으로 왼쪽 방향을 반환
    /// </summary>
    public Vector3 GetLeftDirection()
    {
        return GetDirection(Vector3.left);
    }

    /// <summary>
    /// 현재 카메라 기준으로 오른쪽 방향을 반환
    /// </summary>
    public Vector3 GetRightDirection()
    {
        return GetDirection(Vector3.right);
    }

    /// <summary>
    /// 현재 카메라 기준으로 뒤쪽 방향을 반환
    /// </summary>
    public Vector3 GetBackDirection()
    {
        return GetDirection(Vector3.back);
    }

    /// <summary>
    /// 기준 방향(전/후/좌/우) 중 가장 가까운 방향을 반환
    /// </summary>
    private Vector3 GetDirection(Vector3 referenceDirection)
    {
        Vector3 forward = new Vector3(transform.forward.x, 0, transform.forward.z).normalized;
        Vector3 adjustedDirection = transform.rotation * referenceDirection;
        Vector3[] directions = { Vector3.forward, Vector3.back, Vector3.left, Vector3.right };
        Vector3 nearestDirection = Vector3.zero;
        float maxDot = -Mathf.Infinity;

        foreach (Vector3 dir in directions)
        {
            float dot = Vector3.Dot(adjustedDirection, dir);
            if (dot > maxDot)
            {
                maxDot = dot;
                nearestDirection = dir;
            }
        }

        return nearestDirection;
    }
}
