using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shake : MonoBehaviour
{
    // Start is called before the first frame update
    public Animator camAnim;

    public void camShake()
    {
        camAnim.SetTrigger("Shake");
    }

    
}
