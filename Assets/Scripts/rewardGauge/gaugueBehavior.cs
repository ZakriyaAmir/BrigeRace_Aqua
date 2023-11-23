using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class gaugueBehavior : MonoBehaviour
{
    public int rewardMultiplier;
    public int actualPrize;
    private Animator gaugeAnim;
    public int finalAmount;
    public TMP_Text amountText;
    public Button claimBtn;

    private void Start()
    {
        gaugeAnim = GetComponent<Animator>();
        actualPrize = 1000;
    }

    private void Update()
    {
        finalAmount = rewardMultiplier * actualPrize;
        amountText.text = finalAmount.ToString();
    }

    public void claimReward()
    {
        gaugeAnim.enabled = false;
        claimBtn.interactable = false;
        Debug.Log("Reward Multiplier = " + finalAmount.ToString());
        //Play Reward Ad here
    }

    public void grantReward() 
    {
         
    }
}
