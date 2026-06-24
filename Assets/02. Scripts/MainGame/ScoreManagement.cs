using TMPro;
using UnityEngine;

public class ScoreManagement : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI text;
    [SerializeField] public int targetScore = 100; 
    [SerializeField] private int missScore = 50; 
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
        text.text = $"Score: {currentScore}"; 
    }
    
    public void ScoreRaised()
    {
        currentScore += targetScore; 
        UpdateUI();
    }
    
    public void Scoredown()
    {
        currentScore -= missScore; 
        UpdateUI();
    }

    // 외부에서 최종 점수를 안전하게 읽어갈 수 있도록 함
    public int GetFinalScore()
    {
        return currentScore;
    }
}