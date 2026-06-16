using UnityEngine;
using TMPro; 

public class InGameUiController : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI playerNicknameText;

    private void Start()
    {
        // MenuManager가 저장한 "PlayerNickname" 키값과 똑같이 맞춰서 꺼내옴
        string savedNickname = PlayerPrefs.GetString("PlayerNickname", "Guest");

        Debug.Log($"[디버그] 하드디스크에서 불러온 이름: '{savedNickname}'");

        if (playerNicknameText != null)
        {
            playerNicknameText.text = $"Player: {savedNickname}";
        }
    }
}