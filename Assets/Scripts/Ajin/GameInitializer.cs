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

    // + �� �� ���� �ҷ����� ������
}
