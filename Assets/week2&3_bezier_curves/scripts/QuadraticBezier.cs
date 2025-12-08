using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuadraticBezier : MonoBehaviour
{
    public GameObject p0;
    public GameObject p1;
    public GameObject p2;

    public LineRenderer lineRenderer;

    // Method to calculate a point on the quadratic Bezier curve
    private Vector3 CalculateBezierPoint(float t, Vector3 p0, Vector3 p1, Vector3 p2)
    {
        float u = 1 - t; // u = (1 - t)
        float tt = t * t; // t squared
        float uu = u * u; // (1 - t) squared

        Vector3 point = uu * p0; // (1 - t)^2 * p0
        point += 2 * u * t * p1; // 2(1 - t)t * p1
        point += tt * p2; // t^2 * p2

        return point;
    }

    // Method to calcuate and draw the quadratic Bezier curve
    private void DrawBezierCurve()
    {
        // Number of points on the curve for smoothness
        int curveResolution = 50;
        lineRenderer.positionCount = curveResolution;

        // Loop through each point on the curve
        for (int i = 0; i < curveResolution; i++)
        {
            // Parameter t varies from 0 to 1
            float t = i / (float)(curveResolution - 1);
            Vector3 curvePoint = CalculateBezierPoint(t, p0.transform.position, p1.transform.position, p2.transform.position); // to get the individual point

            lineRenderer.SetPosition(i, curvePoint); // add the calculated point onto the line
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        lineRenderer = GetComponent<LineRenderer>();
        DrawBezierCurve();
    }

    // Update is called once per frame
    void Update() 
    {
        DrawBezierCurve();
    }
}