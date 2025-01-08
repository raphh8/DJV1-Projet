using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuManager : MonoBehaviour
{
    public GameObject rules;
    public void PlayGame()
    {
        SceneManager.LoadScene("Main");
    }

    public void OpenRules()
    {
        rules.SetActive(true);
    }

    public void CloseRules()
    {
        rules.SetActive(false);
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}

