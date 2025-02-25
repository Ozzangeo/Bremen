using Fusion;
using UnityEngine;

public struct NetworkInputData : INetworkInput
{
    // GameScene
    public Vector3 direction; // 이동 방향
    public Vector2 moveInput;
    public bool isJumping; // 점프 여부
    public bool isDash; // 연주
}
