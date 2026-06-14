using TMPro;
using UnityEngine;

public class ScoreManagement : MonoBehaviour
{
    [SerializeField]private TextMeshProUGUI text;
    [SerializeField]private int targetScore = 100; // 타겟 맞출때마다 점수
    [SerializeField]private int missScore = 50; //빗맞출시 점수
    private int currentScore = 0; // 현재 점수

    private void Awake()
    {
        if (text == null)
        {
            Debug.Log("text 연결이 되지 않음");
            return;
        }
    }

    private void UpdateUI()
    {
        text.text = $"Score: {currentScore}"; // TMPtext에 출력 
    }
    public void ScoreRaised()
    {
        currentScore += targetScore; // 맞추면 점수 추가
        UpdateUI();
    }
    public void Scoredown()
    {
        currentScore -= missScore; // 클릭미스시 점수 감소
        UpdateUI();
    }
    
}
