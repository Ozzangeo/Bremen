using System;
using System.Collections.Generic;
using System.Collections;
using UnityEngine;

// 보스 패턴의 FSM 관리
public enum EBossState { Phase1, Phase2, Phase3 }

public class BossPattern : MonoBehaviour
{
  [Header("보스 능력치")] public BossStats bossStats;
  [Header("보스 상태 표시")] public EBossState bossState = EBossState.Phase1;

  BossSpawnMonster bossSpawnMonster;
  BossCreateWave bossCreateWave;
  BossDropObject bossDropObject;
  BossCreateLaser bossCreateLaser;
  BossInfoManager bossInfoManager;

  List<Transform> player;

  void Start()
  {
    if(bossSpawnMonster == null) bossSpawnMonster = GetComponent<BossSpawnMonster>();
    if(bossCreateWave == null) bossCreateWave = GetComponent<BossCreateWave>();
    if(bossDropObject == null) bossDropObject = GetComponent<BossDropObject>();
    if(bossCreateLaser == null) bossCreateLaser = GetComponent<BossCreateLaser>();
    if(bossInfoManager == null) bossInfoManager = GetComponent<BossInfoManager>();

    // 초기화
    bossStats.Initialize();
    bossSpawnMonster.Initialize();
    bossCreateWave.Initialize();
    bossDropObject.Initialize();
    bossCreateLaser.Initialize();
    bossInfoManager.Initialize();

    player = new List<Transform>();
    GameObject[] allObjects = Resources.FindObjectsOfTypeAll<GameObject>();

    foreach(GameObject obj in allObjects)
    {
      if(obj.name == "Player" && obj.activeInHierarchy)
      {
        player.Add(obj.transform);
      }
    }

    // 페이즈 1
    StartCoroutine(Phase1());
  }

  void Update()
  {
    // 보스 사망
    if(bossStats.health <= 0)
    {
      BossDie();
    }
  }

  public void BossDie()
  {
      Debug.Log("보스 사망");

      bossSpawnMonster.DeleteMonster();
      bossCreateWave.DeleteWave();
      bossDropObject.DeleteDropObject();
      bossCreateLaser.DeleteLaser();

      Destroy(gameObject);
  }

  // 상태 갱신
  public void UpdateState(EBossState state)
  {
    switch(state)
    {
      case EBossState.Phase2:
        StartCoroutine(Phase2());
        break;
      case EBossState.Phase3:
        StartCoroutine(Phase3());
        break;
      default:
        break;
    }
  }

  // 페이즈 1 실행
  public IEnumerator Phase1()
  {
    Debug.Log("페이즈 1");

    // 전투 드론 4개 생성
    bossSpawnMonster.SpawnMonster(4);

    // 4박자 파동
    bossCreateWave.CreateWave(4f, bossStats.waveSpeed);
    
    // 2박자 낙하물 1번
    bossDropObject.DropObject(player, 2f, 1);

    // 레이저 발사 시계 한바퀴 반시계 반바퀴
    bossCreateLaser.CreateLaser();
    yield return StartCoroutine(bossCreateLaser.RotateLaser(360f, 90f));
    yield return StartCoroutine(bossCreateLaser.RotateLaser(-180f, 90f));
    bossCreateLaser.DeleteLaser();
  }

  // 페이즈 2 실행
  public IEnumerator Phase2()
  {
    Debug.Log("페이즈 2");

    // 전투 드론 7개 생성
    bossSpawnMonster.SpawnMonster(7);

    // 2박자 파동
    bossCreateWave.CreateWave(2f, bossStats.waveSpeed);

    // 2박자 낙하물 2번
    bossDropObject.DropObject(player, 2f, 2);

    // 레이저 발사 100 ~ 360 랜덤하게 720도 회전
    bossCreateLaser.CreateLaser();

    float totalAngle = 0f; // 누적 회전각
    while (totalAngle < 720f)
    {
      float maxPossibleAngle = Mathf.Min(360f, 720f - totalAngle);
      float randomAngle = UnityEngine.Random.Range(100f, maxPossibleAngle);
      int direction = UnityEngine.Random.Range(0, 2) == 0 ? -1 : 1;

      yield return StartCoroutine(bossCreateLaser.RotateLaser(randomAngle * direction, 90f));
      totalAngle += randomAngle;
    }

    bossCreateLaser.DeleteLaser();
  }

  // 페이즈 3 실행
  public IEnumerator Phase3()
  {
    Debug.Log("페이즈 3");

    // 전투 드론 10개 생성
    bossSpawnMonster.SpawnMonster(10);

    // 2박자 파동, 속도 증가
    bossCreateWave.CreateWave(2f, bossStats.waveSpeed * 1.5f);

    // 1박자 낙하물 3번
    bossDropObject.DropObject(player, 1f, 3);

    // 레이저 발사 100 ~ 360 랜덤하게 1080도 회전, 회전 속도 증가
    bossCreateLaser.CreateLaser();

    float totalAngle = 0f; // 누적 회전각
    while (totalAngle < 1080f)
    {
      float maxPossibleAngle = Mathf.Min(360f, 1080f - totalAngle);
      float randomAngle = UnityEngine.Random.Range(100f, maxPossibleAngle);
      int direction = UnityEngine.Random.Range(0, 2) == 0 ? -1 : 1;

      yield return StartCoroutine(bossCreateLaser.RotateLaser(randomAngle * direction, 150f));
      totalAngle += randomAngle;
    }

    bossCreateLaser.DeleteLaser();
  }
}
