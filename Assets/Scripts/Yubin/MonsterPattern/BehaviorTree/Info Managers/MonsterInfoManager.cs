using UnityEngine;

public class MonsterInfoManager : MonoBehaviour
{
  MonsterBehavior monsterBehavior;
  BehaviorTreeFactory monsterBehaviorTreeFactory;

  void Awake()
  {
    monsterBehavior = GetComponent<MonsterBehavior>();
    monsterBehaviorTreeFactory = GetComponent<BehaviorTreeFactory>();

    monsterBehavior.OnDead += () => {
      monsterBehaviorTreeFactory.MidBossDie();
    };
  }
}
