using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class SpiralPath : MonoBehaviour
{
    // Speed at which the object moves
    public float speed = 1f;

    // Duration for which the object follows the path
    public float duration = 10f;
    private float t = 0f; // Time parameter
    private float timeElapsed = 0f; // Tracks the elapsed time
    
    void Update()
    {
        // If the object has moved for less than the specified
        // duration, continue updating
        if (timeElapsed < duration)
        {
            // Increment time based on speed and deltaTime
            t = t + (speed * Time.deltaTime);
            
            float x = t;
            float y = t * Mathf.Cos(t);
            float z = t * Mathf.Sin(t);
            
            // Set the new position of the object
            transform.position = new Vector3(x, y, z);
    
            // Update elapsed time
            timeElapsed += Time.deltaTime;
        }
    }
}
