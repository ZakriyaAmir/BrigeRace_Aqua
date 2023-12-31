using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class StackManager : MonoBehaviour
{
    [SerializeField] public GameObject stackPoint;
    [SerializeField] private float stackYIncreaseRate = 0.32f;
    [SerializeField] private float stackXposition = -0.18f;

    [SerializeField] public List<GameObject> bricks = new List<GameObject>();
    private bool isPlayer;

    private void Start()
    {
        if (GetComponent<PlayerScript>().isAI)
        {
            isPlayer = false;
        }
        else
        {
            isPlayer = true;
        }
    }

    public void Push(GameObject collectedBrick)
    {
        bricks.Add(collectedBrick);
        MoveToStackAnim(collectedBrick);

        if (isPlayer) 
        {
            GameManager.instance.totalBricksCollected++;
            audioManager.instance.PlayAudio("pick", true, Vector3.zero);
        }
    }
        

    public void Pop()
    {
        var objref = bricks[bricks.Count - 1].gameObject;
        bricks[bricks.Count - 1].gameObject.SetActive(false);
        bricks.RemoveAt(bricks.Count-1);
        Destroy(objref);

        // spawn 1 more brick
        var playerScript = GetComponent<PlayerScript>();
        playerScript.currentlyStandingFloor.transform.GetChild(0).GetComponent<spawnOntoShape>().SpawnOnDemand(playerScript.playerColorIndex);
    }

    public bool isPopable()
    {
        if(bricks.Count > 0)
        {
            return true;
        }
        else
        {
            return false;
        }
    }


    void MoveToStackAnim(GameObject brick)
    {
        Vector3 targetPosition;

        if(bricks.Count == 1)
        {
            targetPosition = new Vector3(stackXposition, stackPoint.transform.localPosition.y, 0);
        }
        else
        {
            targetPosition = new Vector3(stackXposition, (bricks.Count - 1) * stackYIncreaseRate, 0);
        }

        brick.transform.parent = stackPoint.transform;
        brick.transform.DOLocalMove(targetPosition, 0.2f);
        brick.transform.rotation = stackPoint.transform.rotation;
    }
}