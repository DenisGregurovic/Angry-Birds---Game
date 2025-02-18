using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using DG.Tweening;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    [SerializeField] public GameObject menuCanvas;
    [SerializeField] public GameObject optionsCanvas; 

    private void Start()
    {
        ShowMainMenu();
    }

    public void ShowMainMenu()
    {
        menuCanvas.SetActive(true);  
        optionsCanvas.SetActive(false); 
    }

    public void ShowOptionsMenu()
    {
        menuCanvas.SetActive(false); 
        optionsCanvas.SetActive(true); 
    }

    public void PlayGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    public void QuitGame()
    {
        PlayerPrefs.DeleteAll();
        Application.Quit();
    }
}
