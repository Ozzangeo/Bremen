using UnityEngine;

public class CameraController : MonoBehaviour
{
    private GameObject targetPlayer;
    private Camera mainCamera;
    private bool hasTarget = false;
    private float followSpeed = 5f;

    private void Start()
    {
        mainCamera = Camera.main;

        if (mainCamera != null)
        {
            mainCamera.transform.SetParent(transform);
            mainCamera.transform.localPosition = new Vector3(0, 1, -4);
            mainCamera.transform.localRotation = Quaternion.identity;
        }
    }

    private void LateUpdate()
    {
        if (!hasTarget || targetPlayer == null) return;

        Vector3 targetPosition = Vector3.Lerp(transform.position, targetPlayer.transform.position, followSpeed * Time.deltaTime);
        transform.position = targetPosition;

        LookAround();
    }

    public void SetTarget(GameObject target)
    {
        if (!hasTarget)
        {
            targetPlayer = target;
            transform.rotation = target.transform.rotation;
            hasTarget = true;
        }
    }

    private void FollowTarget()
    {
        transform.position = targetPlayer.transform.position;
    }

    private void LookAround()
    {
        Vector2 mouseDelta = new Vector2(Input.GetAxis("Mouse X"), Input.GetAxis("Mouse Y"));
        Vector3 camAngle = transform.rotation.eulerAngles;
        float x = camAngle.x - mouseDelta.y;

        if(x < 180f)
        {
            x = Mathf.Clamp(x, -1f, 70f);
        }
        else
        {
            x = Mathf.Clamp(x, 335f, 361f);
        }

        float y = camAngle.y + mouseDelta.x;


        transform.rotation = Quaternion.Euler(x, y, camAngle.z);
    }
}
