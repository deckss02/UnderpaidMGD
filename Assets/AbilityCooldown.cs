using Microsoft.Unity.VisualStudio.Editor;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.UI;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;

public class AbilityCooldown : MonoBehaviour
{
    public UnityEngine.UI.Image abilityImage01;
    public float cooldownValue = 5f;
    bool isCooldown = false;
    public KeyCode ability1;



    // Start is called before the first frame update
    void Start()
    {
        abilityImage01.fillAmount = 0;
    }

    // Update is called once per frame
    void Update()
    {
        Ability1();
    }

    void Ability1()
    {
        if (Input.GetButtonDown(Ability1) && isCooldown == false; )
    }
}
