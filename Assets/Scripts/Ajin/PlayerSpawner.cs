using Fusion;
using System.Collections.Generic;
using UnityEngine;


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
        if (runner.IsServer)
        {
            if (!spawnedLobby.ContainsKey(player))
            {
                // 카메라의 중앙 x 위치 가져오기
                float cameraCenterX = Camera.main.transform.position.x;

                // x 좌표를 3등분 (-2, 0, 2로 정렬)
                float offsetX = (player.RawEncoded % 3) * 2 - 2;

                // 새로운 스폰 위치 (Y는 0.3, Z는 -2 고정, X만 카메라 기준)
                Vector3 spawnPosition = new Vector3(cameraCenterX + offsetX, 0.3f, -2f);

                // 플레이어 오브젝트 생성
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
