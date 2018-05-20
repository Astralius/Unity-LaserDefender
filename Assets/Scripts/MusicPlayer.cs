using UnityEngine;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(AudioSource))]
public class MusicPlayer : MonoBehaviour
{
    public AudioClip[] Playlist;

    private static MusicPlayer instance;
    private AudioSource musicSource;

    private void Start()
    {
        if (instance != null && instance != this)
        {
            Destroy(this.gameObject);
            print("Duplicate music player self-destructing!");
        }
        else
        {
            instance = this;
            DontDestroyOnLoad(this.gameObject);
            musicSource = GetComponent<AudioSource>();
        }

        SceneManager.sceneLoaded += (scene, mode) => OnSceneLoaded(scene);
    }

    private void OnSceneLoaded(Scene scene)
    {
        musicSource.Stop();
        musicSource.clip = Playlist[scene.buildIndex];
        musicSource.Play();
    }
}
