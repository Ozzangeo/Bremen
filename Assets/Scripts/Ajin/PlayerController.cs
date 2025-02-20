using Fusion;
using UnityEngine;

public class PlayerController : NetworkBehaviour
{
    
    private float moveSpeed = 1f;

    private NetworkCharacterController _characterController;

    private void Awake()
    {
        _characterController = GetComponent<NetworkCharacterController>();
    }

    public override void FixedUpdateNetwork()
    {
        if (GetInput(out NetworkInputData data))
        {
            // 이동
            Vector3 moveDirection = data.direction.normalized * moveSpeed * Runner.DeltaTime;
            _characterController.Move(moveDirection);

            // 점프
            if (data.isJumping)
            {
                Debug.Log("[DEBUG] 점프!");
                Jump();
            }
        }
    }

    private void Jump()
    {

    }
}