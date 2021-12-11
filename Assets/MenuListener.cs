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
        Debug.Log("Load Saved Systems Here");
    }

    public void LoadOptionsScene()
    {
        Debug.Log("Load Options Scene Here");
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
