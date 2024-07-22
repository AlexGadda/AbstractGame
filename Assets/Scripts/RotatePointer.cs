using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotatePointer : MonoBehaviour
{
    Camera mainCamera;
    Vector3 mousePosition;
    Vector3 rotationPoint; // Needed because the center of the circumference can be not 0,0 
    float rotationZ;

    // Start is called before the first frame update
    void Start()
    {
        mainCamera = Camera.main.GetComponent<Camera>();
        if (mainCamera == null)
        {
            Debug.LogError("Main Camera is not found!");
        }
    }

    // Update is called once per frame
    void Update()
    {
        // Calculating the angle betwenn the mouse pointer, the center of the circonference and the X axis
        mousePosition = mainCamera.ScreenToWorldPoint(Input.mousePosition);
        rotationPoint = mousePosition-transform.position;
        rotationZ = Mathf.Atan2(rotationPoint.y, rotationPoint.x) * Mathf.Rad2Deg;

        transform.rotation = Quaternion.Euler(0, 0, rotationZ);
    }
}
