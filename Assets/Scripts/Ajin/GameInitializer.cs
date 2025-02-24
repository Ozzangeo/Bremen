using UnityEngine;

public class GameInitializer : MonoBehaviour
{ 
    private void Start()
    {
        if(PlayerData.Instance == null)
        {
            GameObject playerData = new GameObject("PlayerData");
            playerData.AddComponent<PlayerData>();
        }
    }

    // + 그 외 저장 불러오기 같은거
}
