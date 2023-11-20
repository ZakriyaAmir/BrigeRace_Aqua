using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public GameObject winPanel;
    public GameObject losePanel;
    public GameObject player;
    public PlayerScript[] allPlayers;
    public levelBehavior[] allLevels;
    public GameObject[] playerPrefabs;
    public GameObject[] enemyPrefabs;
    public levelBehavior currentLevel;
    public GameObject loadingPanel;

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
        currentLevel = Instantiate(allLevels[PlayerPrefs.GetInt("currentLevel", 0)]).GetComponent<levelBehavior>();
        //Spawn all enemies
        for (int i = 0; i < currentLevel.totalEnemies; i++) 
        {
            GameObject enemy = Instantiate(enemyPrefabs[i]);
        }
    }

    private void Start()
    {
        allPlayers = FindObjectsOfType<PlayerScript>();
        StopAllPlayers();
        currentLevel.AssignUniqueSpawnPositions();
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
        if (player.transform.position.y < -15)
        {
            losePanel.SetActive(true);
        }
    }

    public void ShowWinPanel(string winText)
    {
        winPanel.transform.GetChild(0).gameObject.GetComponent<TMP_Text>().text = winText;
        winPanel.SetActive(true);
    }

    public void ShowLosePanel(string loseText)
    {
        winPanel.transform.GetChild(0).gameObject.GetComponent<TMP_Text>().text = loseText;
        winPanel.SetActive(false);
        losePanel.SetActive(true);
    }

    public void StartTheGame()
    {
        foreach (PlayerScript player in allPlayers)
        {
            player.StartGame();
        }
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
