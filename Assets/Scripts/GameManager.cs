using Firebase.Analytics;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.SceneManagement;
using UnityEngine.SocialPlatforms.Impl;
using static GameManager;

public class GameManager : MonoBehaviour
{
    public GameObject winPanel;
    public GameObject losePanel;
    public GameObject pausePanel;
    public GameObject tutorialPanel;
    public GameObject player;
    public PlayerScript[] allPlayers;
    public levelBehavior[] allLevels;
    public GameObject[] playerPrefabs;
    public GameObject[] enemyPrefabs;
    public levelBehavior currentLevel;
    public GameObject loadingPanel;
    public int totalBricksCollected;
    public int totalEarnings;
    public TMP_Text winScore;
    public TMP_Text loseMessage;
    public TMP_Text winMessage;
    public bool gameOver;

    public static GameManager instance;

    public List<Material> skyboxMaterials;

    private void Awake()
    {
        Application.targetFrameRate = 120;

        if (instance != null)
        {
            Destroy(this);
        }
        else
        {
            instance = this;
        }
        player = Instantiate(playerPrefabs[PlayerPrefs.GetInt("selectedPlayer", 0)]);
        FindObjectOfType<CameraFollow>().target = player.transform;

        //Reset current level if the total levels count exceeds
        if (PlayerPrefs.GetInt("currentLevel", 0) >= allLevels.Length) 
        {
            Debug.Log("Levels Reset");
            PlayerPrefs.SetInt("currentLevel", 0);

            //GA Event
            FirebaseAnalytics.LogEvent("All_Levels" + "_Cleared");

        }

        currentLevel = Instantiate(allLevels[PlayerPrefs.GetInt("currentLevel", 0)]).GetComponent<levelBehavior>();
        //Spawn all enemies
        for (int i = 0; i < currentLevel.totalEnemies; i++) 
        {
            GameObject enemy = Instantiate(enemyPrefabs[i]);
        }
    }

    public void claimLevelReward() 
    {
        economyManager.Instance.addMoney(totalEarnings);
        //GA Event
        FirebaseAnalytics.LogEvent("Level_Reward_" + totalEarnings);
    }

    private void Start()
    {
        allPlayers = FindObjectsOfType<PlayerScript>();
        StopAllPlayers();
        currentLevel.AssignUniqueSpawnPositions();

        audioManager.instance.PlayAudio("gameplayBGM", false, Vector3.zero);

        if (PlayerPrefs.GetInt("tutorial", 1) == 1)
        {
            tutorialPanel.SetActive(true);
        }

        checkSkybox();

        //GA Event
        FirebaseAnalytics.LogEvent("Level_Started_" + PlayerPrefs.GetInt("currentLevel", 0));
    }

    public void checkSkybox()
    {
        // Assuming you have a variable called "currentLevel" that represents the current level
        int currentLevel = PlayerPrefs.GetInt("currentLevel", 0);
        // Calculate the index of the skybox based on the current level
        int skyboxIndex = currentLevel / 10; // Assuming each range of 10 levels corresponds to one skybox
        // Ensure the skyboxIndex is within the valid range
        skyboxIndex = Mathf.Clamp(skyboxIndex, 0, skyboxMaterials.Count - 1);
        // Assign the selected skybox to the RenderSettings
        RenderSettings.skybox = skyboxMaterials[skyboxIndex];
    }

    public void pauseGame() 
    {
        pausePanel.SetActive(true);
        pausePanel.GetComponent<Animator>().SetBool("show",true);
        StopAllPlayers();

        AdsManager.Instance.RunInterstitialAd();
    }

    public void unPauseGame()
    {
        pausePanel.GetComponent<Animator>().SetBool("show", false);
        StartTheGame();
    }

    public void StopAllPlayers() 
    {
        foreach (PlayerScript player in allPlayers)
        {
            player.enabled = false;
            if (player.isAI)
            {
                player.GetComponent<NavMeshAgent>().enabled = false;
                player.GetComponent<Rigidbody>().isKinematic = true;
                player.GetComponent<AIController>().enabled = false;
                player.GetComponent<AIController>().haveTarget = false;
            }
            else
            {
                player.GetComponent<MoveScript>().controller.enabled = false;
                player.GetComponent<MoveScript>().splash1.gameObject.SetActive(false);
                player.GetComponent<MoveScript>().splash2.gameObject.SetActive(false);
                //player.GetComponent<MoveScript>().splash3.gameObject.SetActive(false);
                player.GetComponent<MoveScript>().enabled = false;
            }
        }
    }

    void Update()
    {
        ShowLosePanelIfFall();
    }

    void ShowLosePanelIfFall()
    {
        if (player.transform.position.y < -15 && !gameOver)
        {
            ShowLosePanel("You Fell!");

            //GA Event
            FirebaseAnalytics.LogEvent("Level_Failed_Fall_" + PlayerPrefs.GetInt("currentLevel", 0));
        }
    }

    public void ShowWinPanel(string winText)
    {

        if (audioManager.instance.vibrationBool) 
        {
            Handheld.Vibrate();
        }

        audioManager.instance.PlayAudio("win", true, Vector3.zero);

        Invoke("delayWinPanel", 6f);
        
        //winMessage.text = winText;
        totalEarnings = totalBricksCollected * 10;
        winScore.text = totalEarnings .ToString();
        //Save win progress
        if (PlayerPrefs.GetInt("levelsCompleted", 0) <= PlayerPrefs.GetInt("currentLevel", 0)) 
        {
            PlayerPrefs.SetInt("levelsCompleted", PlayerPrefs.GetInt("levelsCompleted", 0) + 1);
        }

        AdsManager.Instance.RunInterstitialAd();

        //GA Event
        FirebaseAnalytics.LogEvent("Level_Completed_" + PlayerPrefs.GetInt("currentLevel", 0));
    }

    public void delayWinPanel() 
    {
        winPanel.SetActive(true);
    }

    public void ShowLosePanel(string loseText)
    {
        gameOver = true;

        if (audioManager.instance.vibrationBool)
        {
            Handheld.Vibrate();
        }

        audioManager.instance.PlayAudio("fail", true, Vector3.zero);


        loseMessage.text = loseText;
        winPanel.SetActive(false);
        losePanel.SetActive(true);

        AdsManager.Instance.RunInterstitialAd();

        //GA Event
        FirebaseAnalytics.LogEvent("Level_Failed_" + PlayerPrefs.GetInt("currentLevel", 0));
    }

    public void StartTheGame()
    {
        if (audioManager.instance.vibrationBool)
        {
            Handheld.Vibrate();
        }

        foreach (PlayerScript player in allPlayers)
        {
            player.StartGame();
        }

        audioManager.instance.PlayAudio("start", true, Vector3.zero);
    }

    public void restartLevel()
    {
        showLoading();
        StartCoroutine(delayLoadScene("gameplay"));

        AdsManager.Instance.RunInterstitialAd();

        //GA Event
        FirebaseAnalytics.LogEvent("Level_Restart_" + PlayerPrefs.GetInt("currentLevel", 0));
    }

    public void playNextLevel()
    {
        showLoading();
        PlayerPrefs.SetInt("currentLevel", PlayerPrefs.GetInt("currentLevel", 0) + 1);
        StartCoroutine(delayLoadScene("gameplay"));

        AdsManager.Instance.RunInterstitialAd();
    }

    public void OpenMainMenu() 
    {
        showLoading();
        StartCoroutine(delayLoadScene("mainMenu"));

        AdsManager.Instance.RunInterstitialAd();

        //GA Event
        FirebaseAnalytics.LogEvent("Level_Quit_" + PlayerPrefs.GetInt("currentLevel", 0));
    }

    public IEnumerator delayLoadScene(string scene)
    {
        yield return new WaitForSeconds(2f);
        SceneManager.LoadScene(scene);
    }

    public void showLoading()
    {
        Instantiate(loadingPanel);
    }
}