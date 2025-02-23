using System.Collections;
using UnityEngine;

// 레이저 생성, 회전
public class BossCreateLaser : MonoBehaviour
{

  GameObject laser;
  GameObject pivot;
  BossStats bossStats; // 보스 능력치
  BossPattern bossPattern;
  float laserLength;

  // 초기화
  public void Initialize()
  {
    bossPattern = GetComponent<BossPattern>();
    bossStats = bossPattern.bossStats;
    laserLength = bossStats.laser.transform.localScale.z;
  }

  // 레이저 생성
  public void CreateLaser()
  {
    if (laser == null) laser = Instantiate(bossStats.laser, transform.position, Quaternion.identity);
    if (pivot == null) pivot = new GameObject("LaserPivot");

    pivot.transform.position = transform.position;
    laser.transform.SetParent(pivot.transform);
    laser.transform.position = new Vector3(pivot.transform.position.x, pivot.transform.position.y, laserLength / 2f);
  }

  // 레이저 삭제
  public void DeleteLaser()
  {
    Debug.Log("레이저 파괴");

    if(laser != null) Destroy(laser);
    if(pivot != null) Destroy(pivot);
  }

  // 회전
  public IEnumerator RotateLaser(float targetAngle, float speed)
  {
    Debug.Log("레이저 회전");

    float rotatedAngle = 0f;
    float rotationStep = speed * Time.deltaTime;
    float direction = Mathf.Sign(targetAngle);

    while(Mathf.Abs(rotatedAngle) < Mathf.Abs(targetAngle) && laser != null && pivot != null)
    {
      float step = Mathf.Min(rotationStep, Mathf.Abs(targetAngle) - Mathf.Abs(rotatedAngle));
      pivot.transform.Rotate(Vector3.up * direction, step);

      rotatedAngle += step * direction; 
      yield return null;
    }
  }
}
