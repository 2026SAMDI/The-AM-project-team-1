using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.Networking;
 
// =========================
// 서버 요청 데이터 (UserRequest.java 대응)
// =========================
[Serializable]
public class UserRequestData
{
    public string userId;
    public string password;
    public int score;
 
    public UserRequestData(string userId, string password, int score = 0)
    {
        this.userId = userId;
        this.password = password;
        this.score = score;
    }
}
 
// =========================
// 리더보드 한 줄 (LeaderboardResponse.java 대응)
// =========================
[Serializable]
public class LeaderboardEntry
{
    public string userId;
    public int highScore;
}
 
// =========================
// 리더보드 전체 응답 (LeaderboardResult.java 대응)
// =========================
[Serializable]
public class LeaderboardResult
{
    public List<LeaderboardEntry> skiers;
}
 
// =========================
// 로그인한 유저 정보 (점수 제출 시 재사용)
// =========================
public static class PlayerSession
{
    public static string UserId;
    public static string Password;
 
    public static bool IsLoggedIn => !string.IsNullOrEmpty(UserId);
 
    public static void Clear()
    {
        UserId = null;
        Password = null;
    }
}
 
// =========================
// 서버 통신 매니저
// 빈 GameObject에 붙여서 씬에 하나만 두고 사용하세요.
// =========================
public class ServerManagement : MonoBehaviour
{
    public static ServerManagement Instance { get; private set; }
 
    [Header("서버 주소 설정")]
    [Tooltip("같은 PC에서 테스트할 땐 http://localhost:8080, " +
             "다른 기기(휴대폰 등)에서 접속할 땐 PC의 IP 주소를 적어주세요. (예: http://192.168.0.10:8080)")]
    [SerializeField] private string serverUrl = "http://localhost:8080";
 
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
 
    // =========================
    // 회원가입 (/register)
    // =========================
    public void Register(string userId, string password, Action<bool, string> onComplete)
    {
        var data = new UserRequestData(userId, password);
        StartCoroutine(Post("/register", data, (ok, body) =>
        {
            // 서버는 "회원가입 성공" / "이미 존재하는 아이디" 같은 순수 문자열을 반환
            onComplete?.Invoke(ok && body == "회원가입 성공", body);
        }));
    }
 
    // =========================
    // 로그인 (/login)
    // =========================
    public void Login(string userId, string password, Action<bool, string> onComplete)
    {
        var data = new UserRequestData(userId, password);
        StartCoroutine(Post("/login", data, (ok, body) =>
        {
            bool success = ok && body == "로그인 성공";
 
            if (success)
            {
                // 점수 제출 시 재사용하기 위해 세션에 저장
                PlayerSession.UserId = userId;
                PlayerSession.Password = password;
            }
 
            onComplete?.Invoke(success, body);
        }));
    }
 
    // =========================
    // 점수 제출 (/submit-score)
    // 서버 구조상 매 요청마다 userId/password로 본인 확인 후 비교/저장합니다.
    // =========================
    public void SubmitScore(int score, Action<bool, string> onComplete)
    {
        if (!PlayerSession.IsLoggedIn)
        {
            onComplete?.Invoke(false, "로그인이 필요합니다");
            return;
        }
 
        var data = new UserRequestData(PlayerSession.UserId, PlayerSession.Password, score);
        StartCoroutine(Post("/submit-score", data, (ok, body) =>
        {
            onComplete?.Invoke(ok && body == "점수 저장 성공", body);
        }));
    }
 
    // =========================
    // 리더보드 조회 (/leaderboard)
    // 서버에서 highScore 내림차순으로 정렬되어 옵니다.
    // =========================
    public void GetLeaderboard(Action<List<LeaderboardEntry>> onComplete, Action<string> onError = null)
    {
        StartCoroutine(Get("/leaderboard", (ok, body) =>
        {
            if (!ok)
            {
                onError?.Invoke(body);
                return;
            }
 
            LeaderboardResult result = JsonUtility.FromJson<LeaderboardResult>(body);
            onComplete?.Invoke(result != null && result.skiers != null ? result.skiers : new List<LeaderboardEntry>());
        }));
    }
 
    // =========================
    // 공통 POST 요청 (application/json)
    // =========================
    private IEnumerator Post(string endpoint, object payload, Action<bool, string> onComplete)
    {
        string json = JsonUtility.ToJson(payload);
        byte[] bodyRaw = Encoding.UTF8.GetBytes(json);
 
        using (UnityWebRequest request = new UnityWebRequest(serverUrl + endpoint, "POST"))
        {
            request.uploadHandler = new UploadHandlerRaw(bodyRaw);
            request.downloadHandler = new DownloadHandlerBuffer();
            request.SetRequestHeader("Content-Type", "application/json; charset=utf-8");
 
            yield return request.SendWebRequest();
 
            if (request.result == UnityWebRequest.Result.Success)
            {
                onComplete?.Invoke(true, request.downloadHandler.text);
            }
            else
            {
                Debug.LogError($"[ServerManager] POST {endpoint} 실패: {request.error}");
                onComplete?.Invoke(false, request.error);
            }
        }
    }
 
    // =========================
    // 공통 GET 요청
    // =========================
    private IEnumerator Get(string endpoint, Action<bool, string> onComplete)
    {
        using (UnityWebRequest request = UnityWebRequest.Get(serverUrl + endpoint))
        {
            yield return request.SendWebRequest();
 
            if (request.result == UnityWebRequest.Result.Success)
            {
                onComplete?.Invoke(true, request.downloadHandler.text);
            }
            else
            {
                Debug.LogError($"[ServerManager] GET {endpoint} 실패: {request.error}");
                onComplete?.Invoke(false, request.error);
            }
        }
    }
}

