using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuManager : MonoBehaviour
{
    public void PlayGame()
    {
        
        SceneManager.LoadSceneAsync("MainScene");  
    }

    public void Multiplayer()
    {
        
        SceneManager.LoadScene("MultiplayerScene");  
    }

    public void Settings()
    {
        
        SceneManager.LoadScene("SettingsScene"); 
    }

    public void PlayerStats()
    {
        
        SceneManager.LoadScene("PlayerStatsScene"); 
    }
    public void BackToMainMenu()
    {
        SceneManager.LoadScene("MainMenu"); 
    }
    public void QuitGame()
    {
        
        Application.Quit();
        Debug.Log("Game Quit");
    }
}
