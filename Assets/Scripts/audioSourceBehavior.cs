using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class audioSourceBehavior : MonoBehaviour
{
    public bool destroyOnComplete;
    public Vector3 spawnTarget;
    public AudioSource audioSource;
    public AudioClip clip;

    // Start is called before the first frame update
    void Start()
    {
        if (spawnTarget != null) 
        {
            transform.position = spawnTarget;
        }

        audioSource.clip = clip;
        audioSource.Play();

        if (destroyOnComplete)
        {
            StartCoroutine(DestroyGameObjectAfterAudioPlayback());
        }
    }

    private IEnumerator DestroyGameObjectAfterAudioPlayback()
    {
        // Wait for the audio playback to complete
        yield return new WaitForSeconds(audioSource.clip.length);

        // Destroy the GameObject
        Destroy(gameObject);
    }
}
