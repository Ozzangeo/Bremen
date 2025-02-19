using UnityEngine;

// 몬스터 행동 (임시)
public class MonsterBehavior : MonoBehaviour
{
  // [Header("공격 범위(빨간색)")]
  // public float attackRange = 2.0f;
  // [Header("탐지 범위(초록색)")]
  // public float detectionRange = 10.0f;
  // [Header("이동 속도")]
  // public float moveSpeed = 3.0f;

  [Header("몬스터 능력치")]
  public MonsterStats monsterStats;

  IBehaviorNode rootNode; // 행동 트리 루트 노드
  Transform player;       // 플레이어 오브젝트 위치
  Vector3 spawnPosition;  // 몬스터 스폰 위치

  private void Start()
  {
    player = GameObject.Find("Player").transform;
    spawnPosition = transform.position;

    IBehaviorTreeFactory behaviorTreeFactory = new MonsterBehaviorTreeFactory();
    rootNode = behaviorTreeFactory.CreateBehaviorTree(transform, player, monsterStats, spawnPosition);
  }

  private void Update()
  {
    rootNode.Evaluate();
  }
}
