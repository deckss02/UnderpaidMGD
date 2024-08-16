using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlashShift : MonoBehaviour
{
    public float shiftSpeed = 1f;  // The speed at which the object will move upwards
    public float maxShiftDistance = 2f;  // The maximum distance the object should shift upwards

    private Vector3 initialPosition;  // The initial position of the object
    private float currentShiftDistance = 0f;  // The current distance the object has shifted

    void Start()
    {
        // Store the initial position of the GameObject
        initialPosition = transform.position;
    }

    void Update()
    {
        // Calculate the distance to shift upwards based on the speed and time
        float shiftAmount = shiftSpeed * Time.deltaTime;

        // Check if the current shift distance is less than the max shift distance
        if (currentShiftDistance < maxShiftDistance)
        {
            // Move the object upwards
            transform.position += new Vector3(0, shiftAmount, 0);

            // Update the current shift distance
            currentShiftDistance += shiftAmount;
        }
    }
}
