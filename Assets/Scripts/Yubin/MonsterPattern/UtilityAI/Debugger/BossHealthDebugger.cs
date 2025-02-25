using TMPro;
using UnityEngine;

public class BossHealthDebugger : MonoBehaviour
{
  Transform boss;
  BossInfoManager bossInfoManager;

  private void Awake()
  {
    boss = GameObject.Find("Boss").transform;
    bossInfoManager = boss.GetComponent<BossInfoManager>();
  }

  public void OnButtonClick()
  {
    bossInfoManager.Hit(5500);
  }
}
