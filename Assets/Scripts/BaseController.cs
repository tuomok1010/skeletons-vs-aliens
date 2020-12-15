using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseController : MonoBehaviour
{
    [SerializeField] GameManager.PlayerFaction faction;

    bool hasCountedFinalScore = false;

    // Start is called before the first frame update
    void Start()
    {
        if(faction == GameManager.PlayerFaction.NONE)
        {
            Debug.Log("Warning! Player base faction set to NONE");
        }
    }

    // Update is called once per frame
    void Update()
    {
        if(GameManager.gameState == GameManager.GameState.GAME_TO_MENU_TRANSITION && !hasCountedFinalScore)
        {
            if (faction == GameManager.PlayerFaction.SKELETONS)
                UpdateGameManagerFinalScores(ref GameManager.skeletons);
            else if (faction == GameManager.PlayerFaction.ALIENS)
                UpdateGameManagerFinalScores(ref GameManager.aliens);

            hasCountedFinalScore = true;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Cow")
        {
            NormalCow cow = other.gameObject.GetComponent<NormalCow>();
            if(!cow)
            {
                Debug.Log("Error! In BaseController.cs void OnTriggerEnter: NormalCow not found!");
            }

            if (cow.capturedByFaction == faction)
                return;

            //Debug.Log("Player " + faction + " score increased by " + cow.scoreValue);
            cow.capturedByFaction = faction;

            GameManager.IncreaseScore(faction, cow.scoreValue);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Cow")
        {
            NormalCow cow = other.gameObject.GetComponent<NormalCow>();
            if (!cow)
            {
                Debug.Log("Error! In BaseController.cs void OnTriggerExit: NormalCow not found!");
            }

            if(cow.capturedByFaction == faction)
            {
                //Debug.Log("Player " + cow.capturedByFaction + " score decreased by " + cow.scoreValue);
                cow.capturedByFaction = GameManager.PlayerFaction.NONE;

                // NOTE: negative scoreValue reduces score instead of increasing
                GameManager.IncreaseScore(faction, -cow.scoreValue);
            }
        }
    }

    void UpdateGameManagerFinalScores(ref GameManager.PlayerInfo playerScores)
    {
        FreezingCow.UnfreezeAllCowsBelongingToFaction(faction);

        int layerMask = 1 << LayerMask.NameToLayer("Cow"); // only the cows will trigger in the overlapbox

        Vector3 halfExtents = new Vector3(transform.lossyScale.x / 2.0f, transform.lossyScale.y / 2.0f,
            transform.lossyScale.z / 2.0f);

        Collider[] cowsInBase = Physics.OverlapBox(transform.position, halfExtents, Quaternion.identity, layerMask);

        for(int i = 0; i < cowsInBase.Length; ++i)
        {
            NormalCow cow = cowsInBase[i].GetComponent<NormalCow>();
            switch(cow.type)
            {
                case NormalCow.CowType.NORMAL:
                {
                    ++playerScores.nNormalCowsCaptured;

                } break;

                case NormalCow.CowType.FREEZING:
                {
                    ++playerScores.nFreezingCowsCaptured;

                } break;

                case NormalCow.CowType.HOT:
                {
                    ++playerScores.nHotCowsCaptured;

                } break;

                case NormalCow.CowType.NITRO:
                {
                    ++playerScores.nNitroCowsCaptured;

                } break;

                case NormalCow.CowType.CLOUD:
                {
                    ++playerScores.nCloudCowsCaptured;

                } break;

                default:
                {
                    Debug.Log("Error! In BaseController.cs: void UpdateGameManagerFinalScores(): Unknown cow type");

                } break;
            }
        }
    }

    public GameManager.PlayerFaction GetFaction()
    {
        return faction;
    }
}
