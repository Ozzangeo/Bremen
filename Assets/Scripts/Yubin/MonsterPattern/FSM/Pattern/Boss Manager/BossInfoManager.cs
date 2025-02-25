using Ozi.Weapon.Entity;
using System;
using UnityEngine;

// 보스 정보 관리
public class BossInfoManager : BasicEntityBehaviour
{
  public const int PHASE2_HEALTH = 10_000;
  public const int PHASE3_HEALTH = 5_000;

  BossPattern bossPattern;

  // 초기화
  public void Initialize()
  {
    bossPattern = GetComponent<BossPattern>();
    bossStats = bossPattern.bossStats;

    base.OnHit += OnHit;
    base.OnDead += OnDead;
  }

  private new void OnHit(float damage) {
    var phase_state = EBossState.Phase1;

    // Phase Check
    if (PHASE3_HEALTH < Status.health && Status.health <= PHASE2_HEALTH) {
      phase_state = EBossState.Phase2;
    }
    else if (Status.health <= PHASE3_HEALTH) {
      phase_state = EBossState.Phase3;
    }

    // Update Phase
    if (!phase_state.Equals(bossPattern.bossState)) {
      bossPattern.UpdateState(phase_state);
    }
  }
  private new void OnDead() {
    bossPattern.BossDie();
  }

  // 피격
  [Obsolete]
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
    if(!tempState.Equals(bossPattern.bossState))
    {
      bossPattern.UpdateState(tempState);
    }
  }
}
