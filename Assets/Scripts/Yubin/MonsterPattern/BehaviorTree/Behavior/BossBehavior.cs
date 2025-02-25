using Ozi.Weapon.Entity;
using System.Collections.Generic;
using UnityEngine;

public class BossBehavior : BasicEntityBehaviour
{
  [Header("보스 패턴 팩토리")]
  public BossBehaviorTreeFactory treeFactory;

  IBehaviorNode rootNode; // 행동 트리 루트 노드
  Transform player;       // 플레이어 오브젝트 위치
  Vector3 spawnPosition;  // 몬스터 스폰 위치
  List<Transform> players;

  private void Start()
  {
    players = new List<Transform>();
    GameObject[] playerObjects = GameObject.FindGameObjectsWithTag("Player");

    foreach (GameObject obj in playerObjects)
    {
      players.Add(obj.transform);
    }

    spawnPosition = transform.position;

    if(treeFactory == null) treeFactory = GetComponent<BossBehaviorTreeFactory>();

    rootNode = treeFactory.CreateBehaviorTree(transform, players, monsterStats);
  }

  private void Update()
  {
    rootNode.Evaluate();
  }
}
