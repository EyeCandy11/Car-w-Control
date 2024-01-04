using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuManager : MonoBehaviour
{
    public GameObject PausedPanel;
    public void Start()
    {
        Time.timeScale = 1;
    }
    public void HideObject(GameObject ObjHide)
    {
        object obj = ObjHide;
        ObjHide.SetActive(false);

    }
    public void ShowObject(GameObject ObjShow)
    {
        ObjShow.SetActive(true);
    }

    public void LoadScene(string SceneName)
    {
        SceneManager.LoadScene(SceneName);
    }
    public void Quit()
    {
        Application.Quit();
        Debug.Log("Quit!");
    }
    public void ResetScene(string SceneName)
    {
        SceneManager.LoadScene(SceneName);
    }


}
