using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ScoreHandler : MonoBehaviour, IDataPersistence
{
    private const int SCOREPERUNIT = 10;
    private const int SCOREPERCOIN = 50;
    private readonly int[] LEVEL_UP_VALUES = {1000, 2000, 3500, 5000, 7000, 10000, 12500, 15000, 20000};
    [SerializeField] TMP_Text currentScoreText;
    [SerializeField] TMP_Text highScoreText;
    [SerializeField] GameObject player;
    [SerializeField] GameObject corruption;
    [SerializeField] GameObject initZone;
    private Vector2 pStartPos;
    
    private int maxY = 0;
    private int coinCount = 0;
    private int score = 0;
    private int highScore = 1000;
    public int level = 0;
    public List<GameObject> destroyOnReset = new List<GameObject>(0);
    [SerializeField] bool arcade = false;
    [SerializeField] GameObject gameOverScreen;
    private void UpdateScore()
    {
        score = maxY * SCOREPERUNIT + coinCount * SCOREPERCOIN;
        currentScoreText.SetText("Score: " + $"{score:000000}");
        if(score > highScore)
        {
            highScoreText.color = Color.yellow;
            highScore = score;
        }
        highScoreText.SetText("Highscore: " + $"{highScore:000000}");
        if(score > LEVEL_UP_VALUES[level])
        {
            level++;
        }
    }
    public void AddToZones(GameObject z)
    {
        destroyOnReset.Add(z);
    }
    private void DestroyZones()
    {
        for(int i = 0; i < destroyOnReset.Count; i++)
        {
            Destroy(destroyOnReset[i]);
        }
        destroyOnReset = new List<GameObject>(0);
    }
    public void GameOver()
    {
        if(arcade)
        {
            gameOverScreen.SetActive(true);
            player.SetActive(false);
        }
        else
        {
            Reset();
        }
    }
    public void Reset()
    {
        score = 0;
        level = 0;
        maxY = 0;
        coinCount = 0;
        highScoreText.color = Color.white;
        UpdateScore();
        corruption.GetComponent<Corruption>().Reset();
        player.transform.position = pStartPos;
        DestroyZones();
        AddToZones(Instantiate(initZone, new Vector3(0, 50, 1), Quaternion.identity));
    }
    public void CollectCoin(int value)
    {
        coinCount += value;
        UpdateScore();
    }
    // Start is called before the first frame update
    void Start()
    {
        pStartPos = player.transform.position;
        Reset();
    }

    // Update is called once per frame
    void Update()
    {
        int playerY = (int) player.transform.position.y;
        if(playerY >= maxY + 1)
        {
            maxY = playerY;
            UpdateScore();
        }
    }
    public void SaveData(ref SaveData data)
    {
        data.highScore = this.highScore;
    }
    public void LoadData(SaveData data)
    {
        this.highScore = data.highScore;
        highScoreText.SetText("Highscore: " + $"{highScore:000000}");
    }
}
