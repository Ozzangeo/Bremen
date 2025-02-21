using System;
using UnityEngine;

// 보스 패턴의 FSM 관리
public enum EBossState { Phase1, Phase2, Phase3 }

public class BossPattern : MonoBehaviour
{
  [Header("보스 능력치")] public BossStats bossStats;
  [Header("보스 상태 표시")] public EBossState bossState = EBossState.Phase1;

  [Header("몬스터 소환")] public BossSpawnMonster bossSpawnMonster;
  [Header("파동 생성")] public BossCreateWave bossCreateWave;

  void Start()
  {
    if(bossSpawnMonster == null) bossSpawnMonster = GetComponent<BossSpawnMonster>();
    if(bossCreateWave == null) bossCreateWave = GetComponent<BossCreateWave>();

    // 초기화
    bossStats.Initialize();
    bossSpawnMonster.Initialize();
    bossCreateWave.Initialize();

    // 페이즈 1
    Phase1();
  }

  // 피격
  public void GetDamage(float damage)
  {
    Debug.Log("피격");
    bossStats.health -= damage;

    EBossState tempState = EBossState.Phase1;
    // 페이즈 2
    if(bossStats.health <= 10000 && bossStats.health > 5000)
    {
      tempState = EBossState.Phase2;
    }
    // 페이즈 3
    else if(bossStats.health <= 5000)
    {
      tempState = EBossState.Phase3;
    }

    // 페이즈 갱신
    if(!tempState.Equals(bossState))
    {
      bossState = tempState;
      UpdateState(bossState);
    }
  }

  // 상태 갱신
  public void UpdateState(EBossState state)
  {
    switch(state)
    {
      case EBossState.Phase2:
        Phase2();
        break;
      case EBossState.Phase3:
        Phase3();
        break;
      default:
        break;
    }
  }

  // 페이즈 1 실행
  public void Phase1()
  {
    Debug.Log("페이즈 1");

    // 전투 드론 4개 생성
    bossSpawnMonster.SpawnMonster(4);

    // 레이저 발사 시계 한바퀴 반시계 반바퀴

    // 4박자 파동
    bossCreateWave.CreateWave(4f, bossStats.waveSpeed);
    
    // 2박자 낙하물 1번
  }

  // 페이즈 2 실행
  public void Phase2()
  {
    Debug.Log("페이즈 2");

    // 전투 드론 7개 생성
    bossSpawnMonster.SpawnMonster(7);
    // 레이저 발사 100 ~ 360 랜덤하게 720도 회전

    // 2박자 파동
    bossCreateWave.CreateWave(2f, bossStats.waveSpeed);

    // 2박자 낙하물 2번
  }

  // 페이즈 3 실행
  public void Phase3()
  {
    Debug.Log("페이즈 3");

    // 전투 드론 10개 생성
    bossSpawnMonster.SpawnMonster(10);
    // 레이저 발사 100 ~ 360 랜덤하게 1080도 회전, 회전 속도 증가

    // 2박자 파동, 속도 증가
    bossCreateWave.CreateWave(2f, bossStats.waveSpeed * 1.5f);

    // 1박자 낙하물 3번
  }
}
