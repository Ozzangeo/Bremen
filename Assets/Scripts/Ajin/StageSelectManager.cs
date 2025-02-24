using Fusion;
using UnityEngine;
using UnityEngine.UI;

public class StageSelectManager : MonoBehaviour
{
    [SerializeField] private Button[] stage_Btn;

    private void Start()
    {
        for(int i = 1 ;i < PlayerData.Instance.isClear.Length ; i ++)
        {
            if (PlayerData.Instance.isClear[i] == false)
            {
                stage_Btn[i].interactable = false;
            }
            else
            {
                stage_Btn[i].interactable = true;
            }
        }
    }

    [Rpc(RpcSources.StateAuthority, RpcTargets.All)]
    public void StartStage_RPC(int stageNumber)
    {
        string sceneName = stageNumber + "_StageScene";
        GameSessionManager.Instance.runner.LoadScene(sceneName);
    }
}
