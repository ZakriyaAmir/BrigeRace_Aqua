using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.Security.Claims;
using Firebase.Analytics;

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
        claimed = true;
        //Play Reward Ad here
        //Initialize Admob reward callback in the script in which you are required to use rewarded ad for admob
        if (AdsManager.Instance.RunRewardedAd(() => grantReward()))
        {
            Debug.Log("Reward Ad Available");
        }

        else
        {
            Debug.Log("Reward Ad Unavailable");
            grantReward();
        }

        //Initialize Max reward callback in the script in which you are required to use rewarded ad for applovin
        //MaxSdkCallbacks.Rewarded.OnAdReceivedRewardEvent += OnRewardedAdReceivedRewardEvent;
    }

    public void getRewardInterstitial()
    {
        //Initialize Admob reward callback in the script in which you are required to use rewarded ad for admob
        AdsManager.Instance.RunRewardedInterstitialAd(() => grantReward());
        //Initialize Max reward callback in the script in which you are required to use rewarded ad for applovin
        MaxSdkCallbacks.RewardedInterstitial.OnAdReceivedRewardEvent += OnRewardedInterstitialAdReceivedRewardEvent;
    }

    //Rewarded sample callback methods for Applovin Max
    private void OnRewardedAdReceivedRewardEvent(string adUnitId, MaxSdk.Reward reward, MaxSdkBase.AdInfo adInfo)
    {
        grantReward();
        MaxSdkCallbacks.Rewarded.OnAdReceivedRewardEvent -= OnRewardedAdReceivedRewardEvent;
    }
    //Rewarded sample callback methods for Applovin Max
    private void OnRewardedInterstitialAdReceivedRewardEvent(string adUnitId, MaxSdk.Reward reward, MaxSdkBase.AdInfo adInfo)
    {
        grantReward();
        MaxSdkCallbacks.RewardedInterstitial.OnAdReceivedRewardEvent -= OnRewardedInterstitialAdReceivedRewardEvent;
    }

    public void grantReward() 
    {
        
        Debug.Log("Reward Granted");
        audioManager.instance.PlayAudio("win2", true, Vector3.zero);
        GameManager.instance.totalEarnings = finalAmount;
        GameManager.instance.winScore.text = finalAmount.ToString();
        gaugeAnim.enabled = false;
        claimBtn.interactable = false;
        GameManager.instance.claimLevelReward();
        //GA Event
        FirebaseAnalytics.LogEvent("Level_Reward_Multiplier_" + rewardMultiplier);
    }
}
