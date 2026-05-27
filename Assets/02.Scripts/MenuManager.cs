using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class MenuManager : MonoBehaviour
{
    [Header("--- Panels ---")]
    [SerializeField] private GameObject mainMenuPanel;
    [SerializeField] private GameObject settingsPanel;

    [Header("--- Main Menu UI ---")]
    [SerializeField] private TMP_InputField nicknameInputField;

    [Header("--- Settings UI ---")]
    [SerializeField] private Slider sensitivitySlider;
    [SerializeField] private TMP_Text sensitivityText;

    private void Start()
    {
        mainMenuPanel.SetActive(true);
        settingsPanel.SetActive(false);


        float savedSensitivity = PlayerPrefs.GetFloat("Sensitivity", 1.0f);
        if (sensitivitySlider != null)
        {
            sensitivitySlider.value = savedSensitivity;
            UpdateSensitivityText(savedSensitivity);
        }
        
        
        if (sensitivitySlider != null)
        {
            sensitivitySlider.onValueChanged.RemoveAllListeners(); 
            sensitivitySlider.onValueChanged.AddListener(OnSensitivityChanged);
        }
    }

    public void OnClickPlay()
    {
        string nickname = nicknameInputField.text;
        if (string.IsNullOrEmpty(nickname)) nickname = "Player";
        PlayerPrefs.SetString("PlayerNickname", nickname);
        PlayerPrefs.Save();
        Debug.Log($"[로그] 게임 시작! 닉네임: {nickname}");
    }

    public void OnClickOpenSettings()
    {
        mainMenuPanel.SetActive(false);
        settingsPanel.SetActive(true);
    }

    public void OnClickCloseSettings()
    {
        mainMenuPanel.SetActive(true);
        settingsPanel.SetActive(false);
    }

    
    public void OnSensitivityChanged(float value)
    {
        
        Debug.Log($"[로그] 슬라이더 움직임 감지됨! 현재 값: {value}");

        PlayerPrefs.SetFloat("Sensitivity", value);
        UpdateSensitivityText(value);
    }

    private void UpdateSensitivityText(float value)
    {
        if (sensitivityText != null)
        {
            sensitivityText.text = value.ToString("F2");
        }
        else
        {
            
            Debug.LogError("[에러] Sensitivity Text 칸이 비어있습니다! 인스펙터 창을 확인하세요.");
        }
    }
}