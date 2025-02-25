using System.Collections.Generic;
using System.Collections;
using UnityEngine;

// 가드드 행동 트리를 생성하는 팩토리
// 4박자에 1번이 정확히 뭔지 몰라서 4초에 한번 공격하는거로 임시 구현
[RequireComponent(typeof(MonsterBehavior))]
public class GuardBehaviorTreeFactory : BehaviorTreeFactory
{
  [Header("파동 프리팹")]
  public GameObject wavePrefab; // 파동

  [Header("파동 쿨타임")] public float waveRate = 4f; // 파동 쿨타임
  [Header("파동 속도")] public float waveSpeed = 20f; // 파동 속도
  [Header("돌진 속도")] public float dashSpeed = 14f; // 돌진 속도

  float lastAttackTime = 0f;  // 처음 한 번은 바로
  bool canWave = true;
  Animator animator;

  void Start()
  {
    animator = GetComponent<Animator>();
  }
  
  // 공격 실행 재정의
  public override IBehaviorNode.EBehaviorNodeState PerformAttack(MonsterStats monsterStats, Vector3 spawnPosition)
  {
    if(Time.time - lastAttackTime >= waveRate && canWave)
    {
      StartCoroutine(Wave(monsterStats, player));
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
    animator.SetBool("IsAttack", true);

    while(Vector3.Distance(transform.position, player.position) > 0.5f)
    {
      Debug.Log("돌진");
      transform.position = Vector3.MoveTowards(transform.position, player.position, dashSpeed * Time.deltaTime);
      yield return null;
    }

    animator.SetBool("IsAttack", false);
  }
}
