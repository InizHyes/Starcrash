using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class gameOver : MonoBehaviour
{
    public GameObject playerContorller;
    
    // Start is called before the first frame update
    void Start()
    {
        ///this.gameObject.SetActive(false);
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Replay()
    {
        ///put reply functionality here
        SceneManager.LoadScene("GameBuildBeta");

    }
    public void goToMenu ()
    {
        SceneManager.LoadScene("Menu");

    }
}
