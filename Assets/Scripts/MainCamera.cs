using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainCamera : MonoBehaviour
{
    Animator animator;

    private void Awake()
    {
        animator = GetComponent<Animator>();
        if(!animator)
        {
            Debug.Log("Error! In MainCamera.cs: void Awake(): Could not find animator");
        }
    }

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        switch (GameManager.gameState)
        {
            case GameManager.GameState.MENU_TO_GAME_TRANSITION:
            {
                animator.SetTrigger("TriggerGameTransition");

            } break;

            case GameManager.GameState.GAME_TO_MENU_TRANSITION:
            {
                animator.ResetTrigger("TriggerGameTransition");
                animator.SetTrigger("TriggerMenuTransition");

            } break;
        }
    }
}
