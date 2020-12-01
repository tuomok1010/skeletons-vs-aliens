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
        EXIT
    }

    public enum PlayerFaction
    {
        NONE = 0,
        SKELETONS = 1,
        ALIENS = 2
    }

    public struct PlayerInfo
    {
        public int score;
    }

    public static GameState gameState;
    public static PlayerInfo skeletons;
    public static PlayerInfo aliens;

    private void Awake()
    {
        gameState = GameState.MENU;
        skeletons.score = 0;
        aliens.score = 0;
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
        SceneManager.LoadScene(0);
    }

    public static void RestartScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
    
    public static void ExitGame()
    {
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
