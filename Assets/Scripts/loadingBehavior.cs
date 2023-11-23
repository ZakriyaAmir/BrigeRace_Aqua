using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class loadingBehavior : MonoBehaviour
{
    public Animator anim;
    public Image loadingBar;
    public RectTransform fishIco;

    private void Awake()
    {
        DontDestroyOnLoad(transform.parent.gameObject);
        anim = GetComponent<Animator>();
        showLoadingBar();
    }

    public void showLoadingBar() 
    {
        anim.SetBool("show",true);
    }

    public void hideLoadingBar()
    {
        anim.SetBool("show", false);
    }

    void Start()
    {
        StartCoroutine(FillLoadingBar());
    }

    IEnumerator FillLoadingBar()
    {
        float timer = 0f;
        float fillDuration = 3f;
        float startFill = 0;
        float endFill = 1f; // 100% fill
        loadingBar.fillAmount = startFill;
        yield return new WaitForSeconds(2f);

        while (timer < fillDuration)
        {
            timer += Time.deltaTime;
            loadingBar.fillAmount = Mathf.Lerp(startFill, endFill, timer / fillDuration);
            if (fishIco.localPosition.x < 625) 
            {
                float xVal = Mathf.Lerp(0, 625, timer / fillDuration);
                fishIco.anchoredPosition = new Vector2(xVal, fishIco.anchoredPosition.y);
            }
            yield return null;
        }

        // Ensure the loading bar is fully filled
        loadingBar.fillAmount = endFill;
        yield return new WaitForSeconds(1f);

        hideLoadingBar();

        yield return new WaitForSeconds(2f);
        Destroy(transform.parent.gameObject);
    }
}
