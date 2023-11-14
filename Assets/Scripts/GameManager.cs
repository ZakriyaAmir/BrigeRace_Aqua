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

        //Time.timeScale = 0;
    }

    private void Start()
    {
        allPlayers = FindObjectsOfType<PlayerScript>();
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
}
