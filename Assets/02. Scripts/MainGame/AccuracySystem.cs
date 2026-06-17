using TMPro;
using UnityEngine;

public class AccuracySystem : MonoBehaviour
{
    [SerializeField]private TextMeshProUGUI accuracyText;
    private float shot;
    private float hit; 
    [SerializeField]private ScoreManagement targetScore;

    private void Start() // 시작
    {
        updateAccuracyText();
    } 
    
    public void addShot() // 총을 발사하면(마우스 클릭) shot을 증가시킴
    {
         shot++;
         updateAccuracyText();
    }
    public void addHit() // 타겟이 맞았다면 hit를 증가시킴
    {
        hit++;
        updateAccuracyText();
    } 

    public float accuracyFormula() // 정확도 공식 사용해서 정확도 반환
    {
        if (shot == 0)
        {
            return 0f;
        }
        return hit/shot * 100f;
    }
    private void updateAccuracyText() // 텍스트 업데이트
    {
        accuracyText.text = $"accuracy : {accuracyFormula():F1}%";
    }
}
