using System.Collections;
using TMPro;
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
            GetComponent<MoveScript>().enabled = true;
            GetComponent<MoveScript>().splash1.gameObject.SetActive(true);
            GetComponent<MoveScript>().splash2.gameObject.SetActive(true);
            //GetComponent<MoveScript>().splash3.gameObject.SetActive(true);
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

    public IEnumerator startCelebration(Transform destination, GameObject particle1, GameObject particle2)
    {
        audioManager.instance.stopMusic();

        //Hide fish stack
        GetComponent<StackManager>().stackPoint.SetActive(false);

        float elapsedTime = 0f;
        float lerpTime = 2f;
        Vector3 initialPosition = transform.position;
        while (elapsedTime < lerpTime)
        {
            // Increment the elapsed time
            elapsedTime += Time.deltaTime;

            // Calculate the lerp factor based on elapsed time and lerp time
            float lerpFactor = Mathf.Clamp01(elapsedTime / lerpTime);

            // Lerp the position
            transform.position = Vector3.Lerp(initialPosition, destination.position, lerpFactor);

            // Wait for the next frame
            yield return null;
        }

        // Ensure the final position is exactly the target position
        transform.position = destination.position;

        transform.GetChild(0).GetComponent<Animator>().SetTrigger("win");

        yield return new WaitForSeconds(1.2f);

        particle1.SetActive(true);
        audioManager.instance.PlayAudio("waterDrop", true, Vector3.zero);
        yield return new WaitForSeconds(1f);
        audioManager.instance.PlayAudio("win2", true, Vector3.zero);
        particle2.SetActive(true);
    }
}
