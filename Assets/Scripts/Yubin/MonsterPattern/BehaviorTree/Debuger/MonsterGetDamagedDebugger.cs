using UnityEngine;

public class MonsterGetDamagedDebugger : MonoBehaviour
{
  public GameObject midBoss;
  public float damage;
  MidBossInfoManager midBossInfo;

  void Start()
  {
    midBossInfo = midBoss.GetComponent<MidBossInfoManager>();
  }

  public void temp()
  {
    midBossInfo.GetDamage(damage);
  }
}
