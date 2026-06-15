using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
 
// =========================
// UI 연동 예시
// 회원가입/로그인 입력창, 버튼, 결과 텍스트, 리더보드 영역을
// 인스펙터에서 연결한 뒤 버튼 OnClick에 아래 함수들을 연결하세요.
// =========================
public class ExampleUsage : MonoBehaviour
{
    [Header("회원가입 / 로그인 입력")]
    public InputField userIdField;
    public InputField passwordField;
    public TextMeshProUGUI statusText;
 
    [Header("리더보드 표시")]
    public Transform leaderboardContent;   // 세로 Layout Group이 있는 부모 오브젝트
    public TextMeshProUGUI leaderboardRowPrefab;      // 한 줄짜리 Text 프리팹
 
    // ===== 회원가입 버튼 =====
    public void OnClickRegister()
    {
        ServerManagement.Instance.Register(userIdField.text, passwordField.text, (success, message) =>
        {
            statusText.text = message;
        });
    }
 
    // ===== 로그인 버튼 =====
    public void OnClickLogin()
    {
        ServerManagement.Instance.Login(userIdField.text, passwordField.text, (success, message) =>
        {
            statusText.text = message;
 
            if (success)
            {
                Debug.Log("로그인 성공! 이제 점수 제출 및 리더보드 조회가 가능합니다.");
            }
        });
    }
 
    // ===== 게임이 끝났을 때 호출 (예: finalScore = 1234) =====
    public void SubmitFinalScore(int finalScore)
    {
        ServerManagement.Instance.SubmitScore(finalScore, (success, message) =>
        {
            statusText.text = message;
        });
    }
 
    // ===== 리더보드 새로고침 버튼 =====
    public void OnClickRefreshLeaderboard()
    {
        ServerManagement.Instance.GetLeaderboard(
            skiers => DrawLeaderboard(skiers),
            error => statusText.text = "리더보드 로드 실패: " + error
        );
    }
 
    private void DrawLeaderboard(List<LeaderboardEntry> skiers)
    {
        // 기존 항목 제거
        foreach (Transform child in leaderboardContent)
        {
            Destroy(child.gameObject);
        }
 
        // 새 항목 생성 (highScore 내림차순으로 이미 정렬되어 옴)
        for (int i = 0; i < skiers.Count; i++)
        {
            TextMeshProUGUI row = Instantiate(leaderboardRowPrefab, leaderboardContent);
            row.text = $"{i + 1}위  {skiers[i].userId}  -  {skiers[i].highScore}점";
        }
    }
}