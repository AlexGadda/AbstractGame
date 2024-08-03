using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fire : MonoBehaviour
{
    [SerializeField] LineRenderer lineRenderer;
    [SerializeField] EdgeCollider2D edgeCollider;
    [SerializeField] float radius = 5f; // Radius of the arc
    [SerializeField] float startAngle = 0f; // Start angle in degrees
    [SerializeField] float endAngle = 90f; // End angle in degrees
    [SerializeField] int numberOfSegments = 50; // Number of segments to divide the arc into
    
    // Start is called before the first frame update
    void Start()
    {
        DrawArc();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

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
}