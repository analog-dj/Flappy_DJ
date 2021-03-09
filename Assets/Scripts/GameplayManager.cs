using UnityEngine;
using UnityEngine.UI;

public class GameplayManager : MonoBehaviour
{
    public delegate void GameDelegate();
    public static event GameDelegate OnGameStarted;
    public static event GameDelegate OnGameOverConfirmed;
    public static GameplayManager Instance;
    public GameObject startMenu;
    public GameObject gameOverMenu;
    public GameObject countDownMenu;
    public Text scoreText;
    public Text currentScoreText;

    int score = 0;
    bool gameOver = true;

    enum PageState
    {
        None,
        Start,
        GameOver,
        Countdown
    }

    public bool GameOver { get { return gameOver; } }

    public void Awake()
    {
        Instance = this;
    }

    private void OnEnable()
    {
        CountDownText.OnCountdownFinished += OnCountdownFinished;
        BirdController.OnPlayerDied += OnPlayerDied;
        BirdController.OnPlayerScored += OnPlayerScored;
    }
    private void OnDisable()
    {
        CountDownText.OnCountdownFinished -= OnCountdownFinished;
        BirdController.OnPlayerDied -= OnPlayerDied;
        BirdController.OnPlayerScored -= OnPlayerScored;
    }

    void OnCountdownFinished()
    {
        SetPageState(PageState.None);
        OnGameStarted(); //info wyslane do BirdControlera, że czas latać
        score = 0;
        gameOver = false;
    }

    public void OnPlayerDied()
    {
        gameOver = true;
        currentScoreText.text = "Score: " +score.ToString();
        int savedScore = PlayerPrefs.GetInt("HighScore");
        if (score > savedScore)
        {
            PlayerPrefs.SetInt("HighScore", score);
        }
        SetPageState(PageState.GameOver);
    }

    void OnPlayerScored()
    {
        score++;
        scoreText.text = score.ToString();
    }

    void SetPageState(PageState state)
    { 
    switch(state)
        {
            case PageState.None:
                startMenu.SetActive(false);
                gameOverMenu.SetActive(false);
                countDownMenu.SetActive(false);
                break;

            case PageState.Start:
                startMenu.SetActive(true);
                gameOverMenu.SetActive(false);
                countDownMenu.SetActive(false);
                break;

            case PageState.GameOver:
                startMenu.SetActive(false);
                gameOverMenu.SetActive(true);
                countDownMenu.SetActive(false);
                break;

            case PageState.Countdown:
                startMenu.SetActive(false);
                gameOverMenu.SetActive(false);
                countDownMenu.SetActive(true);
                break;
        }
    }

    public void ConfirmedGameOver()          //uruchamia sie when replay button jest kliknięty
    {
        OnGameOverConfirmed();               //event sent BirdControler
        scoreText.text = "0";
        SetPageState(PageState.Start);
    }

    public void StartGame()                   //jak wyżej ale jak play button is pressed
    { 
        SetPageState(PageState.Countdown);
    }
}