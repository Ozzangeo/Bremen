using UnityEngine;

public class MonsterInfoManager : MonoBehaviour
{
  MonsterBehavior monsterBehavior;
  BehaviorTreeFactory monsterBehaviorTreeFactory;

  float health = 0f;

  void Awake()
  {
    monsterBehavior = GetComponent<MonsterBehavior>();
    monsterBehaviorTreeFactory = GetComponent<BehaviorTreeFactory>();
    health = monsterBehavior.monsterStats.health;
  }

  // 피격
  public void GetDamage(float damage)
  {
    health -= damage;

    if(health <= 0)
    {
      monsterBehaviorTreeFactory.MidBossDie();
    }
  }
}
