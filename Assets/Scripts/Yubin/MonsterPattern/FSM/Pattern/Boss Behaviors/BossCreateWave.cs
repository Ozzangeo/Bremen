using System.Collections.Generic;
using System.Collections;
using UnityEngine;

public class BossCreateWave : MonoBehaviour
{
  [Header("파동 최대 크기")] public float maxScale = 75; // 파동 최대 크기

  BossStats bossStats;    // 보스 능력치
  GameObject wavePrefab;  // 파동
  bool isSpawning = false;
  private Coroutine waveCoroutine;
  GameObject currentWave;

  // 초기화
  public void Initialize()
  {
    bossStats = GetComponent<BossPattern>().bossStats;
    wavePrefab = bossStats.wave;
  }

  // 파동 파괴 (보스 사망)
  public void DeleteWave()
  {
    StopWave();
    Destroy(currentWave);
  }

  // 파동 생성
  public void CreateWave(float createRate, float waveSpeed)
  {
    if(!isSpawning)
    {
      isSpawning = true;
      waveCoroutine = StartCoroutine(WaveLoop(createRate, waveSpeed));
    }
  }

  // 파동 중지
  public void StopWave()
  {
    isSpawning = false;
    if (waveCoroutine != null)
    {
      StopCoroutine(waveCoroutine);
      waveCoroutine = null;
    }
  }

  // 쿨타임마다 파동 생성
  private IEnumerator WaveLoop(float createRate, float waveSpeed)
  {
    while (isSpawning)
    {
      yield return new WaitForSeconds(createRate);
      StartCoroutine(Wave(waveSpeed));
    }
  }

  // 파동 코루틴
  private IEnumerator Wave(float waveSpeed)
  {
    Debug.Log("파동 생성");

    Vector3 spawnVector = new Vector3(transform.position.x, transform.position.y + 1f, transform.position.z);
    currentWave = Instantiate(wavePrefab, spawnVector, Quaternion.identity);
    currentWave.transform.localScale = new Vector3(currentWave.transform.position.x, 20f, currentWave.transform.position.z);
    MonsterAttackPlayer monsterAttackPlayer = currentWave.GetComponent<MonsterAttackPlayer>();
    monsterAttackPlayer.Initialize(10f);
    
    while(currentWave.transform.localScale.x <= maxScale * 10)
    {
      float scaleIncress = waveSpeed * 2.5f * Time.deltaTime;
      currentWave.transform.localScale += new Vector3(scaleIncress, 0, scaleIncress);
      yield return null;
    }

    Destroy(currentWave);
  }
}
