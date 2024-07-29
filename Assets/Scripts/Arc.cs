using System;
using System.Buffers;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public enum RotationDirection { Clockwise, Counterclockwise}

public class Arc : MonoBehaviour
{
    Vector2 center;
    LineRenderer lineRenderer;
    EdgeCollider2D edgeCollider;
    List<Vector2> points = new List<Vector2>();
    RotationDirection rotationDirection;

    void Awake()
    {
        lineRenderer = GetComponent<LineRenderer>();
        edgeCollider = GetComponent<EdgeCollider2D>();

        lineRenderer.positionCount = 0;
        edgeCollider.points = new Vector2[2];
    }

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetCenter(Vector2 center)
    {
        this.center = center;
    }

    public void AddPoint(Vector2 point)
    {
        // Check if have to ignore point 
        if (points.Count >0 && point == points[^1]) // If it's the same point of the last one (mouse didn't move)
            return;
        if (points.Count > 2 && !IsCorrectDirection(point)) // Check if correct direction
        {
            Debug.Log("Point not taken"); // DEBUG
            return;
        }

        // Add point
        points.Add(point);
        lineRenderer.positionCount++;
        lineRenderer.SetPosition(points.Count-1, point);
        edgeCollider.points = points.ToArray();

        // If first 2 points, calculate direction (clock or counterclockwise)
        if (points.Count == 2)
        {
            rotationDirection = CalculateDirection(points[0], points[1]);
        }
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

    
    // Calcolute the bidimensional cross product between 2 vectors, given theri head and a common tail on the center of a circumference
    float CrossProduct(Vector2 p1, Vector2 p2)
    {
        return (p1.x - center.x) * (p2.y - center.y) - (p1.y - center.y) * (p2.x - center.x);
    }

    // Calcolate the traverse direction on an arc of circumference from the p1 to p2  (Clockwise or Counterclockwise)
    RotationDirection CalculateDirection(Vector2 p1, Vector2 p2)
    {
        float cross_product = CrossProduct(p1, p2);

        if (cross_product < 0)
        {
            return RotationDirection.Clockwise;
        }
        else if (cross_product > 0)
        {
            return RotationDirection.Counterclockwise;
        }
        else
        {
            throw new Exception("This Exception should never be raised: cross_product == 0.");
        }
    }

    bool IsCorrectDirection(Vector2 newPoint)
    {
        Debug.Log("Last point: " + points[^1]);
        Debug.Log("New point: " + newPoint);

        Debug.Log(CalculateDirection(points[^1], newPoint));
        if (CalculateDirection(points[^1], newPoint) == rotationDirection)
            return true;
        else
            return false;
    }


}
