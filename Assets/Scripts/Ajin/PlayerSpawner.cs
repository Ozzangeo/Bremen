using Fusion;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSpawner : MonoBehaviour
{
    public static PlayerSpawner Instance;

    [SerializeField] private GameObject _lobbyPlayerPrefab;
    [SerializeField] private GameObject _playerPrefab;
    public Dictionary<PlayerRef, NetworkObject> spawnedLobbyCharacters = new Dictionary<PlayerRef, NetworkObject>();
    public Dictionary<PlayerRef, NetworkObject> spawnedGameCharacters = new Dictionary<PlayerRef, NetworkObject>();

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
            if(!spawnedLobbyCharacters.ContainsKey(player))
            {
                Vector3 spawnPosition = new Vector3((player.RawEncoded % 4) * 2, 1, 0);
                NetworkObject networkPlayerObject = runner.Spawn(_lobbyPlayerPrefab, spawnPosition, Quaternion.identity, player);

                spawnedLobbyCharacters[player] = networkPlayerObject;
            }
        }
    }

    public void DespawnLobbyPlayer(NetworkRunner runner, PlayerRef player)
    {
        if (spawnedLobbyCharacters.TryGetValue(player, out NetworkObject playerObject))
        {
            runner.Despawn(playerObject);
            spawnedLobbyCharacters.Remove(player);
        }
    }

    // ===== Game ===== 

    public void SpawnGamePlayer(NetworkRunner runner, PlayerRef player)
    {
        if (runner.IsServer)
        {
            if (!spawnedGameCharacters.ContainsKey(player))
            {
                Vector3 spawnPosition = new Vector3((player.RawEncoded % 4) * 2, 1, 0);
                NetworkObject networkPlayerObject = runner.Spawn(_playerPrefab, spawnPosition, Quaternion.identity, player);

                spawnedGameCharacters[player] = networkPlayerObject;
            }
        }
    }

    public void DespawnGamePlayer(NetworkRunner runner, PlayerRef player)
    {
        if (spawnedGameCharacters.TryGetValue(player, out NetworkObject playerObject))
        {
            runner.Despawn(playerObject);
            spawnedGameCharacters.Remove(player);
        }
    }

    // ================================

    public NetworkObject GetPlayerObject(PlayerRef player)
    {
        spawnedLobbyCharacters.TryGetValue(player, out NetworkObject playerObject);
        return playerObject;
    }

    public List<NetworkObject> GetAllPlayerObject()
    {
        List<NetworkObject> allPlayers = new List<NetworkObject>();
        foreach (var player in spawnedLobbyCharacters.Values)
        {
            allPlayers.Add(player);
        }
        return allPlayers;
    }
}
