using UnityEngine;

public class MonsterGetDamagedDebugger : MonoBehaviour
{
  public GameObject midBoss;
  MidBossBehaviorTreeFactory midBossBehaviorTreeFactory;

  void Start()
  {
    midBossBehaviorTreeFactory = midBoss.GetComponent<MidBossBehaviorTreeFactory>();
  }

  public void temp()
  {
    midBossBehaviorTreeFactory.GetDamage();
  }
}
