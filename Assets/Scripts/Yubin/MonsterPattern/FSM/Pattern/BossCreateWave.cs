using System.Collections.Generic;
using System.Collections;
using UnityEngine;

public class BossCreateWave : MonoBehaviour
{
  [Header("파동 최대 크기")] public float maxScale = 25f; // 파동 최대 크기

  BossStats bossStats;    // 보스 능력치
  GameObject wavePrefab;  // 파동
  bool isSpawning = false;
  private Coroutine waveCoroutine;

  // 초기화
  public void Initialize()
  {
    bossStats = GetComponent<BossPattern>().bossStats;
    wavePrefab = bossStats.wave;
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

    GameObject wave = Instantiate(wavePrefab, transform.position, Quaternion.identity);
    while(wave.transform.localScale.x <= maxScale * 2)
    {
      float scaleIncress = waveSpeed  * Time.deltaTime;
      wave.transform.localScale += new Vector3(scaleIncress, 0, scaleIncress);
      yield return null;
    }

    Destroy(wave);
  }
}
