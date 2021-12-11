using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuListener : MonoBehaviour
{
    public void LoadEmptySolarSystemScene()
    {
        SceneManager.LoadScene("Main");
    }

    public void LoadSavedSystemsListScene()
    {
        SceneManager.LoadScene("SystemList");
    }

    public void LoadOptionsScene()
    {
        SceneManager.LoadScene("Options");
    }

    public void ExitGame()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }
}
