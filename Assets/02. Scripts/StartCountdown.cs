using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class StartCountdown : MonoBehaviour
{
    public static bool canClick {get; private set;}
    [SerializeField]private float countdown = 3f;
    [SerializeField]private TextMeshProUGUI countdownText;
    [SerializeField]private Timer timer;
    private bool isRunning = false;

    private void Start() // 게임 시작
    {
        canClick = false;
        countdownStart();
    }

    public void Update() // 매 프레임마다 실행
    {
        if(isRunning)
        {
            if (countdown <= 0)
            {
                countdownPause();
            }
            countdown -= Time.deltaTime;

            int second = Mathf.FloorToInt(countdown % 60);

            countdownText.text = $"{second}";
            
        }
    }
    private void countdownStart() => isRunning = true; // 카운트 다운 시작
    private void countdownPause() // 카운트 다운 끝
    {
        isRunning = false;

        timer.countdownEnd();
        countdownText.text = "Start!";
        canClick =true;

        Destroy(countdownText.gameObject);
    }
}
