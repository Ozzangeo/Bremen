using Fusion;
using Fusion.Sockets;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Linq;

public class GameSessionManager : MonoBehaviour, INetworkRunnerCallbacks
{
    public static GameSessionManager Instance { get; private set; }

    public NetworkRunner runner;

    private void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(this.gameObject);
        }
        else
        {
            Destroy(gameObject);
            return;
        }
    }

    public async void EnterGame()
    {
        Debug.Log("EnterGame");

        if (runner == null)
        {
            runner = gameObject.AddComponent<NetworkRunner>();
            runner.ProvideInput = true;
        }

        var scene = SceneRef.FromIndex(SceneManager.GetActiveScene().buildIndex);
        var sceneInfo = new NetworkSceneInfo();
        if (scene.IsValid)
        {
            sceneInfo.AddSceneRef(scene, LoadSceneMode.Additive);
        }

        // 게임 시작
        await runner.StartGame(new StartGameArgs()
        {
            GameMode = GameMode.Shared,  // 필요에 따라 변경 가능 (Client, Host 등)
            SessionName = PlayerData.Instance.roomCode,
            Scene = scene,
            SceneManager = gameObject.AddComponent<NetworkSceneManagerDefault>()
        });

        runner.AddCallbacks(this);
        PlayerSpawner.Instance.SpawnGamePlayer(runner);
    }

    public async void EnterRoomWithCode(string roomCode, GameMode mode)
    {
        if(runner == null)
        {
            runner = gameObject.AddComponent<NetworkRunner>();
            runner.ProvideInput = true;
        }

        await SceneManager.LoadSceneAsync("LobbyScene", LoadSceneMode.Single);

        var scene = SceneRef.FromIndex(SceneManager.GetActiveScene().buildIndex);
        var sceneInfo = new NetworkSceneInfo();
        if (scene.IsValid)
        {
            sceneInfo.AddSceneRef(scene, LoadSceneMode.Additive);
        }

        await runner.StartGame(new StartGameArgs()
        {
            GameMode = mode,
            SessionName = roomCode,
            Scene = scene,
            SceneManager = gameObject.AddComponent<NetworkSceneManagerDefault>()
        });

        runner.AddCallbacks(this);
    }

    public void OnPlayerJoined(NetworkRunner runner, PlayerRef player)
    {
        Debug.Log($"{player.PlayerId}");
        PlayerSpawner.Instance.SpawnLobbyPlayer(runner, player);
    }

    public void OnPlayerLeft(NetworkRunner runner, PlayerRef player)
    {
        Debug.Log($"{player.PlayerId}");
        PlayerSpawner.Instance.DespawnLobbyPlayer(runner, player);
    }

    public void OnInput(NetworkRunner runner, NetworkInput input) { }
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
