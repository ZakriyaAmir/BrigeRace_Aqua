using System.Collections;
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
        yield return new WaitForSeconds(1.2f);
        float firingAngle = 80.0f;
        float gravity = 9.8f;

        // Move projectile to the position of throwing object + add some offset if needed.
        transform.position = transform.position + new Vector3(0, 0.0f, 0);

        // Calculate distance to target
        float target_Distance = Vector3.Distance(transform.position, destination.position);

        // Calculate the velocity needed to throw the object to the target at specified angle.
        float projectile_Velocity = target_Distance / (Mathf.Sin(2 * firingAngle * Mathf.Deg2Rad) / gravity);

        // Extract the X  Y componenent of the velocity
        float Vx = Mathf.Sqrt(projectile_Velocity) * Mathf.Cos(firingAngle * Mathf.Deg2Rad);
        float Vy = Mathf.Sqrt(projectile_Velocity) * Mathf.Sin(firingAngle * Mathf.Deg2Rad);

        // Calculate flight time.
        float flightDuration = target_Distance / Vx;

        // Rotate projectile to face the target.
        transform.rotation = Quaternion.LookRotation(destination.position - transform.position);

        float elapse_time = 0;

        while (elapse_time < flightDuration)
        {
            transform.Translate(0, (Vy - (gravity * elapse_time)) * Time.deltaTime, Vx * Time.deltaTime);

            elapse_time += Time.deltaTime;

            yield return null;
        }

        // Ensure the final position is exactly at targetB
        transform.position = destination.position;

        particle1.SetActive(true);
        audioManager.instance.PlayAudio("waterDrop", true, Vector3.zero);
        yield return new WaitForSeconds(1f);
        audioManager.instance.PlayAudio("win2", true, Vector3.zero);
        particle2.SetActive(true);
    }
}
