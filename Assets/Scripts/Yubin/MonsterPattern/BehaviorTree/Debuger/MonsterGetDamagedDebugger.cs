using UnityEngine;

public class MonsterGetDamagedDebugger : MonoBehaviour
{
  public GameObject midBoss;
  public float damage;
  MonsterInfoManager midBossInfo;

  void Start()
  {
    midBossInfo = midBoss.GetComponent<MonsterInfoManager>();
  }

  public void temp()
  {
    // midBossInfo.GetDamage(damage);
  }
}
