using Fusion;
using System;
using UnityEngine;

public struct NetworkInputData : INetworkInput
{
    // GameScene
    public Vector3 direction; // �̵� ����
    public Vector2 moveInput;
    public bool isJumping; // ���� ����
    public bool isDash; // ����

    public Vector3 moveDirection;
    public bool isForward;
    public bool isBack;
    public bool isLeft;
    public bool isRight;
}
