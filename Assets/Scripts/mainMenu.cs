using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class mainMenu : MonoBehaviour
{
    public GameObject mainPanel;
    public GameObject levelsPanel;
    public GameObject loadingPanel;

    private void Awake()
    {
        Application.targetFrameRate = 120;

        levelsPanel.SetActive(false);
        mainPanel.SetActive(true);
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

    public void playLevel(int index) 
    {
        PlayerPrefs.SetInt("currentLevel", index);
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
}