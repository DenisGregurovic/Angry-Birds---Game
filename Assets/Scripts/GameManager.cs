using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using DG.Tweening;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    [SerializeField] private float secondsToWaitBeforeDeathCheck = 3f;
    [SerializeField] private GameObject menuScreenObject;
    [SerializeField] private SlingShotHandler slingShotHandler;
    [SerializeField] private Image nextLevelImage;
    [SerializeField] private Image congrats;

    public int MaxNumberOfShots = 3;

    private int usedNumberOfShots;

    private IconHandler iconHandler;

    private List<Baddie> baddies = new List<Baddie>();

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        iconHandler = FindFirstObjectByType<IconHandler>();
        Baddie[] arrayOfBaddies = FindObjectsOfType<Baddie>();
        for (int i = 0; i < arrayOfBaddies.Length; i++)
        {
            baddies.Add(arrayOfBaddies[i]);
        }
        nextLevelImage.enabled = false;
    }

    private void OnApplicationQuit()
    {
        PlayerPrefs.DeleteAll(); 
        PlayerPrefs.SetInt("UnlockedLevel", 1);
        PlayerPrefs.Save();
    }

    public void UseShot()
    {
        usedNumberOfShots++;
        iconHandler.UseShot(usedNumberOfShots);
        CheckForLastShot();
    }

    public bool HasEnoughShots()
    {
        if (usedNumberOfShots < MaxNumberOfShots)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    public void CheckForLastShot()
    {
        if (usedNumberOfShots == MaxNumberOfShots)
        {
            StartCoroutine(CheckAfterWaitTime());
        }
    }

    private IEnumerator CheckAfterWaitTime()
    {
        yield return new WaitForSeconds(secondsToWaitBeforeDeathCheck);
        if (baddies.Count == 0)
        {
            WinGame();
        }
        else
        {
            LostGame();
        }
    }

    public void RemoveBaddie(Baddie baddie)
    {
        baddies.Remove(baddie);
        CheckForAllDeadBaddies();
    }

    private void CheckForAllDeadBaddies()
    {
        if (baddies.Count == 0)
        {
            WinGame();
        }
    }

    #region WinLose

    private void WinGame()
    {
        menuScreenObject.SetActive(true);
        slingShotHandler.enabled = false;

        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        int maxLevels = SceneManager.sceneCountInBuildSettings;
        UnlockNewLevel();
        if (currentSceneIndex + 1 < maxLevels)
        {
            congrats.enabled = true;
            nextLevelImage.enabled = true;
        }
    }

    public void RestartGame()
    {
        DOTween.Clear(true);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void LostGame()
    {
        menuScreenObject.SetActive(true);
        slingShotHandler.enabled = false;

        congrats.enabled = false;
        nextLevelImage.enabled = false;
    }

    public void ReturnToMainMenu()
    {
        DOTween.Clear(true);
        SceneManager.LoadScene("Menu");
    }

    public void NextLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    void UnlockNewLevel()
    {
        int currentLevel = SceneManager.GetActiveScene().buildIndex;
        int unlockedLevel = PlayerPrefs.GetInt("UnlockedLevel", 1);

        if (currentLevel >= unlockedLevel)
        {
            PlayerPrefs.SetInt("UnlockedLevel", currentLevel + 1);
            PlayerPrefs.Save();
        }
    }

    #endregion
}
