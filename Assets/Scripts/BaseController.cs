using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseController : MonoBehaviour
{
    [SerializeField] GameManager.PlayerFaction faction;

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

            Debug.Log("Player " + faction + " score increased by " + cow.scoreValue);
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
                Debug.Log("Player " + cow.capturedByFaction + " score decreased by " + cow.scoreValue);
                cow.capturedByFaction = GameManager.PlayerFaction.NONE;

                // NOTE: negative scoreValue reduces score instead of increasing
                GameManager.IncreaseScore(faction, -cow.scoreValue);
            }
        }
    }

    public GameManager.PlayerFaction GetFaction()
    {
        return faction;
    }
}
