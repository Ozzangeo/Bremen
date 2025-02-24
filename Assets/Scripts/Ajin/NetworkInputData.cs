using Fusion;
using UnityEngine;

public struct NetworkInputData : INetworkInput
{
    // GameScene
    public Vector3 direction; // �̵� ����
    public Vector2 moveInput;
    public bool isMoving; // ������
    public bool isJumping; // ���� ����
    public bool isPlaying; // ����
}
