using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyOverTime : MonoBehaviour
{
    public float lifeTime; //How long the object lasts for

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //Countdown from lifeTime to 0
        lifeTime = lifeTime - Time.deltaTime;

        //When it gets to 0, delete the object from the world
        if(lifeTime <= 0f)
        {
            Destroy(gameObject);
        }
        //Debug.Log(Time.deltaTime)
    }
}
