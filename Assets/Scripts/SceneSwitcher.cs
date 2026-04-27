using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneSwitcher : MonoBehaviour
{
    public void GoToScene(string name)
    {
        SceneManager.LoadScene(name);
        Time.timeScale = 1.0f;
    }
}
