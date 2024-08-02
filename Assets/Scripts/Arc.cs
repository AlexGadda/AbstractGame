using System;
using System.Buffers;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.Burst.Intrinsics;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Experimental.Rendering;
using static UnityEngine.UI.Image;

public enum RotationDirection { Clockwise, Counterclockwise }

public class Arc : MonoBehaviour
{
    [SerializeField] float acceleration;
    [SerializeField] float scaleOverTime;
    [SerializeField][Range(10f, 179)] float maxAngle;
    [SerializeField] float deleteDistance;

    [HideInInspector] public Vector2 center;
    [HideInInspector] public float radius;

    PlayerController playerController;
    Vector2 movementVector;
    LineRenderer lineRenderer;
    EdgeCollider2D edgeCollider;
    List<Vector2> points = new List<Vector2>();
    RotationDirection rotationDirection;
    Rigidbody2D rigidBody;
    bool moving = false;
    bool stopDrawing = false;
    float totAngle, angleBetween;
    float distanceFromCenter;

    void Awake()
    {
        lineRenderer = GetComponent<LineRenderer>();
        edgeCollider = GetComponent<EdgeCollider2D>();
        rigidBody = GetComponent<Rigidbody2D>();

        lineRenderer.positionCount = 0;
        edgeCollider.points = new Vector2[2];
    }

    void FixedUpdate()
    {
        if (moving)
        {
            // Movement and Scaling
            rigidBody.AddForce(movementVector * acceleration);
            this.transform.localScale = this.transform.localScale * scaleOverTime;

            // Destruction if too far away 
            distanceFromCenter = Vector2.Distance(this.transform.position, center);
            if (distanceFromCenter > deleteDistance)
                Destroy(this.gameObject);
        }
    }

    // Constructor, MUST be called after instantiation 
    public void Initialize(Vector2 center, float radius, PlayerController playerController)
    {
        this.center = center;
        this.radius = radius;
        this.playerController = playerController;
    }

    public void AddPoint(Vector2 point)
    {
        // Check if have to ignore point 
        if (points.Count > 0)
        {
            if (stopDrawing)
                return;
            if (point == points[^1]) // If it's the same point of the last one (mouse didn't move)
                return;
            if ((totAngle += (angleBetween = Vector2.Angle(points[^1], point))) >= maxAngle)
            {
                stopDrawing = true;
                return;
            }
            if (points.Count > 2 && !IsCorrectDirection(point)) // Check if correct direction
                return;
        }

        // Add point
        points.Add(point);
        lineRenderer.positionCount++;
        lineRenderer.SetPosition(points.Count - 1, point);
        edgeCollider.points = points.ToArray();

        // Subtract Ink
        playerController.AddInk(-angleBetween);

        // If first 2 points, calculate direction (clock or counterclockwise)
        if (points.Count == 2)
        {
            rotationDirection = CalculateDirection(points[0], points[1]);
        }
    }

    // Makes the Arc move. 
    public void Shoot()
    {
        // Destroy the Arc if invalid 
        if (points.Count < 2)
        {
            Destroy(this.gameObject);
            return;
        }

        moving = true; // Makes the Arc move during FixedUpdate

        // Calculate the movementVector 
        Vector2 v1 = points[0] - center; // Vector from the center to the first point of the Arc
        Vector2 v2 = points[^1] - center;
        float angle = Vector2.Angle(v1, v2) / 2;
        Vector3 rotationAxis = rotationDirection == RotationDirection.Clockwise ? Vector3.back : Vector3.forward;
        movementVector = Quaternion.AngleAxis(angle, rotationAxis) * v1;
    }


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
        if (CalculateDirection(points[^1], newPoint) == rotationDirection)
            return true;
        else
            return false;
    }
}
