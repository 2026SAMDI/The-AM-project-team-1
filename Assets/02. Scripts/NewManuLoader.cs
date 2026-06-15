using UnityEngine;

public class NewManuLoader : MonoBehaviour
{
   public void OnClickloadScene()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene("AM_MainScene");
    }
}
