using System.Collections;
using System.Collections.Generic;
using Ozi.Weapon.Entity;
using UnityEngine;

[RequireComponent(typeof(MonsterBehavior))]
public class RhythmHackerBehaviorTreeFactory : BehaviorTreeFactory
{
  [Header("파동 프리팹")]
  public GameObject wavePrefab; // 파동

  [Header("투명화 해제 속도")] public float releaseTime = 5f;  // 투명화 해제 속도
  [Header("반투명화 속도")] public float applicationSpeed = 4f; // 반투명화 속도

  [Header("파동 속도")] public float waveSpeed = 20f; // 파동 속도

  [Header("머터리얼")] public Renderer monsterRenderer;
  private Coroutine transparencyCoroutine;
  private bool isVisible = false;
  private bool isAttacking = false;

  public override IBehaviorNode CreateBehaviorTree(Transform monster, List<Transform> players, MonsterStats monsterStats, Vector3 spawnPosition)
  {
    this.players = players;
    player = ClosestPlayer(monster, players, monsterStats.patrolRange, spawnPosition);
    // 개별 액션 노드
    IBehaviorNode checkAttackRange = new ActionNode(() => CheckAttackRange(monster, monsterStats));          // 공격 범위 확인
    IBehaviorNode performAttack = new ActionNode(() => PerformAttack(monsterStats, spawnPosition)); // 공격
    IBehaviorNode chasePlayer = new ActionNode(() => ChasePlayer(monster, monsterStats, spawnPosition));     // 추적
    IBehaviorNode patrolArea = new ActionNode(() => Patrol(monster, monsterStats, spawnPosition));           // 순찰
    IBehaviorNode returnToSpawn = new ActionNode(() => ReturnToSpawn(monster, spawnPosition, monsterStats));         // 복귀

    // 공격 시퀸스 노드
    IBehaviorNode attackSequence = new SequenceNode(new List<IBehaviorNode> { checkAttackRange, performAttack });

    // 순찰 루틴 (공격 범위에 없다면 순찰 or 복귀)
    IBehaviorNode patrolSelector = new SelectorNode(new List<IBehaviorNode> { patrolArea, returnToSpawn });

    // 공격 가능하면 공격 → 아니면 플레이어 추적 → 순찰 또는 복귀
    IBehaviorNode rootSelector = new SelectorNode(new List<IBehaviorNode> { chasePlayer, attackSequence, patrolSelector });

    return rootSelector;
  }

  private void Start()
  {
    if(monsterRenderer == null) monsterRenderer = GetComponent<Renderer>();
    SetTransparency(0f);
  }

  // 추적 재정의
  public override IBehaviorNode.EBehaviorNodeState ChasePlayer(Transform monster, MonsterStats monsterStats, Vector3 spawnPosition)
  {
    float patrolRange = monsterStats.patrolRange;  // 순찰 범위
    float moveSpeed = monsterStats.moveSpeed;      // 이동 속도
    float attackRange = monsterStats.attackRange;  // 공격 범위

    // 플레이어가 순찰 범위를 벗어나면 투명화
    float playerDistanceFromSpawn = Vector3.Distance(player.position, spawnPosition);
    if(playerDistanceFromSpawn > patrolRange)
    {
      Debug.Log("순찰 상태 전환");

      if(isVisible)
      {
        StopTransparencyEffect();
      }

      return IBehaviorNode.EBehaviorNodeState.Failure;
    }

    // 플레이어가 순찰 범위 내에 들어오면 투명화 해제
    if(!isVisible && transparencyCoroutine == null)
    {
      transparencyCoroutine = StartCoroutine(FadeIn());
    }

    // 공격 범위 내에 들어왔으면 공격 상태로 전환
    float distanceToPlayer = Vector3.Distance(monster.position, player.position);
    if(distanceToPlayer <= attackRange) return IBehaviorNode.EBehaviorNodeState.Failure;

    // 플레이어가 탐지 범위 내에 있으면 계속 추적
    Debug.Log("추적 상태");
    monster.position = Vector3.MoveTowards(monster.position, player.position, moveSpeed * Time.deltaTime);
    return IBehaviorNode.EBehaviorNodeState.Running;
  }

  // 공격 실행 재정의 파동 -> 반투명 -> 이동 반복
  public override IBehaviorNode.EBehaviorNodeState PerformAttack(MonsterStats monsterStats, Vector3 spawnPosition)
  {
    if(!isAttacking)
    {
      StartCoroutine(AttackSequence(transform, player, monsterStats, spawnPosition));
    }

    Debug.Log("공격 상태");
    return IBehaviorNode.EBehaviorNodeState.Success;
  }

  // 공격 시퀸스: 파동 -> 반투명 -> 이동 반복
  private IEnumerator AttackSequence(Transform monster, Transform player, MonsterStats monsterStats, Vector3 spawnPosition)
  {
    isAttacking = true;

    // 파동 발사
    StartCoroutine(Wave(monsterStats));
    yield return new WaitForSeconds(0.1f);

    // 반투명화 (Alpha = 0.7)
    yield return StartCoroutine(SetSemiTransparent());

    // 플레이어에게 이동
    float playerDistance = Vector3.Distance(transform.position, player.position);
    if(playerDistance <= monsterStats.patrolRange)
    {
      yield return StartCoroutine(MoveToPlayer(monster, player, monsterStats, spawnPosition));
    }
    
    yield return new WaitForSeconds(0.1f); 

    isAttacking = false;
  }

  // 반투명화
  private IEnumerator SetSemiTransparent()
  {
    Debug.Log("반투명화");
    float elapsedTime = 0f;
    while(elapsedTime < applicationSpeed)
    {
      elapsedTime += Time.deltaTime;
      float alpha = Mathf.Lerp(1f, 0.7f, elapsedTime / applicationSpeed);
      SetTransparency(alpha);
      yield return null;
    }
    SetTransparency(0.7f);
  }

  // 플레이어에게 이동
  private IEnumerator MoveToPlayer(Transform monster, Transform player, MonsterStats monsterStats, Vector3 spawnPosition)
  {
    Debug.Log("이동");
    float dashTime = 0.5f;
    float elapsedTime = 0f;
    Vector3 startPosition = monster.position;
    Vector3 targetPosition = GetCenterPlayer(monsterStats.patrolRange, spawnPosition);

    while(elapsedTime < dashTime)
    {
      elapsedTime += Time.deltaTime;
      monster.position = Vector3.Lerp(startPosition, targetPosition, elapsedTime / dashTime);
      yield return null;
    }

    monster.position = targetPosition;
  }

  // 순찰 범위 내 플레이어들의 중앙 위치
  private Vector3 GetCenterPlayer(float patrolRange, Vector3 spawnPosition)
  {
    Vector3 sumPosition = Vector3.zero;
    int count = 0;

    foreach (Transform player in players)
    {
      float distance = Vector3.Distance(player.position, spawnPosition);
      if (distance <= patrolRange)
      {
        sumPosition += player.position;
        count++;
      }
    }
    // 만약 순찰 범위 내 플레이어가 없다면 가장 가까운 플레이어에게 이동
    if (count == 0)
    {
      Transform closestPlayer = ClosestPlayer(transform, players, patrolRange, spawnPosition);
      return closestPlayer != null ? closestPlayer.position : spawnPosition;
    }

    return sumPosition / count; // 중앙 위치 반환
  }

  // 파동 코루틴
  private IEnumerator Wave(MonsterStats monsterStats)
  {
    Debug.Log("파동");

    GameObject wave = Instantiate(wavePrefab, transform.position, Quaternion.identity);
    MonsterAttackPlayer monsterAttackPlayer = wave.GetComponent<MonsterAttackPlayer>();
    monsterAttackPlayer.Initialize(monsterStats.attackPower);
    
    float maxScale = monsterStats.attackRange * 10f;  // 파동 최대 크기
    wave.transform.localScale = new Vector3(wave.transform.position.x, 30f, wave.transform.position.z);

    while(wave.transform.localScale.x <= maxScale)
    {
      float scaleIncress = waveSpeed * 2.5f * Time.deltaTime;
      wave.transform.localScale += new Vector3(scaleIncress, 0, scaleIncress);
      yield return null;
    }

    Destroy(wave);
  }

  // 투명화 해제
  private IEnumerator FadeIn()
  {
    Debug.Log("투명화 해제");
    isVisible = true;
    float elapsedTime = 0f;

    while(elapsedTime < releaseTime)
    {
      elapsedTime += Time.deltaTime;
      float alpha = Mathf.Lerp(0f, 1f, elapsedTime / releaseTime);
      SetTransparency(alpha);
      yield return null;
    }

    SetTransparency(1f);
    transparencyCoroutine = null;
  }

  // 즉시 투명
  private void StopTransparencyEffect()
  {
    Debug.Log("즉시 투명");

    if(transparencyCoroutine != null)
    {
      StopCoroutine(transparencyCoroutine);
      transparencyCoroutine = null;
    }

    SetTransparency(0f);
    isVisible = false;
  }

  // 투명도 설정
  private void SetTransparency(float alpha)
  {
    Color color = monsterRenderer.material.color;
    color.a = alpha;
    monsterRenderer.material.color = color;
  }
}