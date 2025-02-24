using System.Collections.Generic;
using UnityEngine;

// 몬스터 스폰
public class BossSpawnMonster : MonoBehaviour
{
  [Header("스폰 범위 지정")]
  public Vector2 spawnRangeX = new Vector2(-5f, 5f); // X 좌표 범위
  public Vector2 spawnRangeZ = new Vector2(-5f, 5f); // Z 좌표 범위
  [HideInInspector] public float spawnY = 0.5f;      // Y좌표 고정

  BossStats bossStats; // 보스 능력치
  BossPattern bossPattern;
  List<GameObject> monsters;

  // 초기화
  public void Initialize()
  {
    bossPattern = GetComponent<BossPattern>();
    bossStats = bossPattern.bossStats;
    monsters = new List<GameObject>();
  }

  // 몬스터 스폰
  public void SpawnMonster(int num)
  {
    for(int i = 0; i < num; i++)
    {
      float RandomX = Random.Range(spawnRangeX.x, spawnRangeX.y);
      float RandomZ = Random.Range(spawnRangeZ.x, spawnRangeZ.y);
      Vector3 spawnPoint = new Vector3(RandomX, spawnY, RandomZ);

      Debug.Log("몬스터 소환");
      GameObject temp = Instantiate(bossStats.monster, spawnPoint, Quaternion.identity);
      monsters.Add(temp);
    }
  }

  // 몬스터 파괴 (보스 사망)
  public void DeleteMonster()
  {
    foreach(GameObject current in monsters)
    {
      Destroy(current);
    }
  }
}
