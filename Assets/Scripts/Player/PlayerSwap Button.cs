using System.Collections;
using System.Collections.Generic;
using UnityEditor.Build;
using UnityEngine;

public class PlayerSwapButton : MonoBehaviour
{
    public PlayerController controller;
    public GameObject Player;

    public SpriteRenderer SR;
    // Start is called before the first frame update
    void Start()
    {
        controller = GetComponent<PlayerController>();
        Player = GetComponent<GameObject>();
        SR = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
