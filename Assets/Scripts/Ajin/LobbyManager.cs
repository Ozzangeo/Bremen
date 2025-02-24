using System.Collections.Generic;
using System.Linq;
using UnityEditorInternal;
using UnityEngine;
using UnityEngine.UI;
using Fusion;
using static Unity.Collections.Unicode;

public class LobbyManager : NetworkBehaviour
{
    public static LobbyManager Instance;
    [SerializeField] Text codeTxt;
    private List<LobbyPlayerController> lobbyPlayers = new List<LobbyPlayerController>();

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
    }

    private void Start()
    {
        codeTxt.text = PlayerData.Instance.roomCode;
    }

    private void Update()
    {
        if (!Object.HasStateAuthority) return;

        lobbyPlayers = FindObjectsOfType<LobbyPlayerController>(true).ToList();

        Debug.Log($"현재 로비에 있는 플레이어 수: {lobbyPlayers.Count}");

        foreach (var player in lobbyPlayers)
        {
            Debug.Log($"플레이어 {player.playerName} 준비 상태: {player.isReady}");
        }

        if (lobbyPlayers.Count > 0 && lobbyPlayers.All(player => player.isReady))
        {
            Debug.Log("모든 플레이어가 준비 완료! 씬 전환");
            StartGame_RPC();
        }
    }

    public void RegisterPlayer(LobbyPlayerController player)
    {
        if (!lobbyPlayers.Contains(player))
        {
            lobbyPlayers.Add(player);
            Debug.Log($"플레이어 등록됨: {player.playerName}, 총 {lobbyPlayers.Count}명");
        }
    }

    public void CheckAllReady()
    {
        foreach (var player in lobbyPlayers)
        {
            Debug.Log($"플레이어 {player.playerName} 준비 상태: {player.isReady}");
            if (!player.isReady)
                return; // 하나라도 준비가 안 됐다면 종료
        }

        Debug.Log("모든 플레이어가 준비 완료! 씬 전환");
        if (GameSessionManager.Instance.runner == null)
        {
            Debug.LogError(" NetworkRunner가 존재하지 않습니다!");
            return;
        }
        GameSessionManager.Instance.runner.LoadScene("PhotonTestScene"); // 씬 전환 실행
        if (GameSessionManager.Instance.runner.IsServer)
        {
            GameSessionManager.Instance.runner.LoadScene("PhotonTestScene");
        }
        else
        {
            Debug.LogWarning("❌ 클라이언트는 씬을 로드할 수 없습니다!");
        }
    }

    [Rpc(RpcSources.StateAuthority, RpcTargets.All)]
    private void StartGame_RPC()
    {
        GameSessionManager.Instance.runner.LoadScene("PhotonTestScene");
    }
}
