using System.Collections.Generic;
using System.Collections;
using UnityEngine;

// EMP 발생장치 행동 트리를 생성하는 팩토리
// 4박자가 정확히 뭔지 몰라서 4초에 한번 발사하는거로 임시 구현
[RequireComponent(typeof(MonsterBehavior))]
public class EMPgeneratorBehaviorTreeFactory : BehaviorTreeFactory
{
  [Header("파동 프리팹")]
  public GameObject wavePrefab; // 파동

  [Header("파동 쿨타임")] public float waveRate = 4f;
  [Header("파동 속도")] public float waveSpeed = 80f;

  float lastAttackTime = 0f;
  bool canWave = true;
  GameObject currentWave;

  public override IBehaviorNode CreateBehaviorTree(Transform monster, List<Transform> players, MonsterStats monsterStats, Vector3 spawnPosition)
  {
    this.players = players;
    player = ClosestPlayer(monster, players, monsterStats.patrolRange, spawnPosition);
    
    // 개별 액션 노드
    IBehaviorNode checkAttackRange = new ActionNode(() => CheckAttackRange(monster, monsterStats)); // 공격 범위 확인
    IBehaviorNode performAttack = new ActionNode(() => PerformAttack(monsterStats, spawnPosition)); // 공격

    // 공격 시퀸스 노드
    IBehaviorNode attackSequence = new SequenceNode(new List<IBehaviorNode> { checkAttackRange, performAttack });

    // 추적, 순찰 로직 없음
    IBehaviorNode rootSelector = new SelectorNode(new List<IBehaviorNode> { attackSequence });

    return rootSelector;
  }

  // 공격 실행 재정의
  public override IBehaviorNode.EBehaviorNodeState PerformAttack(MonsterStats monsterStats, Vector3 spawnPosition)
  {
    if(Time.time - lastAttackTime >= waveRate && canWave)
    {
      StartCoroutine(Wave(monsterStats));
      lastAttackTime = Time.time;
    }
    else if(!canWave)
    {
      lastAttackTime = Time.time;
    }

    Debug.Log("공격 상태");
    return IBehaviorNode.EBehaviorNodeState.Success;
  }

  // 파동 코루틴
  private IEnumerator Wave(MonsterStats monsterStats)
  {
    canWave = false;

    currentWave = Instantiate(wavePrefab, transform.position, Quaternion.identity);
    MonsterAttackPlayer monsterAttackPlayer = currentWave.GetComponent<MonsterAttackPlayer>(); 
    monsterAttackPlayer.Initialize(monsterStats.attackPower);

    float maxScale = monsterStats.attackRange * 10f;  // 파동 최대 크기
    currentWave.transform.localScale = new Vector3(currentWave.transform.position.x, 30f, currentWave.transform.position.z);

    while(currentWave.transform.localScale.x <= maxScale)
    {
      float scaleIncress = waveSpeed * 2.5f * Time.deltaTime;
      currentWave.transform.localScale += new Vector3(scaleIncress, 0, scaleIncress);
      yield return null;
    }

    Destroy(currentWave);
    canWave = true;
  }

  // 몬스터 사망
  public override void MidBossDie()
  {
    Debug.Log("몬스터 사망");
    
    Destroy(currentWave);
    Destroy(gameObject);
  }
}