using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.SceneManagement;
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

        //Reset current level if the total levels count exceeds
        if (PlayerPrefs.GetInt("currentLevel", 0) >= allLevels.Length) 
        {
            Debug.Log("Levels Reset");
            PlayerPrefs.SetInt("currentLevel", 0);
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
    }

    public void pauseGame() 
    {
        pausePanel.SetActive(true);
        pausePanel.GetComponent<Animator>().SetBool("show",true);
        StopAllPlayers();
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
        }
    }

    public void ShowWinPanel(string winText)
    {
        if (audioManager.instance.vibrationBool) 
        {
            Handheld.Vibrate();
        }

        audioManager.instance.PlayAudio("win", true, Vector3.zero);

        winPanel.SetActive(true);
        //winMessage.text = winText;
        totalEarnings = totalBricksCollected * 10;
        winScore.text = totalEarnings .ToString();
        //Save win progress
        if (PlayerPrefs.GetInt("levelsCompleted", 0) <= PlayerPrefs.GetInt("currentLevel", 0)) 
        {
            PlayerPrefs.SetInt("levelsCompleted", PlayerPrefs.GetInt("levelsCompleted", 0) + 1);
        }
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
    }

    public void playNextLevel()
    {
        showLoading();
        PlayerPrefs.SetInt("currentLevel", PlayerPrefs.GetInt("currentLevel", 0) + 1);
        StartCoroutine(delayLoadScene("gameplay"));
    }

    public void OpenMainMenu() 
    {
        showLoading();
        StartCoroutine(delayLoadScene("mainMenu"));
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