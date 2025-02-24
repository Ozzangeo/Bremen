using UnityEngine;

public class MidBossInfoManager : MonoBehaviour
{
  MidBossBehavior midBossBehavior;
  MidBossBehaviorTreeFactory midBossBehaviorTreeFactory;

  void Awake()
  {
    midBossBehavior = GetComponent<MidBossBehavior>();
    midBossBehaviorTreeFactory = GetComponent<MidBossBehaviorTreeFactory>();

    midBossBehavior.OnHit += o => {
      midBossBehaviorTreeFactory.TargetPlayer();
    };

    midBossBehavior.OnDead += () => {
      midBossBehavior.MidBossDie();
    };
  }
}
