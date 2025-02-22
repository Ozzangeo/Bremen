using UnityEngine;

public interface IMidBossBehaviorTreeFactory
{
  // 중간 보스의 행동 트리 생성
  IBehaviorNode CreateBehaviorTree(Transform monster, Transform player, Transform bitCore, MonsterStats monsterStats, Vector3 spawnPosition);
}
