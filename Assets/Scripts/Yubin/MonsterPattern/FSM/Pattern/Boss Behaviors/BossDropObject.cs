using System.Collections.Generic;
using System.Collections;
using UnityEngine;

// 낙하물 생성
public class BossDropObject : MonoBehaviour
{
  BossStats bossStats; // 보스 능력치
  BossPattern bossPattern;
  List<GameObject> currentObjects;
  List<GameObject> currentMarks;

  // 초기화
  public void Initialize()
  {
    bossPattern = GetComponent<BossPattern>();
    bossStats = bossPattern.bossStats;
    currentObjects = new List<GameObject>();
    currentMarks = new List<GameObject>();
  }

  // 낙하물 파괴 (보스 사망)
  public void DeleteDropObject()
  {
    foreach(GameObject current in currentObjects)
    {
      Destroy(current);
    }

    foreach(GameObject current in currentMarks)
    {
      Destroy(current);
    }
  }

  // 낙하물 생성
  public void DropObject(List<Transform> player, float dropRate, int n)
  {
    foreach(Transform current in player)
    {
      StartCoroutine(Drop(current, dropRate, n));
    }
  }

  // 낙하물 생성 코루틴
  // 예고 위치 표시 -> 떨구기
  private IEnumerator Drop(Transform player, float dropRate, int n)
  {
    for(int i = 0; i < n; i++)
    {
    // 예고 위치 표시 후 파괴
    Vector3 markPosition = new Vector3(player.position.x, player.position.y, player.position.z);
    GameObject mark = Instantiate(bossStats.dropObjectMark, markPosition, Quaternion.identity);

    currentMarks.Add(mark);

    yield return new WaitForSeconds(dropRate);
    Destroy(mark);

    // 플레이어 머리 위에 낙하물 생성
    Vector3 dropPosition = new Vector3(markPosition.x, markPosition.y + 30f, markPosition.z);
    GameObject dropObject = Instantiate(bossStats.dropObject, dropPosition, Quaternion.identity);

    currentObjects.Add(dropObject);

    Rigidbody rb = dropObject.GetComponent<Rigidbody>();
    if (rb != null)
    {
      rb.useGravity = true;
    }
    
    // 5초 후 파괴
    yield return new WaitForSeconds(3f);
    Destroy(dropObject);
    }
  }
}
