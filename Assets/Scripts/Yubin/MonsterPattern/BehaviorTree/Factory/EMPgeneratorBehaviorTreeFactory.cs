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

  [Header("공격 쿨타임")] public float waveRate = 4f;
  [Header("파동 속도")] public float waveSpeed = 3f;

  float lastAttackTime = 0f;
  bool canWave = true;

  public override IBehaviorNode CreateBehaviorTree(Transform monster, Transform player, MonsterStats monsterStats, Vector3 spawnPosition)
  {
    // 개별 액션 노드
    IBehaviorNode checkAttackRange = new ActionNode(() => CheckAttackRange(monster, player, monsterStats)); // 공격 범위 확인
    IBehaviorNode performAttack = new ActionNode(() => PerformAttack(player, monsterStats, spawnPosition)); // 공격

    // 공격 시퀸스 노드
    IBehaviorNode attackSequence = new SequenceNode(new List<IBehaviorNode> { checkAttackRange, performAttack });

    // 추적, 순찰 로직 없음
    IBehaviorNode rootSelector = new SelectorNode(new List<IBehaviorNode> { attackSequence });

    return rootSelector;
  }

  // 공격 실행 재정의
  public override IBehaviorNode.EBehaviorNodeState PerformAttack(Transform player, MonsterStats monsterStats, Vector3 spawnPosition)
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

    GameObject wave = Instantiate(wavePrefab, transform.position, Quaternion.identity);
    float maxScale = monsterStats.attackRange * 2f;  // 파동 최대 크기

    while(wave.transform.localScale.x <= maxScale)
    {
      float scaleIncress = waveSpeed  * Time.deltaTime;
      wave.transform.localScale += new Vector3(scaleIncress, 0, scaleIncress);
      yield return null;
    }

    Destroy(wave);
    canWave = true;
  }

}