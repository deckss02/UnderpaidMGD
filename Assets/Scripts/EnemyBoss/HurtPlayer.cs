using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HurtPlayer : MonoBehaviour
{
    private LevelManager theLevelManager; //Make reference to LevelManager

    public int damageToGive;

    // Start is called before the first frame update
    void Start()
    {
     theLevelManager = FindObjectOfType<LevelManager>();   
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    //Check if the player walks into a spike
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
           //theLevelManager.Respawn(); //Call the Respawn() function in the LevelManager script
           theLevelManager.HurtPlayer(damageToGive);
           print(theLevelManager.healthCount);
        }
    }
}
