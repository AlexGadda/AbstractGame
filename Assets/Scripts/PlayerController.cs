using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [SerializeField] GameObject arcPrefab;
    [SerializeField] Transform arcParent;
    [SerializeField] Transform arcStartingPoint; // Generation point of the Arc

    Arc arc;
    Vector2 startPosition, endPosition;
    List<Vector3> points = new List<Vector3>();
    bool isHoldingMouse;

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        if (isHoldingMouse && arc != null)
        {
            arc.AddPoint(arcStartingPoint.position);
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
            startPosition = arcStartingPoint.position;
            points.Add(startPosition);
        }
        // Mouse 1 up
        else if (context.canceled)
        {
            isHoldingMouse = false;
            endPosition = arcStartingPoint.position;
            points.Add(endPosition);

            // Painting the line
            //arc.SetPoints(points.ToArray());

            // Reset
            points.Clear();
            arc = null;
        }
    }
}
