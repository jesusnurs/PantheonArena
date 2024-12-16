using UnityEngine;

public class AudioManager : MonoBehaviour
{
    [System.Serializable]
    public class Sound
    {
        public string name;
        public AudioClip clip;
        [Range(0f, 1f)]
        public float volume = 1f;
        [Range(0.1f, 3f)]
        public float pitch = 1f;
        public bool loop = false;
        [HideInInspector]
        public AudioSource source;
    }

    public static AudioManager Instance;
    public Sound[] sounds;
    private string currentMusicTrack = "";

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            
            // Initialize audio sources
            foreach (Sound s in sounds)
            {
                s.source = gameObject.AddComponent<AudioSource>();
                s.source.clip = s.clip;
                s.source.volume = s.volume;
                s.source.pitch = s.pitch;
                s.source.loop = s.loop;
                s.source.playOnAwake = false;
            }

            // Log all loaded scenes at startup
            Debug.Log("AudioManager: Checking loaded scenes at startup:");
            for (int i = 0; i < UnityEngine.SceneManagement.SceneManager.sceneCount; i++)
            {
                var scene = UnityEngine.SceneManagement.SceneManager.GetSceneAt(i);
            }

            // Play menu music only if we're in the menu scene and it's the only scene
            if (UnityEngine.SceneManagement.SceneManager.GetActiveScene().name == "MainMenu" 
                && UnityEngine.SceneManagement.SceneManager.sceneCount == 1)
            {
                ChangeMusicTrack("MenuMusic");
            }
        }
        else
        {
            Destroy(gameObject);
            return;
        }

        UnityEngine.SceneManagement.SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDestroy()
    {
        UnityEngine.SceneManagement.SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void OnSceneLoaded(UnityEngine.SceneManagement.Scene scene, UnityEngine.SceneManagement.LoadSceneMode mode)
    {
        switch (scene.name)
        {
            case "MainMenu":
                ChangeMusicTrack("MenuMusic");
                break;
            case "Arena":
                ChangeMusicTrack("ArenaMusic");
                break;
        }
    }

    private void ChangeMusicTrack(string newTrack)
    {
        // If it's the same track, don't do anything
        if (currentMusicTrack == newTrack)
        {
            return;
        }
        
        // Stop current music if any
        if (!string.IsNullOrEmpty(currentMusicTrack))
        {
            StopSound(currentMusicTrack);
        }
        
        // Play new music
        PlaySound(newTrack);
        currentMusicTrack = newTrack;
    }

    public void PlaySound(string name)
    {
        Sound s = System.Array.Find(sounds, sound => sound.name == name);
        if (s == null)
        {
            Debug.LogWarning("Sound: " + name + " not found!");
            return;
        }
        s.source.Play();
    }

    public void StopSound(string name)
    {
        Sound s = System.Array.Find(sounds, sound => sound.name == name);
        if (s == null)
        {
            Debug.LogWarning("Sound: " + name + " not found!");
            return;
        }
        s.source.Stop();
    }
}