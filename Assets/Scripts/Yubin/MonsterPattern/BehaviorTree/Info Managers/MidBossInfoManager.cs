using UnityEngine;

public class MidBossInfoManager : MonoBehaviour
{
  MidBossBehavior midBossBehavior;
  MidBossBehaviorTreeFactory midBossBehaviorTreeFactory;

  float health = 0f;

  void Awake()
  {
    midBossBehavior = GetComponent<MidBossBehavior>();
    midBossBehaviorTreeFactory = GetComponent<MidBossBehaviorTreeFactory>();
    health = midBossBehavior.monsterStats.health;
  }

  // 피격
  public void GetDamage(float damage)
  {
    health -= damage;
    midBossBehaviorTreeFactory.TargetPlayer();

    if(health <= 0)
    {
      midBossBehavior.MidBossDie();
    }
  }
}
