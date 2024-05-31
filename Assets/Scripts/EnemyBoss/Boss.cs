using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss : MonoBehaviour
{
    public GameObject PawEye;
    public GameObject PawMouth;
    public GameObject HairballPrefab;
    public GameObject ClawPrefab;
    public GameObject minionPrefab;  //Reference to objects 
    public Transform[] firePoints; //Array of transform points where abilites will be summoned 
    public Transform[] summonPoints; // Array of transform points where minions will be summoned

    // Enumeration to represent different states/attacks of the boss
    private enum AttackType { HairBallRoll, Claw, SummonMinions, PawSlam }

    // Enumeration to represent different states/attacks of the boss
    private enum State { Idle, HairBallRoll, Claw, SummonMinions,PawSlam }
    private State currentState = State.Idle; // Variable to keep track of the current state, initialized to Idle
    // Dictionary to store cooldown times for each state
    private Dictionary<State, float> stateCooldowns = new Dictionary<State, float>
    {
        { State.HairBallRoll, 2f }, // Hairballroll attack has a cooldown of 2 seconds
        { State.Claw, 3f }, // Clawattack attack has a cooldown of 3 seconds
        { State.SummonMinions, 5f}, // Summon minions attack has a cooldown of 5 seconds
        { State.PawSlam, 3f } // Paw Slam has a cooldown of 3 seconds
    };

    private float nextStateTime = 0f; //Variable to track when the next state can be executed
    
    
    private Animator anim;

    private void Start()
    {
        StartCoroutine(StateMachine()); // Start the state machine coroutine when the game starts
        anim = GetComponent<Animator>();
    }

    private void Update()
    {
    }
    IEnumerator StateMachine()
    {
        while (true) // Infinite loop to continuously check and execute states
        {
            if (Time.time >= nextStateTime) // Check if the cooldown period has passed
            {
                switch (currentState) // Switch statement to handle different states
                {
                    case State.Idle: // If the boss is idle, get the next state
                        currentState = GetNextState();
                        break;
                    case State.HairBallRoll: // If the current state is HairBallRoll
                        HairBallRollAttack(); // Execute HairBallRoll attack
                        currentState = State.Idle; // Set the state back to Idle
                        nextStateTime = Time.time + stateCooldowns[State.HairBallRoll]; // Set the next state time based on  cooldown
                        break;
                    case State.SummonMinions: // If the current state is SummonMinions
                        SummonMinions(); // Execute Summon Minions attack
                        currentState = State.Idle; // Set the state back to Idle
                        nextStateTime = Time.time + stateCooldowns[State.SummonMinions]; // Set the next state time based on Summon Minions cooldown
                        break;
                    case State.Claw: // If the current state is Claw
                        ClawAttack(); // Execute Claw attack
                        currentState = State.Idle; // Set the state back to Idle
                        nextStateTime = Time.time + stateCooldowns[State.Claw]; // Set the next state time based on Claw cooldown
                        break;
                    case State.PawSlam: // If the current state is PawSlam
                        PawSlamAttack(); // Execute Claw attack
                        currentState = State.Idle; // Set the state back to Idle
                        nextStateTime = Time.time + stateCooldowns[State.PawSlam]; // Set the next state time based on PawSlam cooldown
                        break;
                }
            }
            yield return null; // Wait for the next frame before continuing the loop
        }
    }

    void HairBallRollAttack()
    {
        foreach (var firePoint in firePoints) // Loop through all fire points
        {
            Instantiate(HairballPrefab, firePoint.position, firePoint.rotation); // Instantiate a fireball at each fire point
        }
        Debug.Log("HairBallRoll attack executed"); // Log the attack execution
    }

    void PawSlamAttack()
    {
        foreach (var firePoint in firePoints) // Loop through all fire points
        {
            Instantiate(PawMouth, firePoint.position, firePoint.rotation); // Instantiate a fireball at each fire point
        }
        Debug.Log("PawSlam attack executed"); // Log the attack execution
    }


    void ClawAttack()
    {
        foreach (var firePoint in firePoints) // Loop through all fire points
        {
            Instantiate(ClawPrefab, firePoint.position, firePoint.rotation); // Instantiate a laser beam at each fire point
        }
        Debug.Log("Claw attack executed"); // Log the attack execution
    }

    void SummonMinions()
    {
        foreach (var summonPoint in summonPoints) // Loop through all summon points
        {
            Instantiate(minionPrefab, summonPoint.position, summonPoint.rotation); // Instantiate a minion at each summon point
        }
        Debug.Log("Minions summoned"); // Log the summoning
    }

    State GetNextState()
    {
        State[] states = { State.HairBallRoll, State.Claw, State.SummonMinions, State.PawSlam }; // Array of possible states/attacks
        return states[Random.Range(0, states.Length)]; // Randomly select and return one of the states
    }
}