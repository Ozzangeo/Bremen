using UnityEngine;

public class CameraDirectionChecker : MonoBehaviour
{
    /// <summary>
    /// ���� ī�޶� ���� �ִ� ������ ��ȯ
    /// </summary>
    public Vector3 GetNearestDirection()
    {
        return GetDirection(Vector3.forward);
    }

    /// <summary>
    /// ���� ī�޶� �������� ���� ������ ��ȯ
    /// </summary>
    public Vector3 GetLeftDirection()
    {
        return GetDirection(Vector3.left);
    }

    /// <summary>
    /// ���� ī�޶� �������� ������ ������ ��ȯ
    /// </summary>
    public Vector3 GetRightDirection()
    {
        return GetDirection(Vector3.right);
    }

    /// <summary>
    /// ���� ī�޶� �������� ���� ������ ��ȯ
    /// </summary>
    public Vector3 GetBackDirection()
    {
        return GetDirection(Vector3.back);
    }

    /// <summary>
    /// ���� ����(��/��/��/��) �� ���� ����� ������ ��ȯ
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
