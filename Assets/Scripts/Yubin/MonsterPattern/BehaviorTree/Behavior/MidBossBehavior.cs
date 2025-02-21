using UnityEngine;

// 중간 보스 행동 실행
public class MidBossBehavior : MonoBehaviour
{
  [Header("몬스터 능력치")]
  public MonsterStats monsterStats;

  [Header("몬스터 패턴 팩토리")]
  public MidBossBehaviorTreeFactory treeFactory;

  IBehaviorNode rootNode; // 행동 트리 루트 노드
  Transform player;       // 플레이어 오브젝트 위치
  Transform bitCore;      // 비트 코어 위치
  Vector3 spawnPosition;  // 몬스터 스폰 위치

  private void Start()
  {
    player = GameObject.Find("Player").transform;
    bitCore = GameObject.Find("BitCore").transform;
    spawnPosition = transform.position;

    if(treeFactory == null) treeFactory = GetComponent<MidBossBehaviorTreeFactory>();

    rootNode = treeFactory.CreateBehaviorTree(transform, player, bitCore,monsterStats, spawnPosition);
  }

  private void Update()
  {
    rootNode.Evaluate();
  }
}
