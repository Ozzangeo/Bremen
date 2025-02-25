using UnityEngine;

public class AnimationHelper : MonoBehaviour
{
  public float offset;

  void LateUpdate()
  {
    Vector3 pos = transform.position;
    pos.y = offset; // 원하는 Y 값으로 고정
    transform.position = pos;
  }
}
