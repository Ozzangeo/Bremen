using Fusion;
using UnityEngine;

public class TestPlayer : NetworkBehaviour
{
    private NetworkCharacterController _cc;
    private CameraController cameraController;

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
            _cc.Move(5 * data.direction * Runner.DeltaTime);
        }
    }
}
