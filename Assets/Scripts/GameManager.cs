using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public GameObject winPanel;
    public GameObject losePanel;
    public GameObject player;

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

        Time.timeScale = 0;
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
        Time.timeScale = 1;
    }
}
