using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using DG.Tweening;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{

    public static GameManager Instance;

    private IconHandler iconHandler;

    [SerializeField] private float secondsToWaitBeforeDeathCheck = 3.0f;
    [SerializeField] private GameObject restartUI;
    [SerializeField] private SlingShotHandler slingShotHandler;
    [SerializeField] private CameraManager cameraManager;
    [SerializeField] private Image nextLevelImg;

    private List<baddie> baddieList = new List<baddie>();

    public int maxNumberOfShots = 3;
    private int usedNumberOfShots;

    private void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
            iconHandler = FindObjectOfType<IconHandler>();
        }

        restartUI.SetActive(false);

        baddie[] _baddies = FindObjectsOfType<baddie>();
        for (int i = 0; i < _baddies.Length; i++)
        {
            baddieList.Add(_baddies[i]);
        }
        nextLevelImg.enabled = false;
    }

    public void UseShot()
    {
        usedNumberOfShots++;
        iconHandler.UseShot(usedNumberOfShots);

        //CheckForLastShot();

    }
    public bool hasEnoughShots()
    {
        if (usedNumberOfShots < maxNumberOfShots)
        {
            return true;

        }
        else { return false; }
    }

    public void CheckForLastShot()
    {
        if(usedNumberOfShots == maxNumberOfShots)
        {
            StartCoroutine(CheckAfterWaitTime());
        }
    }

    private IEnumerator CheckAfterWaitTime()
    {
        yield return new WaitForSeconds(secondsToWaitBeforeDeathCheck);

        //does all the baddies killed
        if(baddieList.Count  == 0)
        {
            //win
            WinGame();
        }
        else
        {
            //lose
            
            RestartGame();
        }

    }

    public void RemoveBaddie(baddie _baddie)
    {
        baddieList.Remove(_baddie);
        CheckForAllDeadBaddie();
    }

    public void CheckForAllDeadBaddie()
    {
        if(baddieList.Count == 0)
        {
            //win
            StartCoroutine(CheckAfterWaitTime());
        }
    }

    #region win/lose

    private void WinGame()
    {
        cameraManager.SwitchToIdleCam();
        
        restartUI.SetActive(true);
        slingShotHandler.enabled = false;

        //do we have any more scene to load?
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        int maxLevels = SceneManager.sceneCountInBuildSettings;
        if(currentSceneIndex + 1 < maxLevels)
        {
            nextLevelImg.enabled = true;
        }


    }
    public void RestartGame()
    {
        cameraManager.SwitchToIdleCam();
       // DOTween.Clear(true);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void NextLevel()
    {

        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    #endregion

}