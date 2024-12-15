using UnityEngine;
using UnityEngine.UIElements;
using System.Linq;
using UnityEngine.UI;
using Button = UnityEngine.UIElements.Button;

public class UIToolkitSettingsController : MonoBehaviour
{
    [SerializeField] private Canvas mainMenuCanvas;
    [SerializeField] private UnityEngine.UI.Button[] mainMenuButtons; // Array for all buttons that should be disabled
    private UIDocument settingsDocument;
    private DropdownField resolutionDropdown;
    private DropdownField qualityDropdown;
    private Button applyButton;
    private Button cancelButton;
    private Button quitButton;
    
    private Resolution[] resolutions;
    private Resolution currentResolution;
    private int currentQualityLevel;
    
    [SerializeField] private bool isMainMenu = false;

    private void Awake()
    {
        settingsDocument = GetComponent<UIDocument>();
        settingsDocument.rootVisualElement.style.display = DisplayStyle.None;
        
        if (isMainMenu && mainMenuButtons == null || mainMenuButtons.Length == 0)
        {
            // Auto-find all buttons if not assigned
            mainMenuButtons = FindObjectsOfType<UnityEngine.UI.Button>();
        }
        
        resolutions = Screen.resolutions;
        currentResolution = Screen.currentResolution;
        currentQualityLevel = QualitySettings.GetQualityLevel();
        
        InitializeUI();
    }

    private void InitializeUI()
    {
        var root = settingsDocument.rootVisualElement;
        
        // Get references to UI elements
        resolutionDropdown = root.Q<DropdownField>("DisplayResolution");
        qualityDropdown = root.Q<DropdownField>("Quality");
        applyButton = root.Q<Button>("Apply");
        cancelButton = root.Q<Button>("Cancel");
        quitButton = root.Q<Button>("Quit");

        // Set up resolution dropdown
        var resolutionOptions = resolutions.Select(res => 
            $"{res.width}x{res.height} @{res.refreshRate}Hz").ToList();
        resolutionDropdown.choices = resolutionOptions;
        
        // Find and set current resolution
        int currentResIndex = System.Array.IndexOf(resolutions, currentResolution);
        if (currentResIndex >= 0)
        {
            resolutionDropdown.index = currentResIndex;
        }

        // Set up quality dropdown
        qualityDropdown.choices = QualitySettings.names.ToList();
        qualityDropdown.index = currentQualityLevel;

        // Set up button events
        applyButton.clicked += ApplySettings;
        cancelButton.clicked += CancelSettings;
        if (quitButton != null)
        {
            quitButton.clicked += QuitGame;
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            // In main menu, Escape only closes settings if they're open
            if (isMainMenu)
            {
                if (settingsDocument.rootVisualElement.style.display == DisplayStyle.Flex)
                {
                    HideSettings();
                }
            }
            else
            {
                // In game, Escape toggles settings
                ToggleSettingsUI();
            }
        }
    }

    public void ShowSettings()
    {
        Debug.Log("Showing settings");
        settingsDocument.rootVisualElement.style.display = DisplayStyle.Flex;
        
        if (isMainMenu && mainMenuButtons != null)
        {
            // Disable interaction with all main menu buttons
            foreach (var button in mainMenuButtons)
            {
                if (button != null)
                    button.interactable = false;
            }
        }
        
        if (!isMainMenu)
        {
            Time.timeScale = 0f;
        }

    }

    public void HideSettings()
    {
        Debug.Log("Hiding settings");
        settingsDocument.rootVisualElement.style.display = DisplayStyle.None;
        
        if (isMainMenu && mainMenuButtons != null)
        {
            // Re-enable interaction with all main menu buttons
            foreach (var button in mainMenuButtons)
            {
                if (button != null)
                    button.interactable = true;
            }
        }
        
        if (!isMainMenu)
        {
            Time.timeScale = 1f;
        }
    }
    
    private void ToggleSettingsUI()
    {
        var root = settingsDocument.rootVisualElement;
        bool isVisible = root.style.display == DisplayStyle.Flex;
        
        if (isVisible)
        {
            HideSettings();
        }
        else
        {
            ShowSettings();
        }
    }
    private void ApplySettings()
    {
        // Apply resolution
        Resolution selectedResolution = resolutions[resolutionDropdown.index];
        Screen.SetResolution(selectedResolution.width, selectedResolution.height, Screen.fullScreen, selectedResolution.refreshRate);
        
        // Apply quality
        QualitySettings.SetQualityLevel(qualityDropdown.index);
        
        // Save current settings
        currentResolution = selectedResolution;
        currentQualityLevel = qualityDropdown.index;
        
        // Save to PlayerPrefs
        PlayerPrefs.SetInt("QualityLevel", currentQualityLevel);
        PlayerPrefs.SetInt("ResolutionWidth", currentResolution.width);
        PlayerPrefs.SetInt("ResolutionHeight", currentResolution.height);
        PlayerPrefs.SetInt("RefreshRate", currentResolution.refreshRate);
        PlayerPrefs.Save();
        
        HideSettings();
    }

    private void CancelSettings()
    {
        // Reset dropdowns to saved values
        resolutionDropdown.index = System.Array.IndexOf(resolutions, currentResolution);
        qualityDropdown.index = currentQualityLevel;
        
        HideSettings();
    }

    private void QuitGame()
    {
        #if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
        #else
            Application.Quit();
        #endif
    }

    private void OnEnable()
    {
        // Load saved settings
        if (PlayerPrefs.HasKey("QualityLevel"))
        {
            currentQualityLevel = PlayerPrefs.GetInt("QualityLevel");
            QualitySettings.SetQualityLevel(currentQualityLevel);
            
            int width = PlayerPrefs.GetInt("ResolutionWidth");
            int height = PlayerPrefs.GetInt("ResolutionHeight");
            int refreshRate = PlayerPrefs.GetInt("RefreshRate");
            
            Screen.SetResolution(width, height, Screen.fullScreen, refreshRate);
            
            // Find matching resolution in available resolutions
            currentResolution = System.Array.Find(resolutions, 
                r => r.width == width && r.height == height && r.refreshRate == refreshRate);
        }
    }
}