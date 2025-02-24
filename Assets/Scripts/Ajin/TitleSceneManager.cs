using UnityEngine;
using UnityEngine.UI;

public class TitleSceneManager : MonoBehaviour
{
    [SerializeField] private GameObject nameSettingPanel;
    [SerializeField] private InputField setNameInput;
    [SerializeField] private InputField reNameInput;

    private void Start()
    {
        nameSettingPanel.SetActive(true);   
    }

    public void ReName()
    {
        if(reNameInput.text != null)
        {
            PlayerData.Instance.playerName = reNameInput.text;
            Debug.Log("이름설정완료");
        }
        else
        {
            Debug.Log("이름없음 오류");
        }
    }

    public void SetName()
    {
        if(setNameInput.text != null)
        {
            PlayerData.Instance.playerName = setNameInput.text;
            nameSettingPanel.gameObject.SetActive(false);
            Debug.Log("이름설정완료");

        }
        else
        {
            Debug.Log("이름없음 오류");
        }
    }
}
