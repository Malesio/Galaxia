using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneInputManager : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButton("Cancel"))
        {
            SceneManager.LoadScene("Startup");
        }
    }
}
