using UnityEngine;
using TMPro;

public class Timer : MonoBehaviour
{
    [SerializeField]private TextMeshProUGUI text;
    private float inGameTimer = 60f; // 1분 = 60초
    private bool isRunning = false;

    public void countdownEnd() // Startcountdown의 카운트 다운 끝날때 시작
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
                return; // 중단
            }
            inGameTimer -= Time.deltaTime; // 실제시간 1초 지날때만 1 감소

            int minute = Mathf.FloorToInt(inGameTimer / 60f); // 분 계산
            int second = Mathf.FloorToInt(inGameTimer % 60f); // 초 계산

            text.text = string.Format("{0:0}:{1:00}",minute,second);
        }
    }
    public void startTimer() => isRunning = true; // 타이머 시작
    public void pauseTimer() // 타이머 끝(정지)
    {
        isRunning = false;
        text.text = "0:00";
    }
    public void resetTimer() //  타이머 리셋 (아직 쓸곳이 없네)
    {
        inGameTimer = 60f;
        isRunning = true;
        text.text = "1:00";
    }
}
