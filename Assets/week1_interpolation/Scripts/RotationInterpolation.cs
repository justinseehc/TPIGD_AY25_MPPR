using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotationInterpolation : MonoBehaviour
{
    public Vector3 startRotationEuler  = new Vector3(0, 0, 0);
    public Vector3 endRotationEuler = new Vector3(0, 180, 0);
    public float duration = 2f;

    private float elapsedTime = 0f;
    private Vector3 currentRotationEuler;

    private Renderer objectRenderer;
    public Color startColor = Color.red;
    public Color endColor = Color.blue;
    public Color currentColor;

    // Start is called before the first frame update
    void Start()
    {
        // Set the initial rotation
        transform.rotation = Quaternion.Euler(startRotationEuler);
        currentRotationEuler = startRotationEuler;

        // Set initial color
        objectRenderer = GetComponent<Renderer>();
        objectRenderer.material.color = startColor;
    }

    // Update is called once per frame
    void Update()
    {
        if (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            float t = elapsedTime / duration;
            t = 1 - (1 - t) * (1 - t);

            // Perform linear interpolation for each axis
            currentRotationEuler.x = (1 - t) * startRotationEuler.x + t * endRotationEuler.x;
            currentRotationEuler.y = (1 - t) * startRotationEuler.y + t * endRotationEuler.y;
            currentRotationEuler.z = (1 - t) * startRotationEuler.z + t * endRotationEuler.z;

            // Peform linear interpolation for each color
            currentColor.r = (1 - t) * startColor.r + t * endColor.r;
            currentColor.g = (1 - t) * startColor.g + t * endColor.g;
            currentColor.b = (1 - t) * startColor.b + t * endColor.b;

            // Apply the interpolated rotation
            transform.rotation = Quaternion.Euler(currentRotationEuler);

            // Apply the interpolated color
            objectRenderer.material.color = currentColor;
        } else
        {
            // Ensure the exact final rotation is applied
            transform.rotation = Quaternion.Euler(endRotationEuler);
            objectRenderer.material.color = endColor;
        }
    }
}
