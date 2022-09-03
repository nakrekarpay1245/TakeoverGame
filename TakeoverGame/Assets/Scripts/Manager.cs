using System.Collections;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class Manager : MonoBehaviour
{
    [Header("Level")]
    [SerializeField]
    private TextMeshProUGUI levelText;
    public int levelNumber = 1;

    [Header("Progress")]
    [SerializeField]
    private Bar progressBar;

    public int totalTowerCount = 0;
    public int alliedTowerCount = 0;
    public int oppositeTowerCount = 0;

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

    public bool isLevelCompleted;
    public bool isLevelFailed;
    public bool isLevelFinished;

    public static Manager manager;
    private void Awake()
    {
        if (manager == null)
        {
            manager = this;
        }
        levelText.text = "LEVEL - " + levelNumber.ToString();
    }

    private void Start()
    {
    }

    private void Update()
    {
        if (!isFirstTouch && firstMove)
        {
            if (!isLevelFinished)
            {
                isFirstTouch = true;
                tutorial.SetActive(false);
            }
        }
    }

    public void FinishLevel()
    {
        StartCoroutine(FinishLevelCoroutine());
    }

    public IEnumerator FinishLevelCoroutine()
    {
        Debug.Log("LEVEL FINISHED !");

        if (!isLevelFinished)
        {
            isLevelFinished = true;
            levelUI.SetActive(false);
            levelEndUI.SetActive(true);
        }

        yield return new WaitForSeconds(1);

        if (totalTowerCount == alliedTowerCount)
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
        Debug.Log("Level Completed !");
    }

    public void LevelFailed()
    {
        levelFailedText.SetActive(true);
        restartButton.SetActive(true);
        Debug.Log("Level Failed !");
    }

    public void StartLevel()
    {
        StartCoroutine(StartLevelRoutine());
    }

    public IEnumerator StartLevelRoutine()
    {
        yield return new WaitForSeconds(0.25f);
        firstMove = true;
        levelUI.SetActive(true);
    }

    public void IncreaseAlliedTowerCount()
    {
        Debug.Log("Increase Allied Tower Count");
        alliedTowerCount++;
    }

    public void DecreaseAlliedTowerCount()
    {
        Debug.Log("Increase Allied Tower Count");
        alliedTowerCount--;
    }

    public void IncreaseOppositeTowerCount()
    {
        Debug.Log("Increase Allied Tower Count");
        oppositeTowerCount++;
    }

    public void DecreaseOppositeTowerCount()
    {
        Debug.Log("Increase Allied Tower Count");
        oppositeTowerCount--;
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
