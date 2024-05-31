using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Experience : MonoBehaviour
{
    private LevelManager theLevelManager; //Make a reference to the LevelManager

    public int ExpValue; //We can have more types of coins with different values

    // Start is called before the first frame update
    void Start()
    {
        theLevelManager = FindObjectOfType<LevelManager>();

    }

    // Update is called once per frame
    void Update()
    {

    }
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            Destroy(gameObject);
            theLevelManager.AddExp(ExpValue);//when player picks up the coins, the coinValue will be added to coinCount and the coin will disappear
        }
    }
}