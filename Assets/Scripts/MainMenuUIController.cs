﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenuUIController : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void StartGame()
    {
        GameManager.gameState = GameManager.GameState.GAME;
        GameManager.GoToScene(2);
    }

    public void ExitGame()
    {
        GameManager.gameState = GameManager.GameState.EXIT;
        GameManager.ExitGame();
    }
}
