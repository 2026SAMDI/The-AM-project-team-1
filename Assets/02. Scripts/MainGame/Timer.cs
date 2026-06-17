using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using UnityEngine.UI; // 버튼 UI 제어를 위해 추가

public class Timer : MonoBehaviour
{
    [Header("--- 기존 인게임 UI ---")]
    [SerializeField] private TextMeshProUGUI text;
    private float inGameTimer = 60f; // 1분 = 60초
    private bool isRunning = false;

    [Header("--- 시스템 연결 ---")]
    [SerializeField] private ScoreManagement scoreManagement; 
    [SerializeField] private AccuracySystem accuracySystem; // 정확도 시스템 연결

    [Header("--- 게임오버 결과창 UI ---")]
    [SerializeField] private GameObject resultPanel; //결과창 패널 오브젝트
    [SerializeField] private TextMeshProUGUI finalScoreText; //결과창 점수 텍스트
    [SerializeField] private TextMeshProUGUI finalAccuracyText; //결과창 정확도 텍스트
    [SerializeField] private Button lobbyButton; //로비로 가기 버튼

    private void Start()
    {
        // 게임 시작할 때는 결과창과 버튼을 확실히 꺼두거나 세팅합니다.
        if (resultPanel != null) resultPanel.SetActive(false);
        if (lobbyButton != null)
        {
            lobbyButton.onClick.RemoveAllListeners();
            lobbyButton.onClick.AddListener(LoadTitleScene);
            lobbyButton.interactable = false; // 서버 저장 전까지 버튼 클릭 방지
        }
    }

    public void countdownEnd() 
    {
        startTimer(); 
    }

    private void Update()
    {
        if(isRunning)
        {
            if (inGameTimer <= 0)
            {
                pauseTimer();
                return; 
            }
            inGameTimer -= Time.deltaTime; 

            int minute = Mathf.FloorToInt(inGameTimer / 60f); 
            int second = Mathf.FloorToInt(inGameTimer % 60f); 

            text.text = string.Format("{0:0}:{1:00}", minute, second);
        }
    }
    
    public void startTimer() => isRunning = true; 
    
    public void pauseTimer() 
    {
        isRunning = false;
        text.text = "0:00";

        // 타이머가 끝나면 결과창을 보여주고 서버에 저장
        ShowResultAndSubmit();
    }

    private void ShowResultAndSubmit()
    {
        //점수와 정확도 데이터 긁어오기
        int finalScore = scoreManagement.GetFinalScore();
        float finalAccuracy = accuracySystem.accuracyFormula(); // AccuracySystem의 함수 호출
        string myNickname = PlayerPrefs.GetString("PlayerNickname", "Player");

        Debug.Log($"[게임 종료] {myNickname} - 점수: {finalScore} / 정확도: {finalAccuracy:F1}%");

        // 결과창 패널을 화면에 띄움
        if (resultPanel != null) resultPanel.SetActive(true);

        // 결과창 텍스트에 최종 데이터를 출력
        if (finalScoreText != null) finalScoreText.text = $"Final Score: {finalScore} point";
        if (finalAccuracyText != null) finalAccuracyText.text = $"Accuracy: {finalAccuracy:F1} %";

        // 마우스 락 풀기 
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        // 백그라운드에서 서버에 점수 전송하기
        if (ServerManagement.Instance != null)
        {
            string dummyPassword = "defaultPassword123!";
            
            ServerManagement.Instance.Register(myNickname, dummyPassword, (regSuccess, regMsg) =>
            {
                ServerManagement.Instance.Login(myNickname, dummyPassword, (loginSuccess, loginMsg) =>
                {
                    if (loginSuccess)
                    {
                        ServerManagement.Instance.SubmitScore(finalScore, (success, message) =>
                        {
                            if (success) Debug.Log("서버에 점수 등록 완료!");
                            else Debug.LogError($"점수 등록 실패: {message}");

                            // 서버 저장이 완료되면 비로소 로비 이동 버튼을 활성화시켜 줍니다.
                            if (lobbyButton != null) lobbyButton.interactable = true;
                        });
                    }
                    else
                    {
                        // 로그인 실패 시에도 나가기는 가능하게 처리
                        if (lobbyButton != null) lobbyButton.interactable = true;
                    }
                });
            });
        }
        else
        {
            // 서버 매니저가 없어도 나갈 수는 있게 처리
            if (lobbyButton != null) lobbyButton.interactable = true;
        }
    }

    private void LoadTitleScene()
    {
        SceneManager.LoadScene("title"); 
    }

    public void resetTimer() => isRunning = false;
}