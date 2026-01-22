using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EasingMovement : MonoBehaviour
{
    // additional: easingtype
    /// <enum>
    /// WHAT IS ENUM?
    /// purpose: provides combination of choices, 
    /// enhancing code readability (instead of 0,1,2 - coders can choose how it is labelled) and 
    /// reducing errors (if the choices are out of range or misspelt, it will result in an error)
    /// </enum>

    public enum EasingType { Linear, EaseIn, EaseOut, EaseInOut }
    public EasingType easingType = EasingType.Linear; // default to linear

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

            // modified: easing function to t
            switch (easingType)
            {
                case EasingType.Linear:
                    break;

                case EasingType.EaseIn:
                    t = EaseInCubic(t);
                    break;

                case EasingType.EaseOut:
                    t = EaseOutCubic(t);
                    break;

                case EasingType.EaseInOut:
                    t = EaseInOutCubic(t);
                    break;
            }

            // modified: non linear interpolation
            Vector3 interpolatedPosition = (1 - t) * positionA + t * positionB;
            transform.position = interpolatedPosition;
        }
        else
        {
            // ensure the object reaches the exact position of pointB -- error handling??
            transform.position = positionB;
        }
    }

    void OnDrawGizmos()
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

            // modified: draw interpolation steps -- to show how the object is moving
            Gizmos.color = Color.green;
            int steps = 20;
            for (int i = 0; i <= steps; i++)
            {
                float t = i / (float)steps;

                // modified: easing function to t
                switch (easingType)
                {
                    case EasingType.Linear:
                        break;

                    case EasingType.EaseIn:
                        t = EaseInCubic(t);
                        break;

                    case EasingType.EaseOut:
                        t = EaseOutCubic(t);
                        break;

                    case EasingType.EaseInOut:
                        t = EaseInOutCubic(t);
                        break;
                }

                Vector3 interpolatedPosition = (1 - t) * positionA + t * positionB;
                Gizmos.DrawSphere(interpolatedPosition, 0.1f);
            }
        }
    }

    // additional: easing functions
    private float EaseInCubic(float x)
    {
        return x* x * x; // t = t^3
    }
    private float EaseOutCubic(float x)
    {
        return 1 - Mathf.Pow(1 - x, 3); // t = (1 - t)^3
    }
    private float EaseInOutCubic(float x)
    {
        return x < 0.5 ? 4 * x * x * x : 1 - Mathf.Pow(-2 * x + 2, 3) / 2; // t = t^3 * (t * 6t - 15) + 10)
    }
}