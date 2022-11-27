using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameController : MonoBehaviour
{
    //GameObjects
    public GameObject player;
    public GameObject block;
    public GameObject[] buildings;    

    //UI
    public Button button;
    public Button restartButton;
    public Button soundButton;

    public Text lenghtText;
    public Text longestLenghtText;
    public GameObject InfoText;
    public Text newLongestFly;


    public Sprite soundOn;
    public Sprite soundOff;

    public float spawnValuesMin;
    public float spawnValuesMax;
    public float buildingSpawnValuesMin;
    public float buildingSpawnValuesMax;
    public float spawnTime;
    public float hillSpawnTime;
    public float buildingSpawnTime;
    
    public int lenght;    
    public int longestRun;
    void Avake()
    {
        UpdateSoundButton(AudioListener.pause);
        DontDestroyOnLoad(gameObject);
    }
    void Start()
    {       
        player.GetComponent<PlayerController>().moveSpeed = 0;
        GameObject.Find("Cloud").GetComponent<BlockMovement>().moveSpeed = 0;
        longestLenghtText.text = PlayerPrefs.GetInt("HighScore").ToString();     
        lenghtText.text = PlayerPrefs.GetInt("LastFly", lenght).ToString();
        newLongestFly.gameObject.SetActive(false);
    } 
    void FixedUpdate()
    {
        UpdateSoundButton(AudioListener.volume==0);
        if (!player.gameObject)
        {
            RestartGame();
        }
        lenghtText.text = lenght.ToString();
    }

    private void RestartGame()
    {
        if (lenght > PlayerPrefs.GetInt("HighScore", 0))
        {
            PlayerPrefs.SetInt("HighScore", lenght);
            newLongestFly.text = lenght.ToString();
            newLongestFly.gameObject.SetActive(true);
        }
        StopAllCoroutines();
        restartButton.gameObject.SetActive(true);
    }

    IEnumerator SpawnBlocks()
    {
        for (int i = 0; i > -1 ; i++)
        {
            Vector3 spawnPosition = new Vector3(16.5f, Random.Range(spawnValuesMin, spawnValuesMax), 0);
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
            Vector3 spawnPosition = new Vector3(16.5f, Random.Range(-buildingSpawnValuesMin, -buildingSpawnValuesMax), 0);
            Quaternion spawnRotation = Quaternion.identity;
            Instantiate(buildings[Random.Range(0,buildings.Length)], spawnPosition, spawnRotation);
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
}
