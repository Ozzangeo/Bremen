using TMPro;
using UnityEngine;

public class BossHealthDebugger : MonoBehaviour
{
  Transform boss;
  BossPattern bossPattern;

  private void Awake()
  {
    boss = GameObject.Find("Boss").transform;
    bossPattern = boss.GetComponent<BossPattern>();
  }

  public void OnButtonClick()
  {
    bossPattern.GetDamage(5500);
  }
}
