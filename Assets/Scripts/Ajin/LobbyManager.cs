using UnityEngine;
using UnityEngine.UI;

public class LobbyManager : MonoBehaviour
{
    [SerializeField] Text codeTxt;

    private void Start()
    {
        codeTxt.text = PlayerData.Instance.roomCode;
    }
}
