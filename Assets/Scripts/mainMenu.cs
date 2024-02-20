using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
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
    public GameObject consentPanel;
    public GameObject levelPrefab;
    public Transform levelsParent;
    public TMP_Text totalMoney;
    public int selectedLevel;
    public int maxLevels;
    public GameObject vibrationToggle;
    public GameObject soundToggle;

    public static mainMenu Instance;

    private void Awake()
    {
        Instance = this;

        Application.targetFrameRate = 120;
        mainPanel.SetActive(true);

        //PlayerPrefs.SetInt("levelsCompleted", 50);
    }

    public void acceptConsent() 
    {
        PlayerPrefs.SetInt("consent", 1);
        consentPanel.SetActive(false);
        AdsManager.Instance.Invoke("RunBannerAd", 4f);
    }

    public void rejectConsent()
    {
        PlayerPrefs.SetInt("consent", 0);
        Application.Quit();
    }

    private void Start()
    {
        checkVibration();
        checkSound();

        checklevels();
        updateMoney();
        audioManager.instance.PlayAudio("menuBGM", false, Vector3.zero);

        if (PlayerPrefs.GetInt("consent", 0) == 0)
        {
            consentPanel.SetActive(true);
        }
        else
        {
            AdsManager.Instance.Invoke("RunBannerAd", 4f);
        }
    }

    public void clearLevels() 
    {
        foreach (Transform level in levelsParent) 
        {
            Destroy(level.gameObject);
        }
    }

    public void updateMoney() 
    {
        // Convert the integer to a string with "N0" format specifier
        string formattedNumber = string.Format("{0:N0}", economyManager.Instance.money);
        totalMoney.text = formattedNumber.ToString();
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

    public void OpenURL(string url) 
    {
        Application.OpenURL(url);
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

    public void checkVibration() 
    {
        if (PlayerPrefs.GetInt("vibration", 1) == 1)
        {
            vibrationToggle.SetActive(true);
            audioManager.instance.vibrationBool = true;
        }
        else 
        {
            vibrationToggle.SetActive(false);
            audioManager.instance.vibrationBool = false;
        }
    }

    public void checkSound()
    {
        if (PlayerPrefs.GetInt("sound", 1) == 1)
        {
            soundToggle.SetActive(true);
            audioManager.instance.soundBool = true;
        }
        else
        {
            soundToggle.SetActive(false);
            audioManager.instance.soundBool = false;
        }
    }

    public void ToggleVibration() 
    {
        if (PlayerPrefs.GetInt("vibration", 1) == 1)
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
        if (PlayerPrefs.GetInt("sound", 1) == 1)
        {
            PlayerPrefs.SetInt("sound", 0);
            audioManager.instance.stopMusic();
        }
        else
        {
            PlayerPrefs.SetInt("sound", 1);

            checkSound();
            if (audioManager.instance.CurrentMusicObject != null)
            {
                audioManager.instance.resumeMusic();
            }
            else 
            {
                audioManager.instance.PlayAudio("menuBGM", false, Vector3.zero);
            }
        }
        checkSound();

        AdsManager.Instance.RunInterstitialAd();
    }

    public void checklevels()
    {
        if (PlayerPrefs.GetInt("levelsCompleted", 0) < maxLevels)
        {
            selectedLevel = PlayerPrefs.GetInt("levelsCompleted", 0);
        }
        else 
        {
            selectedLevel = maxLevels - 1;
        }

        for (int i = 0; i < maxLevels; i++) 
        {
            GameObject level = Instantiate(levelPrefab, levelsParent);
            level.GetComponent<levelUIScript>().levelID = i;

            if (selectedLevel > i)
            {
                level.transform.GetChild(0).GetComponent<Button>().enabled = true;
                level.transform.GetChild(0).GetChild(0).gameObject.SetActive(true);
                level.transform.GetChild(0).GetChild(1).gameObject.SetActive(false);
            }
            else if (selectedLevel == i) 
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