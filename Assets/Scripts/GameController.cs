using LootLocker.Requests;
using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;
using UnityEngine.SocialPlatforms.Impl;
using UnityEngine.UI;

public class GameController : MonoBehaviour
{
    public LeaderBoard leaderBoard;


    [Header("Game objects")]
    public GameObject player;
    public GameObject block;
    public GameObject[] buildings;
    public GameObject playerPrefab;

    [Header("UI")]
    public Button button;
    public Button restartButton;
    public Button soundButton;

    public Text lenghtText;
    public Text longestLenghtText;
    public GameObject InfoText;
    public Text newLongestFly;




    public Sprite soundOn;
    public Sprite soundOff;

    public GameObject leaderBoardPanel;
    public GameObject scrollViewContent;
    public GameObject submitPanel;
    public GameObject congratsPanel;
    public InputField nameInputField;
    public GameObject leaderBoardButton;

    [Header("Values")]
    public float spawnValuesMin;
    public float spawnValuesMax;
    public float buildingSpawnValuesMin;
    public float buildingSpawnValuesMax;
    public float spawnTime;
    public float hillSpawnTime;
    public float buildingSpawnTime;
    public int highScore;
    public int localHighScore;

    public int lenght;
    public int longestRun;

    internal bool isDead;
    void Avake()
    {
        UpdateSoundButton(AudioListener.pause);
        DontDestroyOnLoad(gameObject);
    }
    void Start()
    {
        StartCoroutine(SetupRoutine());
        isDead = false;
        player.GetComponent<PlayerController>().moveSpeed = 0;
        GameObject.Find("Cloud").GetComponent<BlockMovement>().moveSpeed = 0;

        lenghtText.text = PlayerPrefs.GetInt("LastFly", lenght).ToString();
        newLongestFly.gameObject.SetActive(false);
        leaderBoardPanel.SetActive(false);
        submitPanel.SetActive(false);
        congratsPanel.SetActive(false);

    }
    void Update()
    {
        UpdateSoundButton(AudioListener.volume == 0);
        if (isDead)
        {
            RestartGame();
        }
        lenghtText.text = lenght.ToString();
    }

    private void RestartGame()
    {
        if (lenght > highScore)
        {
            ShowSubmitPanel();
            StartCoroutine(leaderBoard.FetchHighScoresRoutine());
            PlayerPrefs.SetInt("HighScore", lenght);
            newLongestFly.text = lenght.ToString();
            newLongestFly.gameObject.SetActive(true);
        }
        StopAllCoroutines();
        restartButton.gameObject.SetActive(true);
        leaderBoardButton.gameObject.SetActive(lenght < highScore);
    }

    IEnumerator SpawnBlocks()
    {
        for (int i = 0; i > -1; i++)
        {
            Vector3 spawnPosition = new Vector3(16.5f, UnityEngine.Random.Range(spawnValuesMin, spawnValuesMax), 0);
            Quaternion spawnRotation = Quaternion.identity;

            GameObject go = Instantiate(block, spawnPosition, spawnRotation);
            go.GetComponent<Animator>().Play("Cloud_anim", 0, .5f);
            yield return new WaitForSeconds(spawnTime);
        }
    }
    IEnumerator SpawnBuildings()
    {
        for (int i = 0; i > -1; i++)
        {
            Vector3 spawnPosition = new Vector3(16.5f, UnityEngine.Random.Range(-buildingSpawnValuesMin, -buildingSpawnValuesMax), 0);
            Quaternion spawnRotation = Quaternion.identity;
            Instantiate(buildings[UnityEngine.Random.Range(0, buildings.Length)], spawnPosition, spawnRotation);
            lenght += 1;

            yield return new WaitForSeconds(buildingSpawnTime);
        }
    }
    public void Play()
    {
        if (GameObject.Find("Cloud"))
        {
            GameObject.Find("Cloud").GetComponent<BlockMovement>().moveSpeed = 10;
        }
        InfoText.SetActive(false);
        player.GetComponent<PlayerController>().moveSpeed = 4;
        button.gameObject.SetActive(false);
        leaderBoardButton.gameObject.SetActive(false);
        restartButton.gameObject.SetActive(false);
        Time.timeScale = 1;
        PlayerPrefs.SetInt("LastFly", 0);
        StartCoroutine(SpawnBlocks());
        StartCoroutine(SpawnBuildings());
    }
    public void GameOver()
    {
        SceneManager.LoadScene(0);
    }
    void UpdateSoundButton(bool isPaused)
    {
        if (isPaused)
        {
            soundButton.image.sprite = soundOff;
        }
        else
        {
            soundButton.image.sprite = soundOn;
        }
    }
    public void ShowLeaderboardPanel()
    {
        StartCoroutine(leaderBoard.FetchHighScoresRoutine());
        leaderBoardPanel.SetActive(true);
    }
    public void CloseLeaderboardPanel()
    {
        leaderBoardPanel.SetActive(false);
    }
    public void ShowSubmitPanel()
    {
        submitPanel.SetActive(true);
        leaderBoardButton.SetActive(false);
    }
    public void CloseSubmitPanel()
    {
        submitPanel.SetActive(false);
        leaderBoardButton.SetActive(true);
    }
    public void ShowCongratsPanel()
    {
        congratsPanel.SetActive(true);
    }
    public void CloseCongratsPanel()
    {
        congratsPanel.SetActive(false);
    }

    public void SaveResult()
    {      

        StartCoroutine(leaderBoard.SubmitScoreRoutine(lenght));
        StartCoroutine(SetPlayerName());

        StartCoroutine(leaderBoard.FetchHighScoresRoutine());        
    }
    IEnumerator SetupRoutine()
    {
        yield return LoginRoutine();
        yield return leaderBoard.FetchHighScoresRoutine();
        yield return leaderBoard.GetHighScore();
        yield return leaderBoard.GetLocalHighScore();
        highScore = leaderBoard.minimumHighScore;
        localHighScore = leaderBoard.memberScore;
        longestLenghtText.text = localHighScore.ToString();
    }
    IEnumerator LoginRoutine()
    {
        bool done = false;

        LootLockerSDKManager.StartGuestSession((response) =>
        {
            if (response.success)
            {
                //print("player was logged in");
                PlayerPrefs.SetString("PlayerID", response.player_id.ToString());
                done = true;
            }
            else
            {
                print("Could not start session.");
                done = true;
            }
        });
        yield return new WaitWhile(() => done == false);
    }
    public IEnumerator SetPlayerName()
    {
        bool done = false;
        string playerID = PlayerPrefs.GetString("PlayerID");
        LootLockerSDKManager.SetPlayerName(!string.IsNullOrWhiteSpace(nameInputField.text) ? nameInputField.text : "Guest-" + playerID, (response) =>
        {
            if (response.success)
            {
                print("Succesfully set player name!");
                done = true;
            }
            else
            {
                print("Could not set player name " + response.Error);
                done = true;
            }
        });
        CloseSubmitPanel();
        ShowCongratsPanel();
        yield return new WaitWhile(() => done == false);
    }


}
