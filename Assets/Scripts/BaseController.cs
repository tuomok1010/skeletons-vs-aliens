using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseController : MonoBehaviour
{
    [SerializeField] GameObject owner;

    // Start is called before the first frame update
    void Start()
    {
        if(!owner)
        {
            Debug.Log("Error! " + gameObject.name + " owner not set!");
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
            cow.isCaptured = true;
            cow.baseOwner = owner;

            if (owner.tag == "Player1")
            {
                GameManager.player1.score += cow.scoreValue;
            }
            else if (owner.tag == "Player2")
            {
                GameManager.player2.score += cow.scoreValue;
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Cow")
        {
            NormalCow cow = other.gameObject.GetComponent<NormalCow>();
            cow.isCaptured = false;
            cow.baseOwner = null;
            if (owner.tag == "Player1")
            {
                GameManager.player1.score -= cow.scoreValue;
            }
            else if (owner.tag == "Player2")
            {
                GameManager.player2.score -= cow.scoreValue;
            }
        }
    }

    public GameObject GetOwner()
    {
        return owner;
    }
}
