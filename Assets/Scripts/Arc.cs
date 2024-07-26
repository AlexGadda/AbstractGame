using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Arc : MonoBehaviour
{
    LineRenderer lineRenderer;
    EdgeCollider2D edgeCollider;

    // Start is called before the first frame update
    void Start()
    {
        lineRenderer = GetComponent<LineRenderer>();
        edgeCollider = GetComponent<EdgeCollider2D>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    public void SetPoints(Vector3[] points)
    {
        lineRenderer.SetPositions(points);
        Debug.Log("hello?");
    }

    /*
    void DrawArc()
    {
        Vector2[] arcPoints = new Vector2[numberOfSegments + 1];

        float angleStep = (endAngle - startAngle) / numberOfSegments;
        lineRenderer.positionCount = numberOfSegments + 1;

        for (int i = 0; i <= numberOfSegments; i++)
        {
            // Calculation of the coordinates of the point 
            float angle = startAngle + i * angleStep;
            float angleRad = Mathf.Deg2Rad * angle;

            Vector3 point = new Vector3(
                Mathf.Cos(angleRad) * radius,
                Mathf.Sin(angleRad) * radius,
                0f
            );

            // Add the point to the LineRenderer
            lineRenderer.SetPosition(i, point);

            // Add the point to the arcPoints array
            arcPoints[i] = point;
        }

        // Set the collider
        edgeCollider.points = arcPoints;
    }
    */
}
