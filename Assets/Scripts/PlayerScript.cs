using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class PlayerScript : MonoBehaviour
{
    public Color playerColor;
    public int playerColorIndex;
    public GameObject currentlyStandingFloor;
    public bool isAI;

    public void StartGame() 
    {
        this.enabled = true;
        if (isAI)
        {
            GetComponent<NavMeshAgent>().enabled = true;
            GetComponent<Rigidbody>().isKinematic = false;
            GetComponent<AIController>().enabled = true;
        }
        else
        {
            GetComponent<MoveScript>().controller.enabled = true;
        }
    }

    private void Update()
    {
        FindCurrentlyStandingFloor();
    }

    void FindCurrentlyStandingFloor()
    {
        RaycastHit hit;
        if (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.down), out hit, Mathf.Infinity))
        {
            if (hit.transform.gameObject != currentlyStandingFloor)
            {
                if (hit.transform.gameObject.layer == LayerMask.NameToLayer("Ground"))
                {
                    currentlyStandingFloor = hit.transform.gameObject;

                    if (isAI) 
                    {
                        gameObject.GetComponent<AIController>().SetCurrentlyStandingFloor();
                    }
                }
            }
        }
    }
}
