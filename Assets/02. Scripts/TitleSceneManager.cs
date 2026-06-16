using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

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

    // ★ [치트키] 서버에 물어보기도 전에 가방에 이름을 먼저 콱 박아버립니다!
    // 이렇게 하면 서버가 터지든 말든 인게임에는 무조건 내 이름이 뜹니다.
    PlayerSession.UserId = nickname; 
    Debug.Log($"[로그] 버튼 클릭 즉시 가방 저장 완료: {PlayerSession.UserId}");

    if (statusText != null) statusText.text = "서버 접속 중...";

    // 그 다음 서버 통신을 보냅니다.
    ServerManagement.Instance.Register(nickname, DummyPassword, (regSuccess, regMsg) =>
    {
        ServerManagement.Instance.Login(nickname, DummyPassword, (loginSuccess, loginMsg) =>
        {
            SceneManager.LoadScene("AM_MainScene");
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
        statusText.text = "";
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
// [안전장치] 부모 오브젝트가 비어있으면 중단
    if (leaderboardContent == null)
    {
        Debug.LogError("[🚨 에러] 'Leaderboard Content' 칸이 비어있습니다!");
        return;
    }

    if (leaderboardRowPrefab == null)
    {
        Debug.LogError("[🚨 에러] 'Leaderboard Row Prefab' 칸이 비어있습니다!");
        return;
    }

    // 🔥 [버그 박멸 핵심 코스] 기존 랭킹 리스트 완벽 청소
    // 자식 오브젝트가 0개가 될 때까지 맨 위에 있는 녀석(0번)을 무조건 즉시 파괴합니다.
    // 이 방식을 쓰면 인덱스가 밀려서 안 지워지는 UI가 절대 생기지 않습니다.
    while (leaderboardContent.childCount > 0)
    {
        DestroyImmediate(leaderboardContent.GetChild(0).gameObject);
    }
 
    // 데이터가 없는 경우 처리
    if (skiers == null || skiers.Count == 0)
    {
        if (statusText != null) statusText.text = "No LeaderBoard";
        return;
    }

    if (statusText != null) statusText.text = "Update LeaderBoard";

    // 3. 새 랭킹 목록 깔끔하게 다시 그리기
    for (int i = 0; i < skiers.Count; i++)
    {
        TextMeshProUGUI row = Instantiate(leaderboardRowPrefab, leaderboardContent);
        row.text = $"{i + 1}.  {skiers[i].userId}  -  {skiers[i].highScore} point";
    };
        }
    }
