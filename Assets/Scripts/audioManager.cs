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

    public GameObject CurrentMusicObject;

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
        GameObject audioObj = Instantiate(audioPrefab,transform);
        audioObj.GetComponent<audioSourceBehavior>().destroyOnComplete = destroyable;
        
        //Only play one music playback at a time
        if (!destroyable) 
        {
            if (CurrentMusicObject != null) 
            {
                Destroy(CurrentMusicObject);
            }
            CurrentMusicObject = audioObj;
        }

        audioObj.GetComponent<audioSourceBehavior>().spawnTarget = spawnPosition;

        if (audio != null)
        {
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

    public void changeSoundVolume(float newVal) 
    {
        PlayerPrefs.SetFloat("soundVolume", newVal);
    }

    public void changeMusicVolume(float newVal)
    {
        PlayerPrefs.SetFloat("musicVolume", newVal);

        if (CurrentMusicObject != null)
        {
            CurrentMusicObject.GetComponent<AudioSource>().volume = newVal;
        }
    }

    public void stopMusic() 
    {
        if (CurrentMusicObject != null)
        {
            CurrentMusicObject.GetComponent<AudioSource>().Stop();
        }
    }
    
    public void resumeMusic() 
    {
        if (CurrentMusicObject != null)
        {
            CurrentMusicObject.GetComponent<AudioSource>().Play();
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
