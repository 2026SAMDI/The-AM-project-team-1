using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement; // 씬 이동을 위해 추가

public class MenuManager : MonoBehaviour
{
    [Header("--- Panels ---")]
    [SerializeField] private GameObject mainMenuPanel;
    [SerializeField] private GameObject settingsPanel;

    [Header("--- Main Menu UI ---")]
    [SerializeField] private TMP_InputField nicknameInputField;
    [SerializeField] private Button playButton;
    
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

    //게임 시작 버튼을 누르면 발동하는 함수
    public void OnClickPlay()
    {
        string nickname = nicknameInputField.text.Trim();
        
        // 만약 빈칸이면 기본값으로 "Player" 지정
        if (string.IsNullOrEmpty(nickname)) nickname = "Player";
        
        // 하드디스크에 "PlayerNickname"이라는 열쇠로 저장
        PlayerPrefs.SetString("PlayerNickname", nickname);
        PlayerPrefs.Save();
        Debug.Log($"[로그] 이름 저장 완료! 닉네임: {nickname}");

        // 저장하자마자 즉시 인게임 Scene으로 이동
        SceneManager.LoadScene("AM_MainScene");
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