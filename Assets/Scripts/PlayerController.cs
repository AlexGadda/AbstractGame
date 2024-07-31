using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [SerializeField] GameObject arcPrefab;
    [SerializeField] Transform arcParent;
    [SerializeField] Transform arcStartingPoint; // Generation point of the Arc, the "Pointer"

    Arc arc;
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
            arc.center = this.transform.position;
            arc.radius = Vector3.Distance(arcStartingPoint.position, this.transform.position);

            // Get the mouse position
            points.Add(arcStartingPoint.position);
        }
        // Mouse 1 up
        else if (context.canceled)
        {
            isHoldingMouse = false;

            // Add the final point to the Arc and making it move
            points.Add(arcStartingPoint.position);
            arc.Shoot();

            // Reset
            points.Clear();
            arc = null;
        }
    }
}
