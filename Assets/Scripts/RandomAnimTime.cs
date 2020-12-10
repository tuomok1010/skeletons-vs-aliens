using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomAnimTime : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
      Animator anim = GetComponent<Animator>();
      AnimatorStateInfo state0 = anim.GetCurrentAnimatorStateInfo(0);
      //could replace 0 by any other animation layer index
      //AnimatorStateInfo state1 = anim.GetCurrentAnimatorStateInfo(1);
      
      anim.Play(state0.fullPathHash, -1, Random.Range(0f, 1f));
      //anim.Play(state1.fullPathHash, -1, Random.Range(0f, 1f));
    }
}
