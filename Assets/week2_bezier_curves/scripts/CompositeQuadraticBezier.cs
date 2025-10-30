using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CompositeQuadraticBezier : MonoBehaviour
{
    public GameObject p0;
    public GameObject p1;
    public GameObject p2;
    public GameObject p3;
    public GameObject p4;

    public LineRenderer lineRenderer;

    // Method to calculate a point on the quadratic Bezier curve -- Modified
    private Vector3 CalculateQuadraticBezierPoint(float t, Vector3 p0, Vector3 p1, Vector3 p2)
    {
        float u = 1 - t; // u = (1 - t)
        float tt = t * t; // t squared
        float uu = u * u; // (1 - t) squared

        Vector3 point = uu * p0; // (1 - t)^2 * p0
        point += 2 * u * t * p1; // 2(1 - t)t * p1
        point += tt * p2; // t^2 * p2

        // return (u * u * p1) + (2 * u * t * p2) + (t * t * p3)
        return point;
    }

    // Method to calcuate and draw the quadratic Bezier curve
    //private void DrawBezierCurve()
    //{
    //    // Number of points on the curve for smoothness
    //    int curveResolution = 50;
    //    lineRenderer.positionCount = curveResolution;

    //    // Loop through each point on the curve
    //    for (int i = 0; i < curveResolution; i++)
    //    {
    //        // Parameter t varies from 0 to 1
    //        float t = i / (float)(curveResolution - 1);
    //        Vector3 curvePoint = CalculateQuadraticBezierPoint(t, p0.transform.position, p1.transform.position, p2.transform.position); // to get the individual point
    //        lineRenderer.SetPosition(i, curvePoint); // add the calculated point onto the line
    //    }
    //}

    // Similar to the function above!
    private void DrawCompositeQuadraticBezierCurve()
    {
        int segmentResolution = 50; // LineRenderer points per segment

        // Two segments, but reuse the shared point
        lineRenderer.positionCount = 2 * segmentResolution - 1;

        // Draw the first segment (p0 -> p2)
        for (int i = 0; i < segmentResolution; i++)
        {
            float t = i / (float)(segmentResolution - 1);
            Vector3 curvePoint = CalculateQuadraticBezierPoint(t, p0.transform.position, p1.transform.position, p2.transform.position);
            lineRenderer.SetPosition(i, curvePoint);
        }

        // Draw the second segment (p3, p4), reusing p2 as the start point
        for (int i = 0; i < segmentResolution; i++)
        {
            float t = i / (float)(segmentResolution - 1);
            Vector3 curvePoint = CalculateQuadraticBezierPoint(t, p2.transform.position, p3.transform.position, p4.transform.position);
            lineRenderer.SetPosition(i + segmentResolution - 1, curvePoint);
        }
    }

    // Implementing C1 Continuity
    private void EnsureC1Continuity()
    {
        // Calculate the vector from p2 to p1
        Vector3 direction1 = p2.transform.position - p1.transform.position;

        // Set p3 to be aligned along the same direction from p2
        p3.transform.position = p2.transform.position + direction1;
    }

    // Start is called before the first frame update
    void Start()
    {
        lineRenderer = GetComponent<LineRenderer>();
        EnsureC1Continuity();
        DrawCompositeQuadraticBezierCurve();
    }

    // Update is called once per frame
    void Update() 
    {
        DrawCompositeQuadraticBezierCurve();
    }
}