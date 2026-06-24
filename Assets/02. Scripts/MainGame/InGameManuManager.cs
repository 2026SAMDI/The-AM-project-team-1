using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class InGameManuManager : MonoBehaviour
{
    [Header ("감도 UI")]
    [SerializeField]private Slider sensitivitySlider;
    [SerializeField]private CharacterMovement playerMovement;
    [SerializeField] private TMP_Text sensitivityText;
    [SerializeField] private TMP_Text currentDifficultyText;

    private void Start()
    {
        float savedSensitivity = PlayerPrefs.GetFloat("Sensitivity",0.5f);

        if(sensitivitySlider != null )
        {
            sensitivitySlider.value = savedSensitivity;
            sensitivitySlider.onValueChanged.AddListener(OnSensitivityChanged);
            UpdateSensitivityText(savedSensitivity);
        }

        if (!PlayerPrefs.HasKey("Difficulty"))
        {
            SetDifficulty("Easy");
        }
    }

    private void UpdateSensitivityText(float value)
    {
        if (sensitivityText != null)
        {
            sensitivityText.text = value.ToString("F2");
        }
    }

    public void OnSensitivityChanged(float value)
    {
        PlayerPrefs.SetFloat("Sensitivity", value);

        if (playerMovement != null)
        {
            sensitivityText.text = value.ToString("F2");
            playerMovement.UpdateSensitivity(value);
        }
    }
    public void SetDifficulty(string difficulty)
    {
        PlayerPrefs.SetString("Difficulty", difficulty);
        Debug.Log($"난이도가 변경되었습니다: {difficulty}");
    }
    public void OnClickSetDifficulty(string difficulty)
    {

        PlayerPrefs.SetString("SelectedDifficulty", difficulty);
        PlayerPrefs.Save(); 

        Debug.Log($"[로그] 난이도 변경 완료! 현재 난이도: {difficulty}");

        if (currentDifficultyText != null)
        {
            currentDifficultyText.text = $"{difficulty}";
        }
    }
}
