using Fusion;
using NUnit.Framework;
using UnityEngine;

public class PlayerData : MonoBehaviour
{
    public static PlayerData Instance { get; private set; }

    public string playerName;
    public string roomCode;
    public CharacterData selectedCharacter;
    public bool[] isClear = new bool[2];

    public int hp;
    public float moveSpeed;

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
        hp = selectedCharacter.maxHP;
        moveSpeed = selectedCharacter.moveSpeed;
    }
}
