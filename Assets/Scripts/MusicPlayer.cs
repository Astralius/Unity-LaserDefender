using UnityEngine;

public class MusicPlayer : MonoBehaviour
{
    public AudioClip startClip;
    public AudioClip gameClip;
    public AudioClip endClip;

    private static MusicPlayer instance;

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
            GameObject.DontDestroyOnLoad(this.gameObject);
        }

    }
}
