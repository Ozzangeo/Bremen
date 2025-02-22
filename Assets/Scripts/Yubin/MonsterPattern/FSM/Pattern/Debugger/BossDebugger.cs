using UnityEngine;

public class BossDebugger : MonoBehaviour
{
  BossSpawnMonster bossSpawnMonster;
  BossCreateWave bossCreateWave;

  void Start()
  {
    bossSpawnMonster =  GetComponent<BossSpawnMonster>();
    bossCreateWave = GetComponent<BossCreateWave>();
  }

  void OnDrawGizmos()
  {
    if(bossSpawnMonster == null) bossSpawnMonster =  GetComponent<BossSpawnMonster>();
    if(bossCreateWave == null) bossCreateWave =  GetComponent<BossCreateWave>();

    // 몬스터 스폰 범위 (빨간색)
    Gizmos.color = Color.red;

    Vector3 center = transform.position + new Vector3((bossSpawnMonster.spawnRangeX.x + bossSpawnMonster.spawnRangeX.y) / 2f, bossSpawnMonster.spawnY, (bossSpawnMonster.spawnRangeZ.x + bossSpawnMonster.spawnRangeZ.y) / 2f);
    Vector3 size = new Vector3(bossSpawnMonster.spawnRangeX.y - bossSpawnMonster.spawnRangeX.x, bossSpawnMonster.spawnY, bossSpawnMonster.spawnRangeZ.y - bossSpawnMonster.spawnRangeZ.x);

    Gizmos.DrawWireCube(center, size);

    // 파동 생성 범위 (초록색)
    Gizmos.color = Color.green;
    Gizmos.DrawWireSphere(transform.position, bossCreateWave.maxScale);
  }
}
