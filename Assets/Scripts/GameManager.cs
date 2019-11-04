using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour {
    private static GameManager instance;
    public static GameManager Instance {
        get { return instance; }
        set { instance = value; }
    }
    public AudioClip dieClip;
    public int level = 1;
    private int initFood = 100;
    public int food = 100;
    public float turnDelay = .1f;
    public List<Enemy> enemys = new List<Enemy> ();
    public bool playersTurn = true;
    private bool enemiesMoving;
    private Text foodText;
    private Text failedText;
    private Image levelImage;
    private Text levelText;
    private MapManager mapManager;

    void Awake () {
        if (instance == null) {
            instance = this;
        } else if (instance != this) {
            Destroy (gameObject);
        }
        DontDestroyOnLoad (gameObject);
        
        mapManager = GetComponent<MapManager> ();
        // InitGame ();
    }

    public void ReduceFood (int count) {
        food -= count;
        checkGameOver ();
    }

    public void AddFood (int Count) {
        food += Count;
    }

    void Update () {
        foodText.text = "Food: " + food;
        if (playersTurn || enemiesMoving)
            return;

        StartCoroutine (MoveEnemies ());
    }

    private void checkGameOver () {
        if (food < 0) {
            AudioManager.Instance.RandomPlay (dieClip);
            AudioManager.Instance.StopBGM ();
            gameOver ();
        }
    }

    public void Restart () {
        SceneManager.LoadScene (0);
    }

    private void gameOver () {
        failedText.enabled = true;
        failedText.text = "After " + level + " days, you starved.";
        enabled = false;
    }

    IEnumerator MoveEnemies () {
        enemiesMoving = true;
        yield return new WaitForSeconds (turnDelay);
        if (enemys.Count == 0) {
            yield return new WaitForSeconds (turnDelay);
        }
        foreach (Enemy enemy in enemys) {
            enemy.Move ();
            yield return new WaitForSeconds (enemy.moveTime);
        }

        playersTurn = true;
        enemiesMoving = false;
    }

    void OnEnable () {
        SceneManager.sceneLoaded += OnLevelFinishedLoading;
    }

    void OnDisable () {
        SceneManager.sceneLoaded -= OnLevelFinishedLoading;
    }

    void OnLevelFinishedLoading (Scene scene, LoadSceneMode mode) {
        level++;
        food = initFood;
        InitGame ();
    }

    private void InitGame () {
        foodText = GameObject.Find ("FoodText").GetComponent<Text> ();
        foodText.text = "Food:" + food;
        failedText = GameObject.Find ("FailedText").GetComponent<Text> ();
        failedText.enabled = false;
        levelImage = GameObject.Find ("LevelImage").GetComponent<Image> ();
        foodText.text = "Food:" + food;
        levelText = GameObject.Find ("LevelText").GetComponent<Text> ();
        levelText.text = "DAY " + level;
        Invoke ("HidelevelImage", 1f);
        enemys.Clear ();
        mapManager.SetupScene (level);
    }

    private void HidelevelImage () {
        levelImage.gameObject.SetActive (false);
    }
}