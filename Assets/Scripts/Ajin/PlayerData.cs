using Fusion;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.Rendering;

public class PlayerData : MonoBehaviour
{
    public static PlayerData Instance { get; private set; }

    public string playerName;
    public string roomCode;
    public string selectedCharacter;
    public bool[] isClear = new bool[3];

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

    public void SetCharacter(string characterData)
    {
        selectedCharacter = characterData;
    }
}
