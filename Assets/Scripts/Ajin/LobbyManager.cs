using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using Fusion;

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
        //if (!Object.HasStateAuthority) return;

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

    private bool CanStartGame()
    {
        int minPlayersRequired = 3;
        if (lobbyPlayers.Count < minPlayersRequired)
        {
            Debug.Log("게임 시작 불가: 최소 플레이어 수 부족!");
            return false;
        }

        if (!lobbyPlayers.All(player => player.isReady))
        {
            Debug.Log("게임 시작 불가: 일부 플레이어가 준비되지 않음!");
            return false;
        }

        if(!character())
        {
            return false;
        }

        return true;
    }

    private bool character()
    {
        int[] selectIndex = new int[3];
        int index = 0;
        foreach (var player in lobbyPlayers)
        {
            selectIndex[index] = player.GetComponent<LobbyPlayerController>().characterIndex;
            index++;
        }
        if (selectIndex[0] == selectIndex[1] || selectIndex[1] == selectIndex[2] || selectIndex[0] == selectIndex[2])
        {
            return false;
        }
        else
        {
            return true;
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
        GameSessionManager.Instance.runner.LoadScene("StageSelectScene"); // 씬 전환 실행
        if (GameSessionManager.Instance.runner.IsServer)
        {
            GameSessionManager.Instance.runner.LoadScene("StageSelectScene");
        }
        else
        {
            Debug.LogWarning("클라이언트는 씬을 로드할 수 없습니다!");
        }
    }

    [Rpc(RpcSources.StateAuthority, RpcTargets.All)]
    private void StartGame_RPC()
    {
        GameSessionManager.Instance.runner.LoadScene("StageSelectScene");
    }
}
