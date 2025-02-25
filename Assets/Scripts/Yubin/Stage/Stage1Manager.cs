using UnityEngine;
using System.Collections.Generic;

public class Stage1Manager : MonoBehaviour
{
  public Vector3 center;
  public float radius;
  
  private List<Transform> players = new List<Transform>();

  private void Awake()
  {
    GameSessionManager.Instance.EnterGame();
    Debug.Log("StageManager");
  }

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

    GameObject[] playerObjects = GameObject.FindGameObjectsWithTag("Player");

    foreach (GameObject obj in playerObjects)
    {
      players.Add(obj.transform);
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
    Debug.Log("클리어 성공");
  }
  
  void OnDrawGizmos()
  {
    Gizmos.color = Color.green;
    Gizmos.DrawWireSphere(center, radius);
  }
}
