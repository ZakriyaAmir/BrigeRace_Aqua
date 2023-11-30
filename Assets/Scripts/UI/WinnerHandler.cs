using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class WinnerHandler : MonoBehaviour
{
    public Transform centerPoint;
    public GameObject particle1;
    public GameObject particle2;
    public GameObject particle3;
    public Transform camGhost;

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
            //Start celebration animations and particles
            StartCoroutine(other.GetComponent<PlayerScript>().startCelebration(centerPoint, particle1, particle2));
            FindObjectOfType<CameraFollow>().targetTransform = camGhost;
            FindObjectOfType<CameraFollow>().startCelebration();
        }
        gameObject.GetComponent<Collider>().enabled = false;

        GameManager.instance.StopAllPlayers();
    }
}
