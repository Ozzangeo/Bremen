using TMPro;
using UnityEngine;

public class BossHealthDebugger : MonoBehaviour
{
  public TextMeshProUGUI text;

  Transform boss;
  BossUtilityAI bossUtilityAI;

  private void Awake()
  {
    boss = GameObject.Find("Boss").transform;
    bossUtilityAI = boss.GetComponent<BossUtilityAI>();
  }

  public void OnButtonClick()
  {
    bossUtilityAI.health -= 30;
  }

  private void Update()
  {
    string newText = "BOSS HP:";
    int currentHP = (int)bossUtilityAI.health;
    text.text = newText + currentHP;
  }
}
