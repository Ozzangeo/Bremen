using Fusion;
using System;
using UnityEngine;

public struct NetworkInputData : INetworkInput
{
    // GameScene
    public Vector3 direction; // 이동 방향
    public Vector2 moveInput;
    public bool isJumping; // 점프 여부
    public bool isDash; // 연주

    public Vector3 moveDirection;
    public bool isForward;
    public bool isBack;
    public bool isLeft;
    public bool isRight;
}
