using Firebase;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.UI;

public class mainMenu : MonoBehaviour
{
    public GameObject mainPanel;
    public GameObject settingsPanel;
    public GameObject levelsPanel;
    public GameObject loadingPanel;
    public GameObject levelPrefab;
    public Transform levelsParent;
    public int selectedLevel;
    public int maxLevels;
    public GameObject vibrationToggle;
    public GameObject soundToggle;
    public bool vibrationBool;
    public bool soundBool;
    private FirebaseApp app;

    private void Awake()
    {
        Application.targetFrameRate = 120;
        mainPanel.SetActive(true);

        //PlayerPrefs.SetInt("levelsCompleted", 14);
    }

    private void Start()
    {
        checklevels();

        checkVibration();
        checkSound();
    }

    public void openLevelPanel() 
    {
        levelsPanel.SetActive(true);
        levelsPanel.GetComponent<Animator>().SetBool("show", true);
    }

    public void closeLevelPanel()
    {
        levelsPanel.GetComponent<Animator>().SetBool("show", false);
    }

    public void playLevel() 
    {
        PlayerPrefs.SetInt("currentLevel", selectedLevel);
        showLoading();
        StartCoroutine(delayLoadScene("gameplay"));
    }

    public IEnumerator delayLoadScene(string scene)
    {
        yield return new WaitForSeconds(2f);
        SceneManager.LoadScene(scene);
    }

    public void showLoading() 
    {
        Instantiate(loadingPanel);
    }
    

    public void changeSelectedLevel(int levelIndex)
    {
        foreach (Transform level in levelsParent)
        {
            level.transform.GetChild(0).GetChild(1).gameObject.SetActive(false);
        }
        selectedLevel = levelIndex;
        levelsParent.GetChild(levelIndex).GetChild(0).GetChild(1).gameObject.SetActive(true);
    }

    public void checkSettings()
    {
        
    }

    public void checkVibration() 
    {
        if (PlayerPrefs.GetInt("vibration", 0) == 1)
        {
            vibrationToggle.SetActive(true);
            vibrationBool = true;
        }
        else 
        {
            vibrationToggle.SetActive(false);
            vibrationBool = false;
        }
    }

    public void checkSound()
    {
        if (PlayerPrefs.GetInt("sound", 0) == 1)
        {
            soundToggle.SetActive(true);
            soundBool = true;
        }
        else
        {
            soundToggle.SetActive(false);
            soundBool = false;
        }
    }

    public void ToggleVibration() 
    {
        if (PlayerPrefs.GetInt("vibration", 0) == 1)
        {
            PlayerPrefs.SetInt("vibration", 0);
        }
        else
        {
            PlayerPrefs.SetInt("vibration", 1);
        }
        checkVibration();
    }

    public void ToggleSound()
    {
        if (PlayerPrefs.GetInt("sound", 0) == 1)
        {
            PlayerPrefs.SetInt("sound", 0);
        }
        else
        {
            PlayerPrefs.SetInt("sound", 1);
        }
        checkSound();
    }

    public void checklevels() 
    {
        selectedLevel = PlayerPrefs.GetInt("levelsCompleted", 0);
        for (int i = 0; i < maxLevels; i++) 
        {
            GameObject level = Instantiate(levelPrefab, levelsParent);
            level.GetComponent<levelUIScript>().levelID = i;

            if (PlayerPrefs.GetInt("levelsCompleted", 0) > i)
            {
                level.transform.GetChild(0).GetComponent<Button>().enabled = true;
                level.transform.GetChild(0).GetChild(0).gameObject.SetActive(true);
                level.transform.GetChild(0).GetChild(1).gameObject.SetActive(false);
            }
            else if (PlayerPrefs.GetInt("levelsCompleted", 0) == i) 
            {
                level.transform.GetChild(0).GetChild(1).gameObject.SetActive(true);
            }
            else
            {
                level.transform.GetChild(0).GetComponent<Button>().enabled = false;
                level.transform.GetChild(0).GetChild(0).gameObject.SetActive(false);
                level.transform.GetChild(0).GetChild(1).gameObject.SetActive(false);
            }

            //Assign levels UI a number
            level.transform.GetChild(0).GetChild(2).GetComponent<TMP_Text>().text = (i + 1).ToString();

        }
        SnapScrollToTarget(levelsParent.GetChild(selectedLevel).GetComponent<RectTransform>());
    }

    public void SnapScrollToTarget(RectTransform target)
    {
        Canvas.ForceUpdateCanvases();

        var y =
                (Vector2)levelsParent.parent.parent.transform.InverseTransformPoint(levelsParent.GetComponent<RectTransform>().position)
                - (Vector2)levelsParent.parent.parent.transform.InverseTransformPoint(target.position);

        levelsParent.GetComponent<RectTransform>().anchoredPosition = new Vector2(levelsParent.GetComponent<RectTransform>().anchoredPosition.x, y.y - 2000);
    }
}