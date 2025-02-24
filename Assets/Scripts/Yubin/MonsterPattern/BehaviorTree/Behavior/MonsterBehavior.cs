using System.Collections.Generic;
using UnityEngine;

// 몬스터 행동 실행

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

  [Header("몬스터 패턴 팩토리")]
  public BehaviorTreeFactory treeFactory;

  IBehaviorNode rootNode; // 행동 트리 루트 노드
  Vector3 spawnPosition;  // 몬스터 스폰 위치
  List<Transform> players;

  private void Start()
  {
    players = new List<Transform>();
    GameObject[] allObjects = Resources.FindObjectsOfTypeAll<GameObject>();

    foreach(GameObject obj in allObjects)
    {
      if(obj.name == "Player" && obj.activeInHierarchy)
      {
        players.Add(obj.transform);
      }
    }

    spawnPosition = transform.position;

    if(treeFactory == null) treeFactory = GetComponent<BehaviorTreeFactory>();

    rootNode = treeFactory.CreateBehaviorTree(transform, players, monsterStats, spawnPosition);
  }

  private void Update()
  {
    rootNode.Evaluate();
  }
}
