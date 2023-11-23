using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.Security.Claims;

public class gaugueBehavior : MonoBehaviour
{
    public int rewardMultiplier;
    public int actualPrize;
    private Animator gaugeAnim;
    public int finalAmount;
    public TMP_Text amountText;
    public Button claimBtn;
    public bool claimed;

    private void Start()
    {
        gaugeAnim = GetComponent<Animator>();
        actualPrize = GameManager.instance.totalEarnings;
    }

    private void FixedUpdate()
    {
        if (!claimed)
        {
            finalAmount = rewardMultiplier * actualPrize;
            amountText.text = finalAmount.ToString();
        }
    }

    public void claimReward()
    {
        //Play Reward Ad here
        claimed = true;
        grantReward();
    }

    public void grantReward() 
    {
        GameManager.instance.totalEarnings = finalAmount;
        GameManager.instance.winScore.text = finalAmount.ToString();
        gaugeAnim.enabled = false;
        claimBtn.interactable = false;
    }
}
