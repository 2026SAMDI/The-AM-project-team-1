using UnityEngine;
using TMPro; // TextMeshPro를 사용하기 위해 추가

public class InGameUiController : MonoBehaviour
{
    // 게임 화면 상단에 플레이어 이름을 보여줄 Text 오브젝트 (인스펙터에서 연결)
    [SerializeField] private TextMeshProUGUI playerNicknameText;

    private void Start()
    {
        // 게임 씬이 시작되자마자 가방(PlayerSession)을 열어 이름을 확인합니다.
        if (PlayerSession.IsLoggedIn)
        {
            // 가방에 있던 이름을 텍스트 UI에 채워 넣습니다!
            playerNicknameText.text = $"Player: {PlayerSession.UserId}";
            Debug.Log($"[인게임] 현재 플레이어는 '{PlayerSession.UserId}'입니다.");
        }
        else
        {
            // 혹시라도 로그인을 안 하고 치트 등으로 바로 들어왔을 때를 위한 예외 처리
            playerNicknameText.text = "Player: Guest";
        }
    }
}