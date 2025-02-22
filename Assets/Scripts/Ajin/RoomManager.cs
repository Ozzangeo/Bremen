using Fusion;
using UnityEngine;
using UnityEngine.UI;

public class RoomManager : MonoBehaviour
{
    [SerializeField] private InputField codeInputField;
    private string _roomCode;

    public void CreateRoomBtn()
    {
        _roomCode = GenerateRoomCode();
        PlayerData.Instance.roomCode = _roomCode;
        Debug.Log($"{_roomCode}");
        GameSessionManager.Instance.EnterRoomWithCode(_roomCode, GameMode.Host);
    }

    public void JoinRoomBtn()
    {
        _roomCode = codeInputField.text;

        if(!string.IsNullOrEmpty(_roomCode) )
        {
            PlayerData.Instance.roomCode = _roomCode;
            Debug.Log($"{_roomCode}");
            GameSessionManager.Instance.EnterRoomWithCode( _roomCode, GameMode.Client);
        }
        else
        {
            Debug.Log($"방 코드 입력 오류");
        }
    }

    private string GenerateRoomCode()
    {
        return UnityEngine.Random.Range(10000, 99999).ToString();
    }
}
