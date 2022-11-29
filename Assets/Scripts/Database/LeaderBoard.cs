using System;
using UnityEngine;
using UnityEngine.UI;
using LootLocker.Requests;
using System.Runtime.InteropServices.ComTypes;
using System.Collections;
using System.Linq;

public class LeaderBoard : MonoBehaviour
{
    int leaderboardID = 9200;

    internal int minimumHighScore;
    internal int memberScore;
    public Text playerNames;
    public Text playerScores;

    public IEnumerator SubmitScoreRoutine(int scoreToUpload)
    {
        bool done = false;

        string playerID = PlayerPrefs.GetString("PlayerID");
        LootLockerSDKManager.SubmitScore(playerID, scoreToUpload, leaderboardID, (response) =>
        {
            if (response.success)
            {
                print("Successfully uploaded score");
                done = true;
            }
            else
            {
                print("Failed " + response.Error);
                done = true;
            }
        });
        yield return new WaitWhile(() => done == false);
    }

    public IEnumerator FetchHighScoresRoutine()
    {
        bool done = false;
        LootLockerSDKManager.GetScoreList(leaderboardID, 10, 0, (response) =>
        {
            if (response.success)
            {
                string tempPlayerNames = "";
                string tempPlayerScores = "";

                LootLockerLeaderboardMember[] members = response.items;
                for (int i = 0; i < members.Length; i++)
                {
                    tempPlayerNames += members[i].rank + ". ";
                    if (members[i].player.name != "")
                    {
                        tempPlayerNames += members[i].player.name;
                    }
                    else
                    {
                        tempPlayerNames+= members[i].player.id;
                    }
                    tempPlayerScores += members[i].score + "\n";
                    tempPlayerNames += "\n";
                }
                done = true;
                playerNames.text = tempPlayerNames;
                playerScores.text= tempPlayerScores;
            }
            else
            {
                print("Failed " + response.Error);
                done = true;
            }
        });
        yield return new WaitWhile(() => done == false);
    }

    public IEnumerator GetHighScore()
    {
        bool done = false;
        string playerID = PlayerPrefs.GetString("PlayerID");

        LootLockerSDKManager.GetScoreList(leaderboardID, 100, 0, (response) =>
        {
            if (response.success)
            {
                LootLockerLeaderboardMember[] members = response.items;

                minimumHighScore = members.Min(f => f.score);
                
                done = true;
            }
            else
            {
                print("Failed " + response.Error);
                done = true;
            }
        });        
        yield return new WaitWhile(() => done == false);
    }

    public IEnumerator GetLocalHighScore()
    {
        bool done = false;
        string playerID = PlayerPrefs.GetString("PlayerID");

        LootLockerSDKManager.GetMemberRank(leaderboardID, playerID, (response) =>
        {
            if (response.success)
            {
                memberScore = response.score;
                done = true;
            }
            else
            {
                print("Failed " + response.Error);
                done = true;
            }
        });
        yield return new WaitWhile(() => done == false);
    }
}
