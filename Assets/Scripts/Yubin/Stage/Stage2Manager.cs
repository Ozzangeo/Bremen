using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stage2Manager : MonoBehaviour
{
  [Header("몬스터")]
  public GameObject drone;
  public GameObject ear;
  public GameObject eye;
  public GameObject guard;
  public GameObject combatDrone;
  public GameObject EMP;

  [Header("몬스터 스폰 범위")]
  public Vector2 x;
  public Vector2 y;
  public float spawnY;

  [Header("비트 코어")]
  public GameObject bitCore;

  private List<GameObject> spawnedObjects = new List<GameObject>(); // 생성된 오브젝트 리스트
  private int currentSpawnCount = 0; // 현재 생성된 개수
  bool[] isClear = { false, false, false, false, false };
  int currentStage = 1;
  bool isSpawningComplete = false; // 현재 스테이지의 몬스터가 모두 생성되었는지 확인

  private void Start()
  {
    Step1();
  }

  void Update()
  {
    if(bitCore == null)
    {
      Debug.Log("클리어 실패");
    }

    if(Input.GetKeyDown(KeyCode.T))
    {
      DebugStageClear();
    }

    if(isClear[currentStage])
    {
      if(currentStage == 4)
      {
        Debug.Log("클리어 성공");
        return;
      }

      currentStage++;

      switch (currentStage)
      {
        case 2:
          Step2();
          break;
        case 3:
          Step3();
          break;
        case 4:
          Step4();
          break;
        default:
          break;
      }
    }
  }

  public void DebugStageClear()
  {
    Debug.Log($"테스트: 스테이지 {currentStage} 강제 클리어");
    
    isClear[currentStage] = true;
    spawnedObjects.Clear();
    isSpawningComplete = true;
  }

  // 1단계
  private void Step1()
  {
    Debug.Log("1단계 시작");

    float spawnInterval = Random.Range(10, 20);
    int spawnPerCycle = Random.Range(2, 5);
    StartCoroutine(SpawnObjects(drone, 30, spawnInterval, spawnPerCycle));

    spawnInterval = Random.Range(5, 12);
    spawnPerCycle = Random.Range(1, 3);
    StartCoroutine(SpawnObjects(ear, 30, spawnInterval, spawnPerCycle));

    StartCoroutine(CheckStageClear());
  }

  // 2단계
  private void Step2()
  {
    if (isClear[2]) return;

    Debug.Log("2단계 시작");

    float spawnInterval = Random.Range(10, 20);
    int spawnPerCycle = Random.Range(2, 5);
    StartCoroutine(SpawnObjects(drone, 25, spawnInterval, spawnPerCycle));

    spawnInterval = Random.Range(5, 12);
    spawnPerCycle = Random.Range(1, 3);
    StartCoroutine(SpawnObjects(ear, 23, spawnInterval, spawnPerCycle));

    spawnInterval = Random.Range(10, 15);
    spawnPerCycle = Random.Range(1, 3);
    StartCoroutine(SpawnObjects(eye, 17, spawnInterval, spawnPerCycle));

    spawnInterval = Random.Range(15, 25);
    StartCoroutine(SpawnObjects(guard, 10, spawnInterval, 2));

    StartCoroutine(CheckStageClear());
  }

  // 3단계
  private void Step3()
  {
    if (isClear[3]) return;

    Debug.Log("3단계 시작");

    float spawnInterval = Random.Range(5, 12);
    int spawnPerCycle = Random.Range(1, 3);
    StartCoroutine(SpawnObjects(ear, 17, spawnInterval, spawnPerCycle));

    spawnInterval = Random.Range(10, 15);
    spawnPerCycle = Random.Range(1, 3);
    StartCoroutine(SpawnObjects(eye, 17, spawnInterval, spawnPerCycle));

    spawnInterval = Random.Range(10, 20);
    spawnPerCycle = Random.Range(2, 4);
    StartCoroutine(SpawnObjects(combatDrone, 24, spawnInterval, spawnPerCycle));

    spawnInterval = Random.Range(15, 25);
    spawnPerCycle = Random.Range(1, 2);
    StartCoroutine(SpawnObjects(guard, 14, spawnInterval, spawnPerCycle));

    StartCoroutine(CheckStageClear());
  }

  // 4단계
  private void Step4()
  {
    if (isClear[4]) return;

    Debug.Log("4단계 시작");

    float spawnInterval = Random.Range(5, 12);
    int spawnPerCycle = Random.Range(1, 3);
    StartCoroutine(SpawnObjects(ear, 17, spawnInterval, spawnPerCycle));

    spawnInterval = Random.Range(10, 15);
    spawnPerCycle = Random.Range(1, 3);
    StartCoroutine(SpawnObjects(eye, 17, spawnInterval, spawnPerCycle));

    spawnInterval = Random.Range(10, 20);
    spawnPerCycle = Random.Range(2, 4);
    StartCoroutine(SpawnObjects(combatDrone, 24, spawnInterval, spawnPerCycle));

    spawnInterval = Random.Range(15, 25);
    spawnPerCycle = Random.Range(1, 2);
    StartCoroutine(SpawnObjects(guard, 14, spawnInterval, spawnPerCycle));

    StartCoroutine(SpawnObjects(EMP, 8, 0, 8));

    StartCoroutine(CheckStageClear());
  }

  private IEnumerator SpawnObjects(GameObject spawnPrefab, int maxSpawnCount, float spawnInterval, int spawnPerCycle)
  {
    while(currentSpawnCount < maxSpawnCount)
    {
      for(int i = 0; i < spawnPerCycle; i++)
      {
        if(currentSpawnCount >= maxSpawnCount)
        {
          isSpawningComplete = true;
          yield break;
        }

        GameObject newObj = Instantiate(spawnPrefab, GetRandomPosition(), Quaternion.identity);
        spawnedObjects.Add(newObj);
        currentSpawnCount++;
      }

      yield return new WaitForSeconds(spawnInterval);
    }

    isSpawningComplete = true;
  }

  private IEnumerator CheckStageClear()
  {
    yield return new WaitUntil(() => isSpawningComplete);

    while(spawnedObjects.Count > 0)
    {
      spawnedObjects.RemoveAll(item => item == null);
      yield return new WaitForSeconds(1f);
    }

    Debug.Log("스테이지 클리어!");
    isClear[currentStage] = true;
  }

  private Vector3 GetRandomPosition()
  {
    float X = Random.Range(x.x, x.y);
    float Z = Random.Range(y.x, y.y);
    return new Vector3(X, spawnY, Z);
  }

  void OnDrawGizmos()
  {
    Gizmos.color = Color.red;

    Vector3 center = new Vector3((x.x + x.y) / 2f, spawnY, (y.x + y.y) / 2f);
    Vector3 size = new Vector3(x.y - x.x, 0.1f, y.y - y.x);

    Gizmos.DrawWireCube(center, size);
  }
}
