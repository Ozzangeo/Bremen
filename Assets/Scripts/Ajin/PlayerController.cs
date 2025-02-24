using ExitGames.Client.Photon.StructWrapping;
using Fusion;
using UnityEngine;
using UnityEngine.EventSystems;


public class PlayerController : NetworkBehaviour
{
    [SerializeField] private GameObject characterBody;
    private NetworkCharacterController _characterController;
    private CameraController _cameraController;
    private GameObject cameraObject;
    private bool _canMove = true;
    private float moveSpeed = 5f;
    private float rotationSpeed = 1f;

    private void Awake()
    {
        _characterController = GetComponent<NetworkCharacterController>();
    }

    private void Start()
    {
        cameraObject = GameObject.Find("CameraController");
        _cameraController = GameObject.Find("CameraController").GetComponent<CameraController>();

        _cameraController.SetTarget(characterBody);
    }

    public override void FixedUpdateNetwork()
    {
        if(!_canMove)
        {
            return;
        }
        if (!Object.HasStateAuthority) return;

        if (GetInput(out NetworkInputData data))
        {
            Vector2 moveInput = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
            Vector3 lookForward = new Vector3(cameraObject.transform.forward.x, 0f, cameraObject.transform.forward.z).normalized;
            Vector3 lookRight = new Vector3(cameraObject.transform.right.x, 0f, cameraObject.transform.right.z).normalized;
            Vector3 moveDirection = lookForward * moveInput.y + lookRight * moveInput.x;
            Debug.DrawRay(cameraObject.transform.position, new Vector3(cameraObject.transform.forward.x, 0f, cameraObject.transform.forward.z).normalized, Color.red);
            characterBody.transform.forward = moveDirection;
            characterBody.transform.forward = moveDirection;
            moveDirection = moveDirection.normalized;
            //characterbody.transform.forward = moveDirection;
            _characterController.Move(moveDirection * moveSpeed * Runner.DeltaTime);

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