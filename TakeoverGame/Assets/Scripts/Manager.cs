using System.Collections;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Manager : MonoBehaviour
{
    [Header("Level")]
    [SerializeField]
    private TextMeshProUGUI levelText;
    [SerializeField]
    private int levelNumber = 1;

    [Header("Progress")]
    [SerializeField]
    private Bar progressBar;

    [Header("Start-End Game")]
    [SerializeField]
    private GameObject tutorial;

    [SerializeField]
    private GameObject levelEndUI;

    [SerializeField]
    private GameObject levelFailedText;
    [SerializeField]
    private GameObject restartButton;

    [SerializeField]
    private GameObject levelCompletedText;
    [SerializeField]
    private GameObject nextButton;

    [SerializeField]
    private GameObject levelUI;

    private bool isFirstTouch;

    private bool firstMove;

    private bool isLevelCompleted;
    private bool isLevelFailed;
    private bool isLevelFinished;

    public static Manager manager;
    private void Awake()
    {
        if (manager == null)
        {
            manager = this;
        }

        levelText.text = "LEVEL - " + levelNumber.ToString();
    }

    private void Update()
    {
        progressBar.SetCurrentValue(TowerManager.towerManager.GetAlliedTowerCount());

        if (!isFirstTouch && firstMove)
        {
            if (!isLevelFinished)
            {
                isFirstTouch = true;
                tutorial.SetActive(false);
            }
        }
    }

    public void FinishLevel(bool _levelCompleted)
    {
        StartCoroutine(FinishLevelCoroutine(_levelCompleted));
    }

    public IEnumerator FinishLevelCoroutine(bool _levelCompleted)
    {
        // Debug.Log("LEVEL FINISHED !");

        if (!isLevelFinished)
        {
            isLevelFinished = true;
            levelUI.SetActive(false);
            levelEndUI.SetActive(true);
        }

        yield return new WaitForSeconds(0.25f);

        if (_levelCompleted)
        {
            LevelCompleted();
        }
        else
        {
            LevelFailed();
        }
        StopAllCoroutines();
    }


    public void LevelCompleted()
    {
        levelCompletedText.SetActive(true);
        nextButton.SetActive(true);
        // Debug.Log("Level Completed !");
    }

    public void LevelFailed()
    {
        levelFailedText.SetActive(true);
        restartButton.SetActive(true);
        // Debug.Log("Level Failed !");
    }

    public void StartLevel()
    {
        StartCoroutine(StartLevelRoutine());
    }

    public IEnumerator StartLevelRoutine()
    {
        progressBar.SetMaxValue(TowerManager.towerManager.GetTowerCount());
        progressBar.SetCurrentValue(TowerManager.towerManager.GetAlliedTowerCount());
        yield return new WaitForSeconds(0.25f);
        firstMove = true;
        levelUI.SetActive(true);
    }

    public void RestartLevel()
    {
        SceneManager.LoadScene(levelNumber - 1);
    }

    public void NextLevel()
    {
        if (SceneManager.sceneCountInBuildSettings > levelNumber)
            SceneManager.LoadScene(levelNumber);
        else
            SceneManager.LoadScene(0);
    }

    public void Quit()
    {
        Application.Quit();
    }
}
