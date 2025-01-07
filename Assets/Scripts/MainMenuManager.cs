using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuManager : MonoBehaviour
{
    public void PlayGame()
    {
        SceneManager.LoadScene("Main");
    }

    public void OpenOptions()
    {
        // Affiche un menu d'options ou ouvre une autre scène
        Debug.Log("Options ouvertes");
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}

