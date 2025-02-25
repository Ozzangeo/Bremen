using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Fusion;
using Unity.VisualScripting;
using UnityEditor.U2D.Animation;

public class LobbyPlayerController : NetworkBehaviour
{
    [SerializeField] private TMP_Text playerNameTxt;
    [SerializeField] private GameObject readyObject;
    [SerializeField] private GameObject[] characterModels; // �𵨸� ��� ������ �ڵ� �ҷ������ ����

    private Button readyBtn;
    private Button leftBtn;
    private Button rightBtn;

    [Networked] public string playerName { get; set; }
    private string previousPlayerName;

    [Networked] public bool isReady { get; set; }
    [Networked] public int characterIndex { get; set; }

    private CharacterData[] characterDatas;

    private void Start()
    {
        readyBtn = GameObject.Find("ReadyButton")?.GetComponent<Button>();
        leftBtn = GameObject.Find("LeftButton")?.GetComponent<Button>();
        rightBtn = GameObject.Find("RightButton")?.GetComponent<Button>();

        readyBtn.onClick.AddListener(ToggleReady);
        leftBtn.onClick.AddListener(() => ChangeCharacter(-1));
        rightBtn.onClick.AddListener(() => ChangeCharacter(1));

        if(Object.HasInputAuthority)
        {
            SetName_RPC(PlayerData.Instance.playerName);
        }

        UpdatePlayerNameUI();
        UpdateReady();
        UpdateCharacterModel();

        characterDatas = Resources.LoadAll<CharacterData>("CharacterDatas");
    }

    public override void FixedUpdateNetwork()
    {
        if (playerName != previousPlayerName)
        {
            UpdatePlayerNameUI();
            previousPlayerName = playerName; // ���� �� ������Ʈ
        }
    }

    private void UpdatePlayerNameUI()
    {
        playerNameTxt.text = playerName;
    }

    private void ToggleReady()
    {
        if (!Object.HasInputAuthority) return;

        isReady = !isReady;
        UpdateReady();
        UpdateReady_RPC(isReady);

        if (LobbyManager.Instance != null)
        {
            LobbyManager.Instance.CheckAllReady();
        }
    }

    private void ChangeCharacter(int offset)
    {
        if (!Object.HasInputAuthority) return;

        characterIndex += offset;
        if (characterIndex >= characterModels.Length)
        {
            characterIndex = 0;
        }
        else if (characterIndex < 0)
        {
            characterIndex = characterModels.Length - 1;
        }

        PlayerData.Instance.selectedCharacter = characterDatas[characterIndex];

        UpdateCharacterModel();
        UpdateCharacterIndex_RPC(characterIndex);
    }

    [Rpc(RpcSources.InputAuthority | RpcSources.StateAuthority, RpcTargets.All)]
    private void SetName_RPC(string name)
    {
        playerName = name;
        playerNameTxt.text = playerName;
    }

    [Rpc(RpcSources.InputAuthority | RpcSources.StateAuthority, RpcTargets.All)]
    private void UpdateReady_RPC(bool readyStatus)
    {
        isReady = readyStatus;
        UpdateReady();
    }

    [Rpc(RpcSources.InputAuthority | RpcSources.StateAuthority, RpcTargets.All)]
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

    public override void Spawned()
    {
        base.Spawned(); // �⺻ ���� ���� (�ʼ��� �ƴ����� �������� ����)

        if (LobbyManager.Instance != null)
        {
            LobbyManager.Instance.RegisterPlayer(this);
        }
    }
}
