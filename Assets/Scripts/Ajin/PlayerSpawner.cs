using Fusion;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class PlayerSpawner : MonoBehaviour
{
    public static PlayerSpawner Instance;

    [SerializeField] private GameObject _lobbyPlayerPrefab;

    public Dictionary<PlayerRef, NetworkObject> spawnedLobby = new Dictionary<PlayerRef, NetworkObject>();
    public Dictionary<PlayerRef, NetworkObject> spawnedPlayers = new Dictionary<PlayerRef, NetworkObject>();
    public Dictionary<PlayerRef, string> spawnedGame = new Dictionary<PlayerRef, string>();
    private Transform SpawnPoint;
    private PlayerController _playerController;
    private CharacterData _characterData;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
            return;
        }
    }

    private void Start()
    {
    }

    // ===== Lobby =====
    public void SpawnLobbyPlayer(NetworkRunner runner, PlayerRef player)
    {
        if(runner.IsServer)
        {
            if(!spawnedLobby.ContainsKey(player))
            {
                Vector3 spawnPosition = new Vector3((player.RawEncoded % 4) * 2, 1, 0);
                NetworkObject networkPlayerObject = runner.Spawn(_lobbyPlayerPrefab, spawnPosition, Quaternion.identity, player);

                spawnedLobby[player] = networkPlayerObject;
                Debug.Log($"{player} - {networkPlayerObject.Name}");
            }
        }
    }

    public void DespawnLobbyPlayer(NetworkRunner runner, PlayerRef player)
    {
        if (spawnedLobby.TryGetValue(player, out NetworkObject playerObject))
        {
            runner.Despawn(playerObject);
            spawnedLobby.Remove(player);
        }
    }

    public void SetCharacters(PlayerRef player,  string selectCharacter)
    {
        spawnedGame.Add(player, selectCharacter);
    }

    public void SetSpawnedPlayers()
    {
        foreach(var player in spawnedGame.Keys)
        {
            _characterData = Resources.Load<CharacterData>(GetPlayerCharacter(player));
            spawnedPlayers[player] = _characterData.characterPrefab.GetComponent<NetworkObject>();
        }
    }
    // ================================

    public string GetPlayerCharacter(PlayerRef player)
    {
        string characterName = spawnedGame[player];
        return characterName;
    }

    public NetworkObject GetPlayerObject(PlayerRef player)
    {
        NetworkObject networkObject = spawnedPlayers[player];
        return networkObject;
    }

    public List<PlayerRef> GetAllPlayers()
    {
        List<PlayerRef> allPlayers = new List<PlayerRef>();
        foreach (var player in spawnedLobby.Keys)
        {
            allPlayers.Add(player);
        }
        return allPlayers;
    }

    public List<PlayerRef> GetOtherPlayer()
    {
        List<PlayerRef> otherPlayer = new List<PlayerRef>();
        foreach (var player in spawnedLobby.Keys)
        {
            if(player != GameSessionManager.Instance.runner.LocalPlayer)
            {
                otherPlayer.Add(player);
            }
        }
        return otherPlayer;
    }
}
