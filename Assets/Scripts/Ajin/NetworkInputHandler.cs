using Fusion;
using Fusion.Sockets;
using System.Collections.Generic;
using System;
using UnityEngine;

public class NetworkInputHandler : MonoBehaviour, INetworkRunnerCallbacks
{
    private CameraDirectionChecker cameraDirectionChecker;

    private void Start()
    {
        cameraDirectionChecker = GameObject.Find("CameraController").GetComponent<CameraDirectionChecker>();
    }

    public void OnInput(NetworkRunner runner, NetworkInput input)
    {
        Debug.Log("¿Œ«≤¿Œ«≤");
        var data = new NetworkInputData();

        if (Input.GetKey(KeyCode.W))
        {
            Vector3 cameraDirection = cameraDirectionChecker.GetNearestDirection();
            cameraDirection = cameraDirectionChecker.transform.TransformDirection(cameraDirection);
            data.direction += cameraDirection;
            data.isDash = Input.GetMouseButtonDown(1);
        }
        if (Input.GetKey(KeyCode.S))
        {
            Vector3 cameraDirection = cameraDirectionChecker.GetBackDirection();
            cameraDirection = cameraDirectionChecker.transform.TransformDirection(cameraDirection);
            data.direction += cameraDirection;
            data.isDash = Input.GetMouseButtonDown(1);
        }
        if (Input.GetKey(KeyCode.A))
        {
            Vector3 cameraDirection = cameraDirectionChecker.GetLeftDirection();
            cameraDirection = cameraDirectionChecker.transform.TransformDirection(cameraDirection);
            data.direction += cameraDirection;
            data.isDash = Input.GetMouseButtonDown(1);
        }
        if (Input.GetKey(KeyCode.D))
        {
            Vector3 cameraDirection = cameraDirectionChecker.GetRightDirection();
            cameraDirection = cameraDirectionChecker.transform.TransformDirection(cameraDirection);
            data.direction += cameraDirection;
            data.isDash = Input.GetMouseButtonDown(1);
        }
        data.isDash = Input.GetMouseButtonDown(1);
        data.isJumping = Input.GetKeyDown(KeyCode.Space);

        input.Set(data);
    }
    public void OnPlayerJoined(NetworkRunner runner, PlayerRef player) { }
    public void OnPlayerLeft(NetworkRunner runner, PlayerRef player) { }
    public void OnInputMissing(NetworkRunner runner, PlayerRef player, NetworkInput input) { }
    public void OnShutdown(NetworkRunner runner, ShutdownReason shutdownReason) { }
    public void OnConnectedToServer(NetworkRunner runner) { }
    public void OnDisconnectedFromServer(NetworkRunner runner, NetDisconnectReason reason) { }
    public void OnConnectRequest(NetworkRunner runner, NetworkRunnerCallbackArgs.ConnectRequest request, byte[] token) { }
    public void OnConnectFailed(NetworkRunner runner, NetAddress remoteAddress, NetConnectFailedReason reason) { }
    public void OnUserSimulationMessage(NetworkRunner runner, SimulationMessagePtr message) { }
    public void OnSessionListUpdated(NetworkRunner runner, List<SessionInfo> sessionList) { }
    public void OnCustomAuthenticationResponse(NetworkRunner runner, Dictionary<string, object> data) { }
    public void OnHostMigration(NetworkRunner runner, HostMigrationToken hostMigrationToken) { }
    public void OnSceneLoadDone(NetworkRunner runner) { }
    public void OnSceneLoadStart(NetworkRunner runner) { }
    public void OnObjectExitAOI(NetworkRunner runner, NetworkObject obj, PlayerRef player) { }
    public void OnObjectEnterAOI(NetworkRunner runner, NetworkObject obj, PlayerRef player) { }
    public void OnReliableDataReceived(NetworkRunner runner, PlayerRef player, ReliableKey key, ArraySegment<byte> data) { }
    public void OnReliableDataProgress(NetworkRunner runner, PlayerRef player, ReliableKey key, float progress) { }
}
