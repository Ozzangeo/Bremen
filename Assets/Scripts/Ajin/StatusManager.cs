using Fusion;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StatusManager : MonoBehaviour
{
    [SerializeField] public Image playerHPBar;
    [SerializeField] public Image comboBar;
    [SerializeField] public Text comboText;

    [SerializeField] public Image[] otherPlayerHp;

    private List<PlayerRef> players;
    private NetworkObject currentPlayerObject;
    private PlayerController currentPlayerController;
    private CharacterData characterData;

    private void Start()
    {
        players = PlayerSpawner.Instance.GetAllPlayers();
    }

    public void UpdatePlayerStatusUI()
    {
        int index = 0;
        foreach (PlayerRef player in players)
        {
            currentPlayerObject = PlayerSpawner.Instance.GetPlayerObject(player);
            currentPlayerController = currentPlayerObject.GetComponent<PlayerController>();
            characterData = Resources.Load<CharacterData>(currentPlayerController.selectCharacter);

            if (player == GameSessionManager.Instance.runner.LocalPlayer)
            {
                playerHPBar.fillAmount = currentPlayerController.hp / characterData.maxHP;
                comboBar.fillAmount = currentPlayerController.combo / 150f;
                comboText.text =  "HP : " + currentPlayerController.combo.ToString();
            }
            else
            {
                otherPlayerHp[index].fillAmount = currentPlayerController.hp / characterData.maxHP;
                index++;
            }
        }
    }
}
