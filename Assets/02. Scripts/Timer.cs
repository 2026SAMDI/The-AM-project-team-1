using UnityEngine;
using TMPro;

public class Timer : MonoBehaviour
{
    [SerializeField]private TextMeshProUGUI text;
    private float inGameTimer = 60f; // 1분 = 60초
    private bool isRunning = false;

    void Start()
    {
        startTimer(); // 타이머 시작
    }

    void Update()
    {
        if(isRunning == true)
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
    public void startTimer(){isRunning = true;}
    public void pauseTimer()
    {
        isRunning = false;
        text.text = "0:00";
    }
    public void resetTimer()
    {
        inGameTimer = 60f;
        isRunning = true;
        text.text = "1:00";
    }
}
