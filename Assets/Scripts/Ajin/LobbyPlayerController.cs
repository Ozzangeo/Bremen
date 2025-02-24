using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Fusion;
using Unity.VisualScripting;

public class LobbyPlayerController : MonoBehaviour
{
    [SerializeField] private TMP_Text playerNameTxt;
    [SerializeField] private GameObject readyObject;
    [SerializeField] private GameObject[] characterModels; // 모델링 모두 들어오면 자동 불러오기로 수정

    private Button readyBtn;
    private Button leftBtn;
    private Button rightBtn;

    [Networked] public string playerName { get; set; }
    [Networked] public bool isReady { get; set; }
    [Networked] public int characterIndex { get; set; }

    private bool lastReadyState;
    private int lastCharacterIndex;

    private NetworkObject localPlayer;
    private LobbyPlayerController _lobbyPlayerController;
    private bool isLocalPlayer = false;

    private void Start()
    {

        readyBtn = GameObject.Find("ReadyButton")?.GetComponent<Button>();
        leftBtn = GameObject.Find("LeftButton")?.GetComponent<Button>();
        rightBtn = GameObject.Find("RightButton")?.GetComponent<Button>();

        readyBtn.onClick.AddListener(ToggleReady);
        leftBtn.onClick.AddListener(() => ChangeCharacter(-1));
        rightBtn.onClick.AddListener(() => ChangeCharacter(1));

        localPlayer =  PlayerSpawner.Instance.GetPlayerObject(GameSessionManager.Instance.runner.LocalPlayer);
        if(localPlayer != null)
        {
            _lobbyPlayerController = localPlayer.GetComponent<LobbyPlayerController>(); 
        }
        if (_lobbyPlayerController == this)
        {
            isLocalPlayer = true;
        }

        if(isLocalPlayer)
        {
            playerNameTxt.text = PlayerData.Instance.playerName;
            playerName = PlayerData.Instance.playerName;
        }

        UpdateUI();
    }

    public void FixedUpdateNetwork()
    {
        if (lastReadyState != isReady)
        {
            lastReadyState = isReady;
            UpdateReady();
        }

        if (lastCharacterIndex != characterIndex)
        {
            lastCharacterIndex = characterIndex;
            UpdateCharacterModel();
        }
    }

    private void ToggleReady()
    {
        if (!isLocalPlayer) return;

        isReady = !isReady;
        UpdateReady();
        UpdateReady_RPC(isReady);
    }

    private void ChangeCharacter(int offset)
    {
        if (!isLocalPlayer) return;

        characterIndex += offset;
        if (characterIndex >= characterModels.Length)
        {
            characterIndex = 0;
        }
        else if (characterIndex < 0)
        {
            characterIndex = characterModels.Length - 1;
        }

        UpdateCharacterModel();
        UpdateCharacterIndex_RPC(characterIndex);
    }

    [Rpc(RpcSources.StateAuthority, RpcTargets.All)]
    private void UpdateReady_RPC(bool readyStatus)
    {
        isReady = readyStatus;
        UpdateReady();
    }

    // 캐릭터 인덱스 동기화를 위한 RPC
    [Rpc(RpcSources.StateAuthority, RpcTargets.All)]
    private void UpdateCharacterIndex_RPC(int index)
    {
        characterIndex = index;
        UpdateCharacterModel();
    }

    private void UpdateReady()
    {
        readyObject.SetActive(isReady);
    }

    private void UpdateCharacterModel()
    {
        for (int i = 0; i < characterModels.Length; i++)
        {
            characterModels[i].SetActive(i == characterIndex);
        }
    }

    private void UpdateUI()
    {
        UpdateReady();
        UpdateCharacterModel();
    }
}
