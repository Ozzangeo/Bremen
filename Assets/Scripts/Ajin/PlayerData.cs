using Fusion;
using UnityEngine;

public class PlayerData : MonoBehaviour
{
    public static PlayerData Instance { get; private set; }

    public int localPlayerID;
    public int ownerId;
    public string playerName;
    public string roomCode;
    public CharacterData selectedCharacter;

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
        }
    }

    private void Start()
    {
        //localPlayerID = RunnerAOIGizmos.LocalPlayer.PlayerID;
        //ownerId = object.InputAuthority.PlayerID;
    }
}
