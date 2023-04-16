using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using TMPro;
using UnityEngine.SceneManagement;

public enum GameState { GS_PAUSEMENU, GS_GAME, GS_LEVELCOMPLETED, GS_GAME_OVER, GS_OPTIONS };

public class GameManager : MonoBehaviour
{
    public TMP_Text scoreText;
    public TMP_Text counter;
    public TMP_Text killsText;
    public TMP_Text totalScoreText;
    public TMP_Text levelCompletedScoreText;
    public TMP_Text levelCompletedHighScoreText;
    public TMP_Text Quality;
    private int score = 0;
    private int kills = 0;
    public int totalScore = 0;
    public GameState currentGameState = GameState.GS_PAUSEMENU;
    public static GameManager instance;
    public Canvas inGameCanvas;
    public Canvas pauseMenuCanvas;
    public Canvas optionsCanvas;
    public GameObject levelCompletedCanvas;
    public GameObject GameOverCanvas;
    public Image[] keysTab;
    public Image[] hpTab;
    private Color originalcolor;
    private float timer = 0;
    const string keyHighScore = "HighScoreLevel1";
    const string keyHighScoretwo = "HighScoreLevel2";
    // Start is called before the first frame update
    private void Awake()
    {
        Application.targetFrameRate = 1200;
        QualitySettings.vSyncCount = 0;
        InGame();
        instance = this;
        totalScoreText.text = "Score: " + totalScore.ToString();
        scoreText.text = score.ToString();
        killsText.text = kills.ToString();
        counter.text = string.Format("{0:00}:{1:00}", (int)timer / 60, (int)timer % 60);
        originalcolor = keysTab[0].color;
        if (!PlayerPrefs.HasKey(keyHighScore))
        {
            PlayerPrefs.SetInt(keyHighScore, 0);
        }
        if (!PlayerPrefs.HasKey(keyHighScoretwo))
        {
            PlayerPrefs.SetInt(keyHighScoretwo, 0);
        }
        for (int i = 0; i < keysTab.Length; i++)
        {
            keysTab[i].color = Color.gray;
        }
        Quality.text = QualitySettings.names[QualitySettings.GetQualityLevel()];
        //levelCompletedCanvas.enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime;
        counter.text = string.Format("{0:00}:{1:00}", (int)timer / 60, (int)timer % 60);
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (currentGameState == GameState.GS_PAUSEMENU)
            {
                InGame();
            }
            else if (currentGameState == GameState.GS_GAME)
            {
                PauseMenu();
                Time.timeScale = 0;
            }
        }
    }

    void SetGameState(GameState newGameState)
    {
        Debug.Log(newGameState);
        if (newGameState == GameState.GS_GAME)
        {
            inGameCanvas.enabled = true;
            pauseMenuCanvas.enabled = false;
            optionsCanvas.enabled = false;
            levelCompletedCanvas.SetActive(false);
            GameOverCanvas.SetActive(false);
        }
        else if (newGameState == GameState.GS_PAUSEMENU)
        {
            pauseMenuCanvas.enabled = true;
        }
        else if (newGameState == GameState.GS_LEVELCOMPLETED)
        {
            Scene currentScene = SceneManager.GetActiveScene();
            if (currentScene.name == "Level1")
            {
                int highScore = PlayerPrefs.GetInt(keyHighScore);
                if (highScore < totalScore)
                {
                    highScore = totalScore;
                    PlayerPrefs.SetInt(keyHighScore, highScore);
                }
                levelCompletedHighScoreText.text = "Highscore: " + PlayerPrefs.GetInt(keyHighScore).ToString();
            }
            else if (currentScene.name == "Level2")
            {
                int highScore = PlayerPrefs.GetInt(keyHighScoretwo);
                if (highScore < totalScore)
                {
                    highScore = totalScore;
                    PlayerPrefs.SetInt(keyHighScoretwo, highScore);
                }
                levelCompletedHighScoreText.text = "Highscore: " + PlayerPrefs.GetInt(keyHighScoretwo).ToString();
            }
            levelCompletedCanvas.SetActive(true);
        }
        else if (newGameState == GameState.GS_GAME_OVER)
        {
            GameOverCanvas.SetActive(true);
        }
        else if (newGameState == GameState.GS_OPTIONS)
        {
            optionsCanvas.enabled = true;
            Time.timeScale = 0;
        }
        currentGameState = newGameState;

    }

    public void PauseMenu()
    {
        SetGameState(GameState.GS_PAUSEMENU);
    }

    public void Options()
    {
        SetGameState(GameState.GS_OPTIONS);
    }

    public void InGame()
    {
        SetGameState(GameState.GS_GAME);
        Time.timeScale = 1;
        /*
        GameObject[] allObjects = Object.FindObjectsOfType<GameObject>();
        foreach (GameObject obj in allObjects)
        {
            Animator animator = obj.GetComponent<Animator>();
            if(animator!=null)
            {
                animator.enabled = true;
            }
        }
        */
    }

    public void LevelCompleted()
    {
        SetGameState(GameState.GS_LEVELCOMPLETED);
        levelCompletedScoreText.text = "Score: " + totalScore.ToString();
    }

    public void GameOver()
    {
        SetGameState(GameState.GS_GAME_OVER);
    }

    public void AddPoints(int points)
    {
        score = score + points;
        scoreText.text = score.ToString();
    }

    public void AddTotalScore(Action action)
    {
        if (action == Action.kill)
        {
            totalScore += 15;
        }
        if (action == Action.collect)
        {
            totalScore += 10;
        }
        totalScoreText.text = "Score: " + totalScore.ToString();
    }

    public void AddKills()
    {
        kills++;
        killsText.text = kills.ToString();
    }

    public void AddKeys()
    {
        keysTab[FoxController.keysFound].color = originalcolor;
        FoxController.keysFound++;
    }

    public void AddHP()
    {
        hpTab[(FoxController.lives) - 1].enabled = true;
    }

    public void LostHP()
    {
        hpTab[(FoxController.lives)].enabled = false;
    }

    public void OnResumeButtonClicked()
    {
        InGame();
    }

    public void OnOptionsButtonClicked()
    {
        Options();
    }

    public void OnMinusButtonClicked()
    {
        QualitySettings.DecreaseLevel();
        Quality.text = QualitySettings.names[QualitySettings.GetQualityLevel()];
    }

    public void OnPlusButtonClicked()
    {
        QualitySettings.IncreaseLevel();
        Quality.text = QualitySettings.names[QualitySettings.GetQualityLevel()];
    }

    public void OnRestartButtonClicked()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void OnReturnToMainMenuButtonClicked()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene("MainMenu");
    }

    public void OnLevel2ButtonPressed()
    {
        SceneManager.LoadSceneAsync("Level2");
    }
    /*
    public void TurnOffAnim()
    {
        GameObject[] allObjects = Object.FindObjectsOfType<GameObject>();
        foreach (GameObject obj in allObjects)
        {
            Animator animator = obj.GetComponent<Animator>();
            if (animator != null)
            {
                animator.enabled = false;
            }
        }
    }
    */
}