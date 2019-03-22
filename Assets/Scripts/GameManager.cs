using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class GameManager : MonoBehaviour {

    public int level = 1;//当前关卡
    public int energy = 100;
    public AudioClip dieAudio;

    private static GameManager _instance;
    private Player player;
    private MapManager mapManager;
    private bool sleepStep=true;
    private bool isWin = false;
    private Text energyText;
    private Image failImage;
    private Image DayImage;
    private Text DayText;
     
    public static GameManager Instance
    {
        get { return _instance;}
    }

    public List<Enemy> enemyList = new List<Enemy>();

    void Awake()
    {
        _instance = this;
        DontDestroyOnLoad(this.gameObject);
        InitGame();  
    }

    void InitGame()
    {
        mapManager = GetComponent<MapManager>();
        print("InitGame");
        mapManager.InitMap();
        energyText = GameObject.Find("EnergyText").GetComponent<Text>();
        failImage = GameObject.Find("FailImage").GetComponent<Image>();
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
        DayImage = GameObject.Find("DayImage").GetComponent<Image>();
        DayText = GameObject.Find("DayText").GetComponent<Text>();
        DayText.text = "Day: " + level;
        Invoke("HideDayInf", 1);

        failImage.gameObject.SetActive(false);
        isWin = true;
        enemyList.Clear();
    }

    void HideDayInf()
    {
        DayImage.gameObject.SetActive(false);
    }

    void UpdateEnergy(int change)
    {
        if (change > 0)
        {
            energyText.text = "Energy: " + energy + "  +" + change;
        }else
        {
            energyText.text = "Energy: " + energy+"  "+change;
        }
    }

	public void AddEnergy(int count)
    {
        energy += count;
        UpdateEnergy(count);
    }

    public void LoseEnergy(int count)
    {
        energy -= count;
        UpdateEnergy(-count);
        if (energy <= 0)
        {
            failImage.gameObject.SetActive(true);
            AudioManager.Instance.RandomPlay(dieAudio);
            AudioManager.Instance.EndBGM();
        }
    }

    public void OnPlayerMove()
    {
        if (sleepStep)
        {
            sleepStep = false;
        }else
        {
            foreach(var enemy in enemyList)
            {
                enemy.Move();
            }
            sleepStep = true;
        }
        if (player.targetPosition.x == mapManager.cols - 2 && player.targetPosition.y == mapManager.rows - 2)
        {
            isWin = true;
            Application.LoadLevel(Application.loadedLevel);//Load this round again
        }
    }

    void OnLevelWasLoaded(int sceneLevel)
    {
        level++;
        InitGame();
    }
}
