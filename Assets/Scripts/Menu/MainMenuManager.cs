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
        
        // If we're in MainMenu but Arena is also loaded, unload it
        for (int i = 0; i < SceneManager.sceneCount; i++)
        {
            Scene scene = SceneManager.GetSceneAt(i);
            if (scene.name == "Arena")
            {
                Debug.Log("Found Arena scene loaded with MainMenu, unloading it...");
                SceneManager.UnloadSceneAsync(scene);
            }
        }
        
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
        Debug.Log("Play button clicked - Loading Arena scene");
        // Ensure we properly unload the current scene and load the new one
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