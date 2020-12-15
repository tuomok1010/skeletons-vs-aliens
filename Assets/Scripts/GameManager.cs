using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public enum GameState
    {
        MENU,
        GAME,
        PAUSE,
        END,
        EXIT,
        MENU_TO_GAME_TRANSITION,
        GAME_TO_MENU_TRANSITION
    }

    public enum PlayerFaction
    {
        NONE = 0,
        SKELETONS = 1,
        ALIENS = 2
    }

    [System.Serializable] public struct GameSettings
    {
        public int gameTimeInSeconds;
    }

    public struct PlayerInfo
    {
        public int score;
        public int nNormalCowsCaptured;
        public int nFreezingCowsCaptured;
        public int nHotCowsCaptured;
        public int nNitroCowsCaptured;
        public int nCloudCowsCaptured;
    }

    public static GameState gameState;
    public static PlayerInfo skeletons;
    public static PlayerInfo aliens;
    public static int gameTimer;
    public static int gameTimeInSeconds;

    public GameSettings gameSettings;

    float timeElapsedInGame;

    private void Awake()
    {
        gameState = GameState.MENU;
        gameTimeInSeconds = gameSettings.gameTimeInSeconds;
        timeElapsedInGame = 0.0f;
        gameTimer = gameSettings.gameTimeInSeconds;

        skeletons.score = 0;
        skeletons.nNormalCowsCaptured = 0;
        skeletons.nFreezingCowsCaptured = 0;
        skeletons.nHotCowsCaptured = 0;
        skeletons.nNitroCowsCaptured = 0;
        skeletons.nCloudCowsCaptured = 0;

        aliens.score = 0;
        aliens.nNormalCowsCaptured = 0;
        aliens.nFreezingCowsCaptured = 0;
        aliens.nHotCowsCaptured = 0;
        aliens.nNitroCowsCaptured = 0;
        aliens.nCloudCowsCaptured = 0;
    }

    // Start is called before the first frame update
    void Start()
    {
        gameState = GameManager.GameState.MENU;
        SceneManager.LoadScene(1);
    }

    // Update is called once per frame
    void Update()
    {
        if (gameState == GameState.GAME)
            RunGameLogic();
        else if (gameState == GameState.PAUSE)
            RunPauseLogic();
        else if (gameState == GameState.EXIT)
            RunExitLogic();
    }

    void RunGameLogic()
    {
        if (Input.GetKeyDown(KeyCode.P) || Input.GetKeyDown(KeyCode.Escape))
        {
            gameState = GameState.PAUSE;
            PauseGame();
        }
        else
        {
            timeElapsedInGame += Time.deltaTime;

            if (timeElapsedInGame >= 1.0f)
            {
                --gameTimer;
                timeElapsedInGame = 0.0f;

                if (gameTimer < 0)
                {
                    gameState = GameState.GAME_TO_MENU_TRANSITION;
                }
            }
        }
    }

    void RunPauseLogic()
    {
        if (Input.GetKeyDown(KeyCode.P) || Input.GetKeyDown(KeyCode.Escape))
        {
            gameState = GameState.GAME;
            UnpauseGame();
        }
    }

    void RunExitLogic()
    {
        ExitGame();
    }

    public static void PauseGame()
    {
        Time.timeScale = 0;
    }

    public static void UnpauseGame()
    {
        Time.timeScale = 1;
    }

    public static void GoToScene(int index)
    {
        SceneManager.LoadScene(index);
    }

    public static void RestartGame()
    {
        //skeletons.score = 0;
        //skeletons.nNormalCowsCaptured = 0;
        //skeletons.nFreezingCowsCaptured = 0;
        //skeletons.nHotCowsCaptured = 0;
        //skeletons.nNitroCowsCaptured = 0;
        //skeletons.nCloudCowsCaptured = 0;

        //aliens.score = 0;
        //aliens.nNormalCowsCaptured = 0;
        //aliens.nFreezingCowsCaptured = 0;
        //aliens.nHotCowsCaptured = 0;
        //aliens.nNitroCowsCaptured = 0;
        //aliens.nCloudCowsCaptured = 0;

        //gameTimer = gameTimeInSeconds;

        SceneManager.LoadScene(0);
    }

    public static void RestartScene()
    {
        Debug.Log("Loading scene " + SceneManager.GetActiveScene().buildIndex);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
    
    public static void ExitGame()
    {
        Debug.Log("Quitting application!");
        Application.Quit();
    }

    public static int GetPlayerScore(PlayerFaction player)
    {
        if (player == PlayerFaction.SKELETONS)
        {
            return skeletons.score;
        }
        else if (player == PlayerFaction.ALIENS)
        {
            return aliens.score;
        }
        else
        {
            Debug.Log("Error! GetPlayerScore(): Invalid faction!");
            return -1;
        }
    }

    public static void IncreaseScore(PlayerFaction player, int amount)
    {
        if (player == PlayerFaction.SKELETONS)
            skeletons.score += amount;
        else if (player == PlayerFaction.ALIENS)
            aliens.score += amount;
        else
            Debug.Log("Error! IncreaseScore(): Invalid faction!");
    }
}
