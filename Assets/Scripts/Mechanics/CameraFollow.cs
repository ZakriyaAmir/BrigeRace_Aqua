using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform target;
    public Vector3 offset;

    public bool isCelebrating;
    public Transform targetTransform;
    public float lerpSpeed = 2.0f;

    private void LateUpdate()
    {
        //Player is not lose
        if (!isCelebrating)
        {
            if (target.transform.position.y > -15)
            {
                transform.position = target.position + offset;
            }
        }
    }

    public void startCelebration()
    {
        isCelebrating = true;
        StartCoroutine(lerpToCelebrate());
    }

    IEnumerator lerpToCelebrate()
    {
        yield return new WaitForSeconds(1.6f);
        while (Vector3.Distance(transform.position, targetTransform.position) > 0.01f || Quaternion.Angle(transform.rotation, targetTransform.rotation) > 0.01f)
        {
            // Lerp position
            transform.position = Vector3.Lerp(transform.position, targetTransform.position, lerpSpeed * Time.deltaTime);

            // Lerp rotation
            transform.rotation = Quaternion.Lerp(transform.rotation, targetTransform.rotation, lerpSpeed * Time.deltaTime);

            yield return null; // Wait for the next frame
        }

        // Ensure the final transform matches the target precisely
        transform.position = targetTransform.position;
        transform.rotation = targetTransform.rotation;

        yield break;
    }

}
