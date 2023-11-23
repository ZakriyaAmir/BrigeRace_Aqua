using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class WinnerHandler : MonoBehaviour
{
    public TMP_Text winnerText;
    public GameObject resultPanel;

    private void OnTriggerEnter(Collider other)
    {
        AnnounceWinner(other);
    }

    void AnnounceWinner(Collider other)
    {
        if (other.GetComponent<PlayerScript>().isAI)
        {
            GameManager.instance.ShowLosePanel("You have failed to finish first!");
        }
        else 
        {
            GameManager.instance.ShowWinPanel("You Win!");
        }
        gameObject.GetComponent<Collider>().enabled = false;

        GameManager.instance.StopAllPlayers();
    }
}
