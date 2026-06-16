// using System.Collections.Generic;
// using TMPro;
// using UnityEngine;
// using UnityEngine.UI;
 
// // =========================
// // UI 연동 예시
// // 회원가입/로그인 입력창, 버튼, 결과 텍스트, 리더보드 영역을
// // 인스펙터에서 연결한 뒤 버튼 OnClick에 아래 함수들을 연결하세요.
// // =========================
// public class ExampleUsage : MonoBehaviour
// {
//     [Header("회원가입 / 로그인 입력")]
//     public TMP_InputField userIdField;
//     public TMP_InputField passwordField;
//     public TextMeshProUGUI statusText;
 
//     [Header("리더보드 표시")]
//     public Transform leaderboardContent;   // 세로 Layout Group이 있는 부모 오브젝트
//     public TextMeshProUGUI leaderboardRowPrefab;      // 한 줄짜리 Text 프리팹
 
//     // ===== 회원가입 버튼 =====
//     public void OnClickRegister()
//     {
//         ServerManagement.Instance.Register(userIdField.text, passwordField.text, (success, message) =>
//         {
//             statusText.text = message;
//         });
//     }
 
//     // ===== 로그인 버튼 =====
//     public void OnClickLogin()
//     {
//         ServerManagement.Instance.Login(userIdField.text, passwordField.text, (success, message) =>
//         {
//             statusText.text = message;
 
//             if (success)
//             {
//                 Debug.Log("로그인 성공! 이제 점수 제출 및 리더보드 조회가 가능합니다.");
//             }
//         });
//     }
 
//     // ===== 게임이 끝났을 때 호출 (예: finalScore = 1234) =====
//     public void SubmitFinalScore(int finalScore)
//     {
//         ServerManagement.Instance.SubmitScore(finalScore, (success, message) =>
//         {
//             statusText.text = message;
//         });
//     }
 
//     // ===== 리더보드 새로고침 버튼 =====
//     public void OnClickRefreshLeaderboard()
//     {
//         ServerManagement.Instance.GetLeaderboard(
//             skiers => DrawLeaderboard(skiers),
//             error => statusText.text = "리더보드 로드 실패: " + error
//         );
//     }
 
//     private void DrawLeaderboard(List<LeaderboardEntry> skiers)
//     {
//         // 기존 항목 제거
//         foreach (Transform child in leaderboardContent)
//         {
//             Destroy(child.gameObject);
//         }
 
//         // 새 항목 생성 (highScore 내림차순으로 이미 정렬되어 옴)
//         for (int i = 0; i < skiers.Count; i++)
//         {
//             TextMeshProUGUI row = Instantiate(leaderboardRowPrefab, leaderboardContent);
//             row.text = $"{i + 1}위  {skiers[i].userId}  -  {skiers[i].highScore}점";
//         }
//     }
// }
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TitleSceneManager : MonoBehaviour
{
    [Header("--- 패널 오브젝트 설정 ---")]
    [SerializeField] private GameObject mainMenuPanel;     // 1. 메인 메뉴 패널 (첫 화면)
    [SerializeField] private GameObject leaderboardPanel;  // 2. 리더보드 패널

    [Header("--- 닉네임 입력 UI ---")]
    [SerializeField] private TMP_InputField nicknameField; // [수정] 이름만 받는 인풋필드
    [SerializeField] private TextMeshProUGUI statusText;   // 안내 메시지용

    [Header("--- 리더보드 UI (이름 + 점수 표시) ---")]
    [SerializeField] private Transform leaderboardContent;         // 세로 Layout Group 부모
    [SerializeField] private TextMeshProUGUI leaderboardRowPrefab; // 한 줄짜리 Text 프리팹

    // 내부적으로 서버 인증을 통과하기 위한 가짜 비밀번호 고정값
    private const string DummyPassword = "DefaultPassword!!!";

    private void Start()
    {
        // 게임을 켜면 메인 메뉴판(입력창이 있는 곳)을 바로 보여줍니다.
        mainMenuPanel.SetActive(true);
        leaderboardPanel.SetActive(false); 
    }

    // =================================================================
    // 📢 BUTTON EVENTS (유니티 버튼 UI에 연결하는 함수들)
    // =================================================================

    // ===== [게임 시작] 버튼 누를 때 =====
    public void OnClickStartGame()
    {
        string nickname = nicknameField.text.Trim();

        if (string.IsNullOrWhiteSpace(nickname))
        {
            if (statusText != null) statusText.text = "이름을 입력해야 시작할 수 있습니다.";
            return;
        }

        if (statusText != null) statusText.text = "Server Connecting...";

        // [우회 로직] 
        // 1단계: 먼저 이 이름으로 서버에 회원가입을 찔러봅니다.
        ServerManagement.Instance.Register(nickname, DummyPassword, (regSuccess, regMsg) =>
        {
            // 가입 성공했거나, 이미 존재하는 이름이라도 상관없이 바로 2단계 로그인을 시도합니다.
            ServerManagement.Instance.Login(nickname, DummyPassword, (loginSuccess, loginMsg) =>
            {
                if (loginSuccess)
                {
                    Debug.Log($"[로그] '{nickname}'님 환영합니다. 게임 씬으로 이동!");
                    SceneManager.LoadScene("AM_MainScene");
                }
                else
                {
                    if (statusText != null) statusText.text = "Connecting failed : " + loginMsg;
                }
            });
        });
    }

    // ===== [리더보드 보기] 버튼 누를 때 =====
    public void OnClickOpenLeaderboard()
    {
        mainMenuPanel.SetActive(false);
        leaderboardPanel.SetActive(true);
        RefreshLeaderboardData(); // 열릴 때 자동으로 최신 순위 로드
    }

    // ===== 리더보드 창의 [닫기] 버튼 누를 때 =====
    public void OnClickCloseLeaderboard()
    {
        leaderboardPanel.SetActive(false);
        mainMenuPanel.SetActive(true); 
    }
 
    // ===== 리더보드 창의 [새로고침] 버튼 누를 때 =====
    public void OnClickRefreshButton()
    {
        RefreshLeaderboardData();
    }


    // =================================================================
    // 🔒 PRIVATE METHODS (내부 로직)
    // =================================================================
 
    private void RefreshLeaderboardData()
    {
if (statusText != null) 
    {
        statusText.text = "Loading LeaderBoard";
    }
    else
    {
        Debug.Log("[로그] statusText가 연결되지 않았지만 리더보드를 계속 로드합니다.");
    }
    
    // 2. ServerManagement가 실제로 씬에 있는지 최종 검문
    if (ServerManagement.Instance == null)
    {
        Debug.LogError("[🚨 에러] 씬에 ServerManagement 오브젝트가 없습니다! 하이어라키 창을 확인하세요.");
        return;
    }

    ServerManagement.Instance.GetLeaderboard(
        skiers => DrawLeaderboard(skiers),
        error => 
        { 
            // 여기도 안전장치 추가
            if (statusText != null) statusText.text = "LoadFailed: " + error; 
            Debug.LogError("리더보드 로드 실패: " + error);
        });
    }
 
    // 서버에서 온 데이터 리스트를 받아와 이름과 점수를 세트로 화면에 띄우는 코드입니다.
    private void DrawLeaderboard(List<LeaderboardEntry> skiers)
    {
        // 기존 랭킹 UI들 싹 청소
        foreach (Transform child in leaderboardContent)
        {
            child.SetParent(null);
            Destroy(child.gameObject);
        }
 
        if (skiers == null || skiers.Count == 0)
        {
            if (statusText != null) statusText.text = "No Ranking";
            return;
        }

        if (statusText != null) statusText.text = "Reroll LeaderBoard";

        // [이름 + 점수 세트로 띄우기]
        // 서버에서 받아온 skiers 배열을 돌면서 등수, 유저ID(이름), 하이스코어 점수를 한 줄로 조합해 찍어줍니다.
        for (int i = 0; i < skiers.Count; i++)
        {
            TextMeshProUGUI row = Instantiate(leaderboardRowPrefab, leaderboardContent);
            
            // 문자열 포맷을 활용하여 깔끔하게 이름과 점수를 매칭합니다.
            row.text = $"{i + 1}.   {skiers[i].userId}   ({skiers[i].highScore}p)";
        }
    }
}