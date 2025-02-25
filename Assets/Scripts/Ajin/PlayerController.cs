using ExitGames.Client.Photon.StructWrapping;
using Fusion;
using Ozi.ChartPlayer;
using UnityEngine;
using UnityEngine.EventSystems;


public class PlayerController : NetworkBehaviour
{
    public CharacterData selectCharacter;
    private NetworkCharacterController _characterController;
    private CameraController cameraController;
    private bool _canMove = true;
    private float moveSpeed = 5f;
    private float jumpHeigh = 5f;

    public float hp;
    public int combo;

    private BremenChartPlayer bremenChartPlayer;

    private void Awake()
    {
        _characterController = GetComponent<NetworkCharacterController>();
    }

    private void Start()
    {
        bremenChartPlayer = GameObject.FindAnyObjectByType<BremenChartPlayer>();
        selectCharacter = PlayerData.Instance.selectedCharacter;
        hp = selectCharacter.maxHP;
        combo = 0;

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
            // 방향조절
            transform.forward = data.moveDirection;

            // 점프
            if (data.isJumping)
            {
                Debug.Log("점프");
                Jump();
            }

            if (data.isDash)
            {
                if (BremenNoteResult.Perfect == bremenChartPlayer.TryProcessNote())
                {
                    Debug.Log("대쉬");
                    Dash(data.direction);
                }
            }
        }
    }

    public void EnableMovement(bool enable)
    {
        _canMove = enable;
    }
    private void Jump()
    {
        _characterController.Jump();
    }

    private void Dash(Vector3 direction)
    {
        float time = 0;
        while (time < 1.5f)
        {
            _characterController.Move(direction * moveSpeed * 2 * Runner.DeltaTime);
            time += Time.deltaTime;
        }
    }
}