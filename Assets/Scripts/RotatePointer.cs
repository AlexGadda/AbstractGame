using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class RotatePointer : MonoBehaviour
{
    Camera mainCamera;
    Vector3 mousePosition, previousMousePosition;
    Vector3 rotationPoint; 
    float rotationZ;
    Vector3 center; // Center of the circle
    float radius; // Radius of the circle 

    void Start()
    {
        mainCamera = Camera.main.GetComponent<Camera>();
        if (mainCamera == null)
        {
            Debug.LogError("Main Camera is not found!");
        }

        center = transform.position;
        radius = transform.localScale.x / 2;
    }

    void Update()
    {
        mousePosition = mainCamera.ScreenToWorldPoint(Input.mousePosition);

        // Check if mouse position is valid
        if(Vector2.Distance(mousePosition, transform.position) <= radius)
        {
            Mouse.current.WarpCursorPosition(mainCamera.WorldToScreenPoint(previousMousePosition));
            mousePosition = previousMousePosition;
        }

        // Calculating the angle betwenn the mouse pointer, the center of the circonference and the X axis
        rotationPoint = mousePosition-transform.position;
        rotationZ = Mathf.Atan2(rotationPoint.y, rotationPoint.x) * Mathf.Rad2Deg;

        // Rotate
        transform.rotation = Quaternion.Euler(0, 0, rotationZ);

        previousMousePosition = mousePosition;
    }
}
