using UnityEngine;
using UnityEngine.SceneManagement;
using Fusion;
using Fusion.Sockets;
using System.Collections.Generic;
using System;

public class BasicSpawnerTest : MonoBehaviour, INetworkRunnerCallbacks
{
    [SerializeField] private NetworkPrefabRef _playerPrefab;
    private NetworkRunner _runner;
    //현재 방에 접속 중인 플레이어의 정보와 네트워크 오브젝트를 담는 딕셔너리
    private CameraDirectionChecker cameraDirectionChecker;

    private void Start()
    {
        cameraDirectionChecker = GameObject.Find("CameraController").GetComponent<CameraDirectionChecker>();
    }

    async void StartGame(GameMode mode)
    {
        _runner = gameObject.AddComponent<NetworkRunner>();
        _runner.ProvideInput = true;

        var scene = SceneRef.FromIndex(SceneManager.GetActiveScene().buildIndex);
        var sceneInfo = new NetworkSceneInfo();
        if(scene.IsValid)
        {
            sceneInfo.AddSceneRef(scene, LoadSceneMode.Additive);
        }

        await _runner.StartGame(new StartGameArgs()
        {
            GameMode = mode,
            SessionName = "TestRoom",
            Scene = scene,
            SceneManager = gameObject.AddComponent<NetworkSceneManagerDefault>()
        });
    }

    private void OnGUI()
    {
        if(_runner == null)
        {
            if(GUI.Button(new Rect(0,0,200,40), "Host"))
            {
                StartGame(GameMode.Host);
            }
            if(GUI.Button(new Rect(0,40,200,40), "Join"))
            {
                StartGame(GameMode.Client);
            }
        }
    }

    public void OnPlayerJoined(NetworkRunner runner, PlayerRef player)
    {
        if(runner.IsServer)
        {
            Vector3 spawnPosition = new Vector3((player.RawEncoded % runner.Config.Simulation.PlayerCount) * 3, 1, 0);
            runner.Spawn(_playerPrefab, spawnPosition, Quaternion.identity, player);
        }
    }
    public void OnPlayerLeft(NetworkRunner runner, PlayerRef player)
    {
        
    }
    public void OnInput(NetworkRunner runner, NetworkInput input)
    {
        var data = new NetworkInputData();

        if (Input.GetKey(KeyCode.W))
        {
            Vector3 cameraDirection = cameraDirectionChecker.GetNearestDirection();
            cameraDirection = cameraDirectionChecker.transform.TransformDirection(cameraDirection);
            data.direction += cameraDirection;
        }
        if (Input.GetKey(KeyCode.S))
        {
            Vector3 cameraDirection = cameraDirectionChecker.GetBackDirection();
            cameraDirection = cameraDirectionChecker.transform.TransformDirection(cameraDirection);
            data.direction += cameraDirection;
        }
        if (Input.GetKey(KeyCode.A))
        {
            Vector3 cameraDirection = cameraDirectionChecker.GetLeftDirection();
            cameraDirection = cameraDirectionChecker.transform.TransformDirection(cameraDirection);
            data.direction += cameraDirection;
        }
        if (Input.GetKey(KeyCode.D))
        {
            Vector3 cameraDirection = cameraDirectionChecker.GetRightDirection();
            cameraDirection = cameraDirectionChecker.transform.TransformDirection(cameraDirection);
            data.direction += cameraDirection;
        }

        data.isDash = Input.GetMouseButtonDown(1);
        data.isJumping = Input.GetKeyDown(KeyCode.Space);

        input.Set(data);
    }

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
