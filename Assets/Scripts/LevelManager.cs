using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{
    public void Start()
    {
        GameObject.DontDestroyOnLoad(this.gameObject);
    }

    public void LoadLevel(string name)
    {
        Debug.Log("New Level load: " + name);
        SceneManager.LoadScene(name);
    }

    public void LoadLevelDelayed(string name, float seconds)
    {
        StartCoroutine(LoadLevelWithDelay(name, seconds));
    }

    private IEnumerator LoadLevelWithDelay(string name, float seconds)
    {
        yield return new WaitForSeconds(seconds);
        LoadLevel(name);
    }

    public void QuitRequest()
    {
        Debug.Log("Quit requested");
        Application.Quit();
    }
}