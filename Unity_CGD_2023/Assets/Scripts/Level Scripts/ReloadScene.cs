using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ReloadScene : MonoBehaviour
{
    void Update()
    {
        // Check if the 'P' key is pressed
        if (Input.GetKeyDown(KeyCode.P))
        {
            // Reload the current scene
            SceneReload();
        }
    }

    void SceneReload()
    {
        // Get the index of the current scene
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;

        // Reload the current scene
        SceneManager.LoadScene(currentSceneIndex);
    }
}
