using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour
{
    public bool keyPickedUp;
    public bool locked;
    [SerializeField] GameObject KEY;
    void Start()
    {
        locked = true;
    }
    // Update is called once per frame
    void Update()
    {
        
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if(other.tag == "Key" && !keyPickedUp)
        {
           locked = false;
        }
    }
}
