using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SwordSkill : MonoBehaviour
{
    public GameObject swordPrefab;

    // Start is called before the first frame update
    void Start()
    {
        GetComponent<Button>().interactable = false;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
