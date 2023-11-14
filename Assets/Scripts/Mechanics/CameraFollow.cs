using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform target;
    public Vector3 offset;

    private void LateUpdate()
    {
        //Player is not lose
        if (target.transform.position.y > -15)
        {
            transform.position = target.position + offset;
        }
    }

}
