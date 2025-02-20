using Fusion;
using UnityEngine;

public struct NetworkInputData : INetworkInput
{
    public Vector3 direction; // 이동 방향
    public bool isJumping; // 점프 여부
}
