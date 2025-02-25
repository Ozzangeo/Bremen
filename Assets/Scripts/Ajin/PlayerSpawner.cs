using Fusion;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSpawner : MonoBehaviour
{
    public static PlayerSpawner Instance;

    [SerializeField] private GameObject _lobbyPlayerPrefab;
    [SerializeField] private GameObject _playerPrefab;
    public Dictionary<PlayerRef, NetworkObject> spawnedCharacters = new Dictionary<PlayerRef, NetworkObject>();
    private Transform SpawnPoint;

    private void Awake()
    {
        if(Instance == null)
            Instance = this;
    }

    private void Start()
    {
        if(SpawnPoint == null)
        {
            SpawnPoint = GameObject.Find("SpawnPoint").transform;
        }
    }

    // ===== Lobby =====
    public void SpawnLobbyPlayer(NetworkRunner runner, PlayerRef player)
    {
        if(runner.IsServer)
        {
            if(!spawnedCharacters.ContainsKey(player))
            {
                Vector3 spawnPosition = new Vector3((player.RawEncoded % 4) * 2, 1, 0);
                NetworkObject networkPlayerObject = runner.Spawn(_lobbyPlayerPrefab, spawnPosition, Quaternion.identity, player);

                spawnedCharacters[player] = networkPlayerObject;
            }
        }
    }

    public void DespawnLobbyPlayer(NetworkRunner runner, PlayerRef player)
    {
        if (spawnedCharacters.TryGetValue(player, out NetworkObject playerObject))
        {
            runner.Despawn(playerObject);
            spawnedCharacters.Remove(player);
        }
    }

    // ===== Game ===== 
    public void SpawnGamePlayer(NetworkRunner runner)
    {
        if (runner.IsServer)
        {
            foreach(var player in spawnedCharacters.Keys)
            {
                Vector3 spawnPosition = new Vector3(SpawnPoint.position.x + (player.RawEncoded % 4) * 2, SpawnPoint.position.y + 1, SpawnPoint.position.z);
                NetworkObject networkPlayerObject = runner.Spawn(_playerPrefab, spawnPosition, Quaternion.identity, player);
            }
        }
    }

    public void DespawnGamePlayer(NetworkRunner runner, PlayerRef player)
    {
        if (spawnedCharacters.TryGetValue(player, out NetworkObject playerObject))
        {
            runner.Despawn(playerObject);
            spawnedCharacters.Remove(player);
        }
    }

    // ================================

    public NetworkObject GetPlayerObject(PlayerRef player)
    {
        spawnedCharacters.TryGetValue(player, out NetworkObject playerObject);
        return playerObject;
    }

    public List<PlayerRef> GetAllPlayers()
    {
        List<PlayerRef> allPlayers = new List<PlayerRef>();
        foreach (var player in spawnedCharacters.Keys)
        {
            allPlayers.Add(player);
        }
        return allPlayers;
    }

    public List<PlayerRef> GetOtherPlayer()
    {
        List<PlayerRef> otherPlayer = new List<PlayerRef>();
        foreach (var player in spawnedCharacters.Keys)
        {
            if(player != GameSessionManager.Instance.runner.LocalPlayer)
            {
                otherPlayer.Add(player);
            }
        }
        return otherPlayer;
    }
}
