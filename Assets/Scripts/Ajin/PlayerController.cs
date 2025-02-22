using Fusion;
using UnityEngine;

public class PlayerController : NetworkBehaviour
{
    public CharacterData characterData;
    private NetworkCharacterController _characterController;
    private bool _canMove = false;

    private void Awake()
    {
        _characterController = GetComponent<NetworkCharacterController>();
    }

    public override void FixedUpdateNetwork()
    {
        if(!_canMove)
        {
            return;
        }

        if (GetInput(out NetworkInputData data))
        {
            // 이동
            Vector3 moveDirection = data.direction.normalized * characterData.moveSpeed * Runner.DeltaTime;
            _characterController.Move(moveDirection);

            // 점프
            if (data.isJumping)
            {
                Debug.Log("[DEBUG] 점프!");
                Jump();
            }
        }
    }

    public void EnableMovement(bool enable)
    {
        _canMove = enable;
    }

    private void Jump()
    {

    }
}