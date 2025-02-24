using System.Collections.Generic;
using System.Collections;
using UnityEngine;

public class MidBossGuardBehaviorTreeFactory : MidBossBehaviorTreeFactory
{
  [Header("파동 프리팹")]
  public GameObject wavePrefab; // 파동

  [Header("파동 쿨타임")] public float waveRate = 4f; // 파동 쿨타임
  [Header("파동 속도")] public float waveSpeed = 20f; // 파동 속도
  [Header("돌진 속도")] public float dashSpeed = 14f; // 돌진 속도

  float lastAttackTimePlayer = 0f;  // 마지막 공격 시간 (플레이어)
  float lastAttackTimeBitCore = 0f; // 마지막 공격 시간 (비트코어)
  bool canWave = true;
  
  // 비트 코어 공격 재정의
  public override IBehaviorNode.EBehaviorNodeState AttackBitCore(Transform bitCore, MonsterStats monsterStats, Transform monster)
  {
    if(isPlayerTarget) return IBehaviorNode.EBehaviorNodeState.Failure; // 플레이어 어그로라면 중지

    if(Time.time - lastAttackTimeBitCore >= waveRate && canWave)
    {
      StartCoroutine(Wave(monsterStats, bitCore));
      lastAttackTimeBitCore = Time.time;
    }
    else if(!canWave)
    {
      lastAttackTimeBitCore = Time.time;
    }

    Debug.Log("공격 상태(비트코어)");
    return IBehaviorNode.EBehaviorNodeState.Success;
  }

  // 플레이어 공격 재정의
  public override IBehaviorNode.EBehaviorNodeState PerformAttack(Transform player, MonsterStats monsterStats, Transform monster)
  {
    if(!isPlayerTarget) return IBehaviorNode.EBehaviorNodeState.Failure; // 플레이어 어그로가 아니라면 비트코어 타겟팅

    if(Time.time - lastAttackTimePlayer >= waveRate && canWave)
    {
      StartCoroutine(Wave(monsterStats, player));
      lastAttackTimePlayer = Time.time;
    }
    else if(!canWave)
    {
      lastAttackTimePlayer = Time.time;
    }

    Debug.Log("공격 상태");
    return IBehaviorNode.EBehaviorNodeState.Success;
  }

  // 파동 코루틴
  private IEnumerator Wave(MonsterStats monsterStats, Transform player)
  {
    Debug.Log("파동");
    canWave = false;

    GameObject wave = Instantiate(wavePrefab, transform.position, Quaternion.identity);
    float maxScale = monsterStats.attackRange * 10f;  // 파동 최대 크기
    wave.transform.localScale = new Vector3(wave.transform.position.x, 30f, wave.transform.position.z);

    while(wave.transform.localScale.x <= maxScale)
    {
      float scaleIncress = waveSpeed * 2.5f * Time.deltaTime;
      wave.transform.localScale += new Vector3(scaleIncress, 0, scaleIncress);
      yield return null;
    }

    Destroy(wave);
    canWave = true;

    StartCoroutine(Dash(player, monsterStats));
  }

  // 돌진 코루틴
  private IEnumerator Dash(Transform player, MonsterStats monsterStats)
  {
    while(Vector3.Distance(transform.position, player.position) > 0.5f)
    {
      Debug.Log("돌진");
      transform.position = Vector3.MoveTowards(transform.position, player.position, dashSpeed * Time.deltaTime);
      yield return null;
    }
  }
}
