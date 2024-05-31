using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Checkpoint : MonoBehaviour
{
    //Store sprite images flag open and close
    public Sprite FlagClose;
    public Sprite FlagOpen;

    private SpriteRenderer theSpriteRenderer;

    public bool checkpointActive;
    // Start is called before the first frame update
    void Start()
    {
        //Get & Store a reference to the SpriteRenderer component so that we can access it 
        theSpriteRenderer = GetComponent<SpriteRenderer>();
    }   

    // Update is called once per frame
    void OnTriggerEnter2D(Collider2D other)
    {
        if(other.tag =="Player")
        {
            //Set the sprite in the Sprite Renderer to flagOpen sprite
            theSpriteRenderer.sprite = FlagOpen;
            checkpointActive = true;
        }

    }
}
