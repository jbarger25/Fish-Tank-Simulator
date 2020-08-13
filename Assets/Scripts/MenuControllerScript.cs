using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuControllerScript : MonoBehaviour
{

    public void Quit()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = (false);
#else
        Application.Quit();
#endif
    }

    public void Play()
    {
        SceneManager.LoadScene("FlockingAIScene");
    }
    public void PlayMP()
    {
        SceneManager.LoadScene("Launcher");
    }
}
