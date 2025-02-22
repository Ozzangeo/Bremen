using UnityEngine;

[RequireComponent(typeof(MonsterBehavior))]
public class RhythmHackerBehaviorTreeFactory : BehaviorTreeFactory
{
  [Header("투명화 해제 속도")] public float ReleaseTime = 5f;  // 투명화 해제 속도
  
}
