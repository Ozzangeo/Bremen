using Fusion;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSpawner : MonoBehaviour
{
    public static PlayerSpawner Instance;

    [SerializeField] private GameObject _playerPrefab;
    private Dictionary<PlayerRef, NetworkObject> _spawnedCharacters = new Dictionary<PlayerRef, NetworkObject>();

    private void Awake()
    {
        if(Instance == null)
            Instance = this;
    }

    public void SpawnPlayer(NetworkRunner runner, PlayerRef player)
    {
        if(runner.IsServer)
        {
            Vector3 spawnPosition = new Vector3((player.RawEncoded % 4) * 2, 1, 0);
            NetworkObject networkPlayerObject = runner.Spawn(_playerPrefab, spawnPosition, Quaternion.identity, player);

            if (networkPlayerObject != null)
            {
                _spawnedCharacters[player] = networkPlayerObject;
            }
        }
    }

    public void DespawnPlayer(NetworkRunner runner, PlayerRef player)
    {
        if (_spawnedCharacters.TryGetValue(player, out NetworkObject playerObject))
        {
            runner.Despawn(playerObject);
            _spawnedCharacters.Remove(player);
        }
    }

    public NetworkObject GetPlayerObject(PlayerRef player)
    {
        _spawnedCharacters.TryGetValue(player, out NetworkObject playerObject);
        return playerObject;
    }
}
