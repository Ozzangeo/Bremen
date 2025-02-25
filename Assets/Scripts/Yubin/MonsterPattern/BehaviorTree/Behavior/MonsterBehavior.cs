using Ozi.Weapon.Entity;
using System.Collections.Generic;
using UnityEngine;

// 몬스터 행동 실행

public class MonsterBehavior : BasicEntityBehaviour
{
  // [Header("공격 범위(빨간색)")]
  // public float attackRange = 2.0f;
  // [Header("탐지 범위(초록색)")]
  // public float detectionRange = 10.0f;
  // [Header("이동 속도")]
  // public float moveSpeed = 3.0f;

  [Header("몬스터 패턴 팩토리")]
  public BehaviorTreeFactory treeFactory;

  IBehaviorNode rootNode; // 행동 트리 루트 노드
  Vector3 spawnPosition;  // 몬스터 스폰 위치
  List<Transform> players;
  private Vector3 lastPosition;

  private void Start()
  {
    players = new List<Transform>();
    GameObject[] playerObjects = GameObject.FindGameObjectsWithTag("Player");

    foreach (GameObject obj in playerObjects)
    {
      players.Add(obj.transform);
    }

    spawnPosition = transform.position;
    lastPosition = transform.position;
    if(treeFactory == null) treeFactory = GetComponent<BehaviorTreeFactory>();

    rootNode = treeFactory.CreateBehaviorTree(transform, players, monsterStats, spawnPosition);
  }

  private void Update()
  {
    if(rootNode != null)
    {
      rootNode.Evaluate();
    }

    RotateTowardsMovementDirection();

    lastPosition = transform.position; // 현재 위치 저장
  }

  // 이동 방향으로 회전하는 함수
  private void RotateTowardsMovementDirection()
  {
    Vector3 movementDirection = transform.position - lastPosition;

    if (movementDirection.magnitude > 0.01f) // 이동 중일 때만 회전
    {
      movementDirection.y = 0; // Y축 회전 방지 (수평 회전만 적용)
      Quaternion targetRotation = Quaternion.LookRotation(movementDirection);
      transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * 5f);
    }
  }
}