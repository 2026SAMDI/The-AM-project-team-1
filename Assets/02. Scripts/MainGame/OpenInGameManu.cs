using UnityEngine;
using UnityEngine.InputSystem;

public class OpenMenu : MonoBehaviour
{
    [Header("패널")]
    // [SerializeField] private GameObject maingamePenel;
    [SerializeField]private GameObject manuPenel;
    [Header("뭐로하지")]
    [SerializeField] private Timer timer;
    public static bool isMenuOpen = false;

    public void OnMenuTrigger(InputAction.CallbackContext ctx)
    {
        if (ctx.performed)
        {
            if (manuPenel.activeSelf)
            {
                CloseGameManu();
            }
            else
            {
                OpenGameManu();
            }
        }
    }
    private void OpenGameManu()
    {
        isMenuOpen = true;
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
        Time.timeScale = 0f;
        manuPenel.SetActive(true);
    }
    public void CloseGameManu()
    {
        isMenuOpen = false;
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
        Time.timeScale = 1f;
        manuPenel.SetActive(false);
    }
}
