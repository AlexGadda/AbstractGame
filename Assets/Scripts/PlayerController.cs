using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [SerializeField] GameObject arcPrefab;
    [SerializeField] Transform arcParent;
    [SerializeField] float radius = 5f; // Radius of the arc
    [SerializeField] float startAngle = 0f; // Start angle in degrees
    [SerializeField] float endAngle = 90f; // End angle in degrees
    [SerializeField] int numberOfSegments = 50; // Number of segments to divide the arc into
    [SerializeField] Transform arcStartingPoint; // Generation point of the Arc

    Arc arc;
    LineRenderer lineRenderer;
    EdgeCollider2D edgeCollider;
    Vector2 mouseStartPosition, mousePosition, mouseEndPosition;
    List<Vector3> points = new List<Vector3>();
    bool isHoldingMouse;

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
    }

    private void FixedUpdate()
    {
        if (isHoldingMouse)
        {
            Debug.Log(arcStartingPoint.position);
            points.Add(Camera.main.ScreenToWorldPoint(Input.mousePosition));
        }
    }

    public void OnShoot(InputAction.CallbackContext context)
    {
        // Mouse 1 down
        if (context.started)
        {
            isHoldingMouse = true;

            // Create a new Arc
            arc = GameObject.Instantiate(arcPrefab, arcParent).GetComponent<Arc>();

            // Get the mouse position and set isHoldingMouse to true
            mouseStartPosition = arcStartingPoint.position;
            points.Add(mouseStartPosition);
        }
        // Mouse 1 up
        else if (context.canceled)
        {
            isHoldingMouse = false;
            mouseEndPosition = arcStartingPoint.position;
            points.Add(mouseEndPosition);

            // Painting the line
            arc.SetPoints(points.ToArray());
        }
    }
}
