using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static GameManager;

public class audioManager : MonoBehaviour
{
    public List<audioFile> audioFiles;
    public GameObject audioPrefab;

    public bool vibrationBool;
    public bool soundBool;

    public static audioManager instance;

    [Serializable]
    public class audioFile
    {
        public string name;
        public AudioClip sound;
    }

    private void Awake()
    {
        checkVibration();
        checkSound();
        DontDestroyOnLoad(gameObject);
        if (instance != null)
        {
            Destroy(gameObject);
        }
        else
        {
            instance = this;
        }
    }

    public void PlayAudio(string soundName, bool destroyable, Vector3 spawnPosition)
    {
        // Find the AudioFile with the specified name in the list
        if (!soundBool)
        {
            return;
        }

        audioFile audio = audioFiles.Find(file => file.name == soundName);

        //Spawn an audio object prefab
        GameObject audioObj = Instantiate(audioPrefab);
        audioObj.GetComponent<audioSourceBehavior>().destroyOnComplete = destroyable;
        audioObj.GetComponent<audioSourceBehavior>().spawnTarget = spawnPosition;

        if (audio != null)
        {
            Debug.Log("Zak = " + audio.sound);
            audioObj.GetComponent<audioSourceBehavior>().clip = audio.sound;
        }
        else
        {
            Debug.LogError($"AudioFile with name {soundName} not found.");
        }
    }

    public void checkVibration()
    {
        if (PlayerPrefs.GetInt("vibration", 1) == 1)
        {
            vibrationBool = true;
        }
        else
        {
            vibrationBool = false;
        }
    }

    public void checkSound()
    {
        if (PlayerPrefs.GetInt("sound", 1) == 1)
        {
            soundBool = true;
        }
        else
        {
            soundBool = false;
        }
    }
}
