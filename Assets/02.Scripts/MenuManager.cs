using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class MenuManager : MonoBehaviour
{
    [Header("--- Panels ---")]
    public GameObject mainMenuPanel;
    public GameObject settingsPanel;

    [Header("--- Main Menu UI ---")]
    public TMP_InputField nicknameInputField;

    [Header("--- Settings UI ---")]
    public Slider sensitivitySlider;
    public TMP_Text sensitivityText;

    private void Start()
    {
        mainMenuPanel.SetActive(true);
        settingsPanel.SetActive(false);

        // 로드 시 작동 확인용 로그
        float savedSensitivity = PlayerPrefs.GetFloat("Sensitivity", 1.0f);
        if (sensitivitySlider != null)
        {
            sensitivitySlider.value = savedSensitivity;
            UpdateSensitivityText(savedSensitivity);
        }
        
        // 코드 자동 연결 방식도 확실하게 재확인
        if (sensitivitySlider != null)
        {
            sensitivitySlider.onValueChanged.RemoveAllListeners(); // 중복 방지
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

    // 슬라이더 조절 시 무조건 실행되어야 하는 함수
    public void OnSensitivityChanged(float value)
    {
        // ★ 이 로그가 유니티에 뜨는지 보는 게 핵심입니다!
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
            // ★ 만약 텍스트가 안 뜨면 이게 범인입니다.
            Debug.LogError("[에러] Sensitivity Text 칸이 비어있습니다! 인스펙터 창을 확인하세요.");
        }
    }
}