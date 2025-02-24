using UnityEngine;

[CreateAssetMenu(fileName = "CharacterData", menuName = "Scriptable Objects/CharacterData")]
public class CharacterData : ScriptableObject
{
    public string characterName;
    public int maxHP;
    public float moveSpeed;
    public int comboUnit;
    public GameObject characterPrefab;
    public RuntimeAnimatorController animatorController;
}
