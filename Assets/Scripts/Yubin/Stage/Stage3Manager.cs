using UnityEngine;

public class Stage3Manager : MonoBehaviour
{
  [Header("보스")] public GameObject boss;

  void Update()
  {
    if(boss == null)
    {
      Debug.Log("클리어");
    }
  }
}
