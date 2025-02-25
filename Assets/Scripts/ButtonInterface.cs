using UnityEngine;
using UnityEngine.SceneManagement;

public class ButtonInterface : MonoBehaviour
{
    public void PlayGame()
    {
        SceneManager.LoadSceneAsync(1);
    }

    public void ViewCredits()
    {
        SceneManager.LoadSceneAsync(2);
    }

    public void ReturnToMain()
    {
        SceneManager.LoadSceneAsync(0);
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
