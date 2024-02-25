using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class BacktoMenu : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI title_text;
    [SerializeField] TextMeshProUGUI movecontrols_text;
    public void Load(string LevelName)
    {
        SceneManager.LoadScene(LevelName);
    }
}
