using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LinearInterpolation : MonoBehaviour
{
    public Transform pointA;
    public Transform pointB;
    public float duration = 2.0f;
    
    // for time tracking and positioning
    private float elapsedTime = 0.0f;
    private Vector3 positionA;
    private Vector3 positionB;

    // Start is called before the first frame update
    void Start()
    {
        // both codes are used to capture the position of both points
        positionA = pointA.position;
        positionB = pointB.position;
    }

    // Update is called once per frame
    void Update()
    {
        // implement time tracking and interpolation
        if (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            float t = elapsedTime / duration;
            t = Mathf.Clamp01(t);

            // explicit linear interpolation
            Vector3 interpolatedPosition = (1 - t) * positionA + t * positionB;

            // update the movingobject's position
            transform.position = interpolatedPosition;
        } else
        {
            // ensure the object reaches the exact position of pointB -- error handling??
            transform.position = positionB;
        }
    }

    private void OnDrawGizmos()
    {
        if (pointA != null && pointB != null)
        {
            // draw point A and point B (outline)
            Gizmos.color = Color.red;
            Gizmos.DrawSphere(pointA.position, 0.2f);

            Gizmos.color = Color.blue;
            Gizmos.DrawSphere(pointB.position, 0.2f); // 0.2f is the diameter of the sphere

            // draw a line between 2 points
            Gizmos.color = Color.gray;
            Gizmos.DrawLine(pointA.position, pointB.position);

            // draw interpolation steps -- to show how the object is moving
            Gizmos.color = Color.green;
            int steps = 20;
            for (int i = 0; i <= steps; i++)
            {
                float t = i / (float)steps;
                Vector3 interpolatedPosition = (1 - t) * positionA + t * positionB;
                Gizmos.DrawSphere(interpolatedPosition, 0.1f);
            }
        }
    }
}
