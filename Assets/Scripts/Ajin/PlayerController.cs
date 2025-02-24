using ExitGames.Client.Photon.StructWrapping;
using Fusion;
using UnityEngine;
using UnityEngine.EventSystems;


public class PlayerController : NetworkBehaviour
{
    private NetworkCharacterController _characterController;
    private CameraController cameraController;
    private bool _canMove = true;
    private float moveSpeed = 5f;
    private float jumpHeigh = 5f;

    private void Awake()
    {
        _characterController = GetComponent<NetworkCharacterController>();
    }

    private void Start()
    {
        if(Object.HasInputAuthority)
        {
            if(cameraController == null)
            {
                cameraController = GameObject.Find("CameraController").GetComponent<CameraController>();
            }
            cameraController.SetTarget(gameObject);
        }
    }

    public override void FixedUpdateNetwork()
    {
        if(!_canMove) return;

        if (GetInput(out NetworkInputData data))
        {
            data.direction.Normalize();
            _characterController.Move(data.direction * moveSpeed * Runner.DeltaTime);

            // 점프
            if (data.isJumping)
            {
                Debug.Log("점프");
                Jump();
            }

            if(data.isPlaying)
            {
                Debug.Log("연주");
            }
        }
    }

    public void EnableMovement(bool enable)
    {
        _canMove = enable;
    }

    private void Jump()
    {
        _characterController.Move(new Vector3( 0, jumpHeigh, 0) * Runner.DeltaTime);
    }
}