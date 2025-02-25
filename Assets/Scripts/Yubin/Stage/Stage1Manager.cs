using UnityEngine;
using System.Collections.Generic;

public class Stage1Manager : MonoBehaviour
{
  public Vector3 center;
  public float radius;
  
  private List<Transform> players = new List<Transform>();

  void Start()
  {
    FindPlayers();
  }

  void Update()
  {
    if(AllPlayersInRange())
    {
      StageClear();
    }
  }

  void FindPlayers()
  {
    players.Clear();
    players = new List<Transform>();

    GameObject[] allObjects = Resources.FindObjectsOfTypeAll<GameObject>();

    foreach(GameObject obj in allObjects)
    {
      if(obj.name == "Player" && obj.activeInHierarchy)
      {
        players.Add(obj.transform);
      }
    }
  }

  bool AllPlayersInRange()
  {
    foreach(Transform player in players)
    {
      if(Vector3.Distance(player.transform.position, center) > radius)
      {
        return false;
      }
    }
    return true;
  }

  void StageClear()
  {
    Debug.Log("경 스테이지 클리어 축");
  }
  
  void OnDrawGizmos()
  {
    Gizmos.color = Color.green;
    Gizmos.DrawWireSphere(center, radius);
  }
}
