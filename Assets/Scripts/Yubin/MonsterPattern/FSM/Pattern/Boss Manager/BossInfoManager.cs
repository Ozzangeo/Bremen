using UnityEngine;

// 보스 정보 관리
public class BossInfoManager : MonoBehaviour
{
  BossStats bossStats; // 보스 능력치
  BossPattern bossPattern;

  // 초기화
  public void Initialize()
  {
    bossPattern = GetComponent<BossPattern>();
    bossStats = bossPattern.bossStats;
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
    if(!tempState.Equals(bossPattern.bossState))
    {
      bossPattern.UpdateState(tempState);
    }
  }
}
