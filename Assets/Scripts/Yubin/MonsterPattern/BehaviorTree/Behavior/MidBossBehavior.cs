using Ozi.Weapon.Entity;
using System.Collections.Generic;
using UnityEngine;

// 중간 보스 행동 실행
public class MidBossBehavior : BasicEntityBehaviour 
{
  [Header("몬스터 패턴 팩토리")]
  public MidBossBehaviorTreeFactory treeFactory;

  IBehaviorNode rootNode; // 행동 트리 루트 노드
  Transform bitCore;      // 비트 코어 위치
  Vector3 spawnPosition;  // 몬스터 스폰 위치
  List<Transform> players;
  private Vector3 lastPosition;

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
    
    bitCore = GameObject.Find("Mid Boss BitCore").transform;
    spawnPosition = transform.position;
    lastPosition = transform.position;

    if(treeFactory == null) treeFactory = GetComponent<MidBossBehaviorTreeFactory>();

    rootNode = treeFactory.CreateBehaviorTree(transform, players, bitCore,monsterStats, spawnPosition);
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

  // 중간 보스 사망
  public void MidBossDie()
  {
    Debug.Log("중간 보스 사망");
    
    Destroy(gameObject);
  }
}
