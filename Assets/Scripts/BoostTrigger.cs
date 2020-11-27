using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoostTrigger : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "Cow")
        {
            NormalCow cow = other.gameObject.GetComponent<NormalCow>();
            cow.clawOwner = transform.parent.gameObject;
            cow.isPickedUp = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Cow")
        {
            NormalCow cow = other.gameObject.GetComponent<NormalCow>();
            cow.clawOwner = null;
            cow.isPickedUp = false;
        }
    }
}
