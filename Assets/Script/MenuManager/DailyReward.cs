
using System;
using TMPro;
using UnityEngine;

public class DailyReward : MonoBehaviour
{
    public int[] dailyRewards = { 100, 200, 300, 400, 500, 600, 700 }; // Array of daily reward amounts for 7 days
    public string lastRewardDateKey = "LastRewardDate"; // Key to store last reward date in PlayerPrefs
    public string consecutiveDaysKey = "ConsecutiveDays"; // Key to store consecutive login days in PlayerPrefs
    public TMP_Text rewardTest;
    
    void Start()
    {
        /*DateTime currentDate = DateTime.Today;
        DateTime lastRewardDate;

        if (PlayerPrefs.HasKey(lastRewardDateKey))
        {
            lastRewardDate = DateTime.Parse(PlayerPrefs.GetString(lastRewardDateKey));
        }
        else
        {
            lastRewardDate = currentDate.AddDays(-1); // Set to yesterday if no last reward date exists
        }

        if (currentDate.Date > lastRewardDate.Date)
        {
            // Player logged in after last reward date
            if (lastRewardDate.Date.AddDays(1) == currentDate.Date)
            {
                // Player logged in consecutively
                int consecutiveDays = PlayerPrefs.GetInt(consecutiveDaysKey, 0) + 1;

                if (consecutiveDays > dailyRewards.Length)
                {
                    consecutiveDays = 1; // Reset consecutive days if more than 7 days have passed
                }

                // Grant reward for the current day
                GrantDailyReward(consecutiveDays);
                
                PlayerPrefs.SetInt(consecutiveDaysKey, consecutiveDays);
            }
            else
            {
                // Player logged in after missing a day
                GrantDailyReward(1);
                PlayerPrefs.SetInt(consecutiveDaysKey, 1);
            }

            // Save today's date as the last reward date
            PlayerPrefs.SetString(lastRewardDateKey, currentDate.ToString("yyyy-MM-dd"));
        }
        else
        {
            // Player already claimed the reward for today
            Debug.Log("You've already claimed your reward for today.");
            rewardTest.text = "You've already claimed your reward for today.";
        }*/
    }

    void GrantDailyReward(int consecutiveDays)
    {
        // Grant the daily reward
        int rewardAmount = dailyRewards[consecutiveDays - 1];
        Debug.Log("You've earned a daily reward of " + rewardAmount + " coins for logging in " + consecutiveDays + " consecutive days!");
        rewardTest.text = "You've earned a daily reward of " + rewardAmount + " coins for logging in " + consecutiveDays + " consecutive days!";
        // Add the reward to the player's account (you can modify this to fit your game's currency system)
        // For example, if you have a currency manager:
        // CurrencyManager.Instance.AddCoins(rewardAmount);
    }

    public void ClaimReward()
    {
        DateTime currentDate = DateTime.Today;
        DateTime lastRewardDate;

        if (PlayerPrefs.HasKey(lastRewardDateKey))
        {
            lastRewardDate = DateTime.Parse(PlayerPrefs.GetString(lastRewardDateKey));
        }
        else
        {
            lastRewardDate = currentDate.AddDays(-1); // Set to yesterday if no last reward date exists
        }

        if (currentDate.Date > lastRewardDate.Date)
        {
            // Player logged in after last reward date
            if (lastRewardDate.Date.AddDays(1) == currentDate.Date)
            {
                // Player logged in consecutively
                int consecutiveDays = PlayerPrefs.GetInt(consecutiveDaysKey, 0) + 1;

                if (consecutiveDays > dailyRewards.Length)
                {
                    consecutiveDays = 1; // Reset consecutive days if more than 7 days have passed
                }

                // Grant reward for the current day
                GrantDailyReward(consecutiveDays);
                
                PlayerPrefs.SetInt(consecutiveDaysKey, consecutiveDays);
            }
            else
            {
                // Player logged in after missing a day
                GrantDailyReward(1);
                PlayerPrefs.SetInt(consecutiveDaysKey, 1);
            }

            // Save today's date as the last reward date
            PlayerPrefs.SetString(lastRewardDateKey, currentDate.ToString("yyyy-MM-dd"));
        }
        else
        {
            // Player already claimed the reward for today
            Debug.Log("You've already claimed your reward for today.");
            rewardTest.text = "You've already claimed your reward for today.";
        }
    }
}
