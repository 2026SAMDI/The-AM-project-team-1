using UnityEngine;
using UnityEngine.SceneManagement;

public class MainGameTitleUI : MonoBehaviour
{
    public void goToManu() 
    {
        Time.timeScale = 1f; // 이거 하는 이유 : 없으면 다시 시작할때 시간 안흐름(더월드 당함)
        OpenMenu.isMenuOpen = false;
        SceneManager.LoadScene("title");
    }
    public void RestartGame()
    {
        Time.timeScale = 1f;
        OpenMenu.isMenuOpen = false;
        Scene scene = SceneManager.GetActiveScene();
        SceneManager.LoadScene(scene.name);
    }
}
