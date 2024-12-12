using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ButtonsController : MonoBehaviour
{
    public void LoadLevelsScene()
    {
        SceneManager.LoadScene("Levels");
    }

    public void LoadLevel1Scene()
    {
        SceneManager.LoadScene("Level-1");
    }
    public void LoadLevel2Scene()
    {
        SceneManager.LoadScene("Level-2");
    }
    public void LoadLevel3Scene()
    {
        SceneManager.LoadScene("Level-3");
    }
    public void LoadLevel4Scene()
    {
        SceneManager.LoadScene("Level-4");
    }

    public void LoadMenuScene()
    {
        SceneManager.LoadScene("Menu");
    }
}
