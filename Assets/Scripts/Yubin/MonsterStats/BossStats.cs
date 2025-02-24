using UnityEngine;

[CreateAssetMenu(fileName = "BossStats", menuName = "ScriptableObjects/BossStats", order = 2)]
public class BossStats : ScriptableObject
{
  [Header("체력")]
  public float health = 15000f;          // 체력

  [Header("소환 몬스터")]
  public GameObject monster;    // 소환 몬스터

  [Header("파동")]
  public GameObject wave;       // 파동
  public float waveSpeed = 20f;       // 파동 속도

  [Header("레이저")]
  public GameObject laser;      // 레이저

  [Header("낙하물")]
  public GameObject dropObject; // 낙하물
  public GameObject dropObjectMark; // 낙하물 표시

  // 스크립터블 오브젝트 초기화
  public void Initialize()
  {
    health = 15000f;
    waveSpeed = 40f;
  }
}
