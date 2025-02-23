using System.Collections.Generic;
using System.Collections;
using UnityEngine;

public class MidBossBitCoreBehaviorTreeFactory : MidBossBehaviorTreeFactory
{
  [Header("파동 프리팹")]
  public GameObject wavePrefab; // 파동

  [Header("파동 쿨타임")] public float waveRate = 4f;
  [Header("파동 속도")] public float waveSpeed = 20f;
  [Header("파동 이동 속도")] public float waveMoveSpeed = 10f;

  float lastAttackTime = 0f;

  public override IBehaviorNode CreateBehaviorTree(Transform monster, Transform player, Transform bitCore, MonsterStats monsterStats, Vector3 spawnPosition)
  {
    // 1. 6등분 각도중에 하나 선택
    // 2. 디버프 결정(둘중하나)
    // 3. 4박자에 한 번 선택한 각도로 파동 생성

    IBehaviorNode performAttack = new ActionNode(() => PerformAttack(player, monsterStats, monster)); // 공격
    IBehaviorNode rootSelector = new SelectorNode(new List<IBehaviorNode> { performAttack });

    return rootSelector;
  }

  // 플레이어 공격 재정의
  public override IBehaviorNode.EBehaviorNodeState PerformAttack(Transform player, MonsterStats monsterStats, Transform monster)
  {
    // 파동
    if(Time.time - lastAttackTime >= waveRate)
    {
      float angle = ChooseAngle();
      Quaternion rotation = Quaternion.Euler(0, angle, 0);
      StartCoroutine(Wave(monsterStats, rotation));

      lastAttackTime = Time.time;
    }

    // 디버프 결정

    Debug.Log("공격 상태(플레이어)");
    return IBehaviorNode.EBehaviorNodeState.Success;
  }

  // 각도 선택
  private float ChooseAngle()
  {
    float rand = Random.Range(0f, 360f);
    if (rand < 60f)  return 30f;
    if (rand < 120f) return 90f;
    if (rand < 180f) return 150f;
    if (rand < 240f) return 210f;
    if (rand < 300f) return 270f;
    return 330f;
  }

  // 파동 코루틴
  private IEnumerator Wave(MonsterStats monsterStats, Quaternion angle)
  {
    GameObject wave = Instantiate(wavePrefab, transform.position, angle);
    Vector3 direction = angle * Vector3.forward;

    Destroy(wave, 5f);

    while(true && wave != null)
    {
      float scaleIncrease = waveSpeed * 0.35f * Time.deltaTime;
      wave.transform.localScale += new Vector3(scaleIncrease, 0, 0);
      wave.transform.position += direction * waveMoveSpeed * Time.deltaTime;
      yield return null;
    }
  }

}
