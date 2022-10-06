using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MenusControl : MonoBehaviour
{

    public GameObject PausePanel;
   
    void Start()
    {
        PausePanel.SetActive(false);
    }

 
    void Update()
    {

    }


    public void Resume()
    {
        Time.timeScale = 1;
        PausePanel.SetActive(false);
    }


    public void menu()
    {
        Time.timeScale = 1;

        SceneManager.LoadScene(0);

    }

    public void pause()
    {
        
            Time.timeScale = 0;
            PausePanel.SetActive(true);
        
      
    }

    public void play()
    {
        SceneManager.LoadScene(1);
    }

    public void exitGame()
    {
        Application.Quit();
    }
}
