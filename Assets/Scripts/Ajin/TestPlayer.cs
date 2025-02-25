using Fusion;
using Ozi.ChartPlayer;
using Unity.VisualScripting;
using UnityEngine;

public class TestPlayer : NetworkBehaviour
{
    private NetworkCharacterController _cc;
    private CameraController cameraController;
    private float moveSpeed = 5f;
    private float rotationSpeed = 5f;
    private float jumpHeigh = 5f;

    private void Awake()
    {
        _cc = GetComponent<NetworkCharacterController>();
    }

    private void Start()
    {
        if (Object.HasInputAuthority)
        {
            if (cameraController == null)
            {
                cameraController = GameObject.Find("CameraController").GetComponent<CameraController>();
            }
            cameraController.SetTarget(gameObject);
        }
    }

    public override void FixedUpdateNetwork()
    {
        if (GetInput(out NetworkInputData data))
        {
            data.direction.Normalize();
            Vector3 moveDir = new Vector3(data.direction.x, 0, data.direction.z) * moveSpeed * Runner.DeltaTime;
            _cc.Move(moveDir);
            // 방향조절

            if (data.isJumping)
            {
                Debug.Log("점프");
                Jump();
            }

            if (data.isDash)
            {
                Dash(data.direction);
            }
        }
    }

    private void Jump()
    {
        _cc.Jump();
    }

    private void Dash(Vector3 direction)
    {
        float time = 0;
        while (time < 1.5f)
        {
            _cc.Move(direction * moveSpeed * 2 * Runner.DeltaTime);
            time += Time.deltaTime;
        }
    }
}
