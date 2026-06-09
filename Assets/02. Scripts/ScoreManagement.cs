using TMPro;
using UnityEngine;

public class ScoreManagement : MonoBehaviour
{
    [SerializeField]private TextMeshProUGUI text;
    [SerializeField]private float targetScore = 100f; // 타겟 맞출때마다 점수
    
    private float currentScore = 0f; // 현재 점수
    private float clickCount = 0f;

    private void Update()
    {
        text.text = $"Score: {currentScore}"; // TMPtext에 출력
    }

    public void Score_Raised() => currentScore += targetScore; // 맞추면 점수 추가
    public void Score_down() => currentScore -= targetScore; // 클릭미스시 점수 감소
    
}
