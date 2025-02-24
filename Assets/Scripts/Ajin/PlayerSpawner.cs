using Fusion;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSpawner : MonoBehaviour
{
    public static PlayerSpawner Instance;

    [SerializeField] private GameObject _lobbyPlayerPrefab;
    [SerializeField] private GameObject _playerPrefab;
    public Dictionary<PlayerRef, NetworkObject> spawnedCharacters = new Dictionary<PlayerRef, NetworkObject>();

    private void Awake()
    {
        if(Instance == null)
            Instance = this;
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
                Vector3 spawnPosition = new Vector3((player.RawEncoded % 4) * 2, 1, 0);
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

    public List<NetworkObject> GetAllPlayerObject()
    {
        List<NetworkObject> allPlayers = new List<NetworkObject>();
        foreach (var player in spawnedCharacters.Values)
        {
            allPlayers.Add(player);
        }
        return allPlayers;
    }
}
