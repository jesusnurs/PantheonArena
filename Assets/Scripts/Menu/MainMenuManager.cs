using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuManager : MonoBehaviour
{
    [SerializeField] private UIToolkitSettingsController settingsController;

    private void Start()
    {
        // Ensure we start with proper state
        Time.timeScale = 1f;
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
        
        // Clean up any duplicate systems
        CleanupDuplicateSystems();
    }

    private void CleanupDuplicateSystems()
    {
        // Clean up duplicate EventSystems
        var eventSystems = FindObjectsOfType<UnityEngine.EventSystems.EventSystem>();
        if (eventSystems.Length > 1)
        {
            for (int i = 1; i < eventSystems.Length; i++)
            {
                Destroy(eventSystems[i].gameObject);
            }
        }

        // Clean up duplicate AudioListeners
        var audioListeners = FindObjectsOfType<AudioListener>();
        if (audioListeners.Length > 1)
        {
            for (int i = 1; i < audioListeners.Length; i++)
            {
                Destroy(audioListeners[i]);
            }
        }
    }

    public void PlayGame()
    {
        Debug.Log("Play button clicked");
        // Make sure to unload everything before loading new scene
        SceneManager.LoadScene("Arena", LoadSceneMode.Single);
    }

    public void Settings()
    {
        Debug.Log("Settings button clicked");
        if (settingsController != null)
        {
            settingsController.ShowSettings();
        }
        else
        {
            Debug.LogError("Settings Controller not assigned in MainMenuManager!");
        }
    }

    public void QuitGame()
    {
        Debug.Log("Quit button clicked");
        #if UNITY_EDITOR
                UnityEditor.EditorApplication.isPlaying = false;
        #else
                    Application.Quit();
        #endif
    }
}