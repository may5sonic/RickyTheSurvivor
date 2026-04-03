using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOverUI : MonoBehaviour
{
    public GameObject gameOverRoot;
    public bool pauseTime = true;

    void Awake()
    {
        if (gameOverRoot != null)
        {
            gameOverRoot.SetActive(false);
        }
    }

    public void Show()
    {
        if (gameOverRoot != null)
        {
            gameOverRoot.SetActive(true);
        }

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        if (pauseTime)
        {
            Time.timeScale = 0f;
        }
    }

    public void Restart()
    {
        Time.timeScale = 1f;
        Scene activeScene = SceneManager.GetActiveScene();
        SceneManager.LoadScene(activeScene.buildIndex);
    }

    public void Quit()
    {
        Time.timeScale = 1f;
        Application.Quit();
    }
}
