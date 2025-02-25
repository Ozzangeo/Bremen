using Fusion;
using System.Collections.Generic;
using UnityEngine;

public class GamePlayerSpawner : MonoBehaviour
{
    public static GamePlayerSpawner Instance;

    [SerializeField] private GameObject _playerPrefab_1;
    [SerializeField] private GameObject _playerPrefab_2;
    [SerializeField] private GameObject _playerPrefab_3;

    private Transform SpawnPoint;
    private PlayerController _playerController;
    private CharacterData _characterData;

    private void Awake()
    {
        if (Instance == null)
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

    private void Start()
    {
        SpawnGamePlayer(GameSessionManager.Instance.runner);
        PlayerSpawner.Instance.SetSpawnedPlayers();
    }

    // ===== Game ===== 
    public async void SpawnGamePlayer(NetworkRunner runner)
    {
        Debug.Log("SpawnGamePlayer");
        SpawnPoint = GameObject.Find("SpawnPoint").transform;
        if (runner.IsServer)
        {
            foreach (var player in PlayerSpawner.Instance.spawnedGame.Keys)
            {
                Debug.Log($"{player}!!!");
                Vector3 spawnPosition = new Vector3(SpawnPoint.position.x + (player.RawEncoded % 4) * 2, SpawnPoint.position.y, SpawnPoint.position.z);
                _characterData = Resources.Load<CharacterData>(PlayerSpawner.Instance.GetPlayerCharacter(player));
                NetworkObject playerObject = await runner.SpawnAsync(_characterData.characterPrefab, spawnPosition, Quaternion.identity, player); ;
                playerObject.AssignInputAuthority(player);
                _playerController =  _characterData.characterPrefab.GetComponent<PlayerController>();
                _playerController.SetCharacter();
                if(PlayerSpawner.Instance.GetPlayerObject(player).HasInputAuthority)
                {
                    Debug.Log("HasInputAuthority");
                }
            }
        }
    }

    public void DespawnGamePlayer(NetworkRunner runner, PlayerRef player)
    {
        if (PlayerSpawner.Instance.spawnedLobby.TryGetValue(player, out NetworkObject playerObject))
        {
            runner.Despawn(playerObject);
            PlayerSpawner.Instance.spawnedLobby.Remove(player);
        }
    }
}
