using UnityEngine;

public class BossBehavior : BasicEntityBehaviour
{
  [Header("보스 패턴 팩토리")]
  public BossBehaviorTreeFactory treeFactory;

  IBehaviorNode rootNode; // 행동 트리 루트 노드
  Transform player;       // 플레이어 오브젝트 위치
  Vector3 spawnPosition;  // 몬스터 스폰 위치

  private void Start()
  {
    player = GameObject.Find("Player").transform;
    spawnPosition = transform.position;

    if(treeFactory == null) treeFactory = GetComponent<BossBehaviorTreeFactory>();

    rootNode = treeFactory.CreateBehaviorTree(transform, player, monsterStats);
  }

  private void Update()
  {
    rootNode.Evaluate();
  }
}
