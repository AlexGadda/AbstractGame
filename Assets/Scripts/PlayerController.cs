using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [SerializeField] CanvasManager canvasManager;
    [Header("Prefabs")]
    [SerializeField] GameObject arcPrefab;
    [SerializeField] Transform arcParent;
    [SerializeField] Transform arcStartingPoint; // Generation point of the Arc, the "Pointer
    [Header("Ink")]
    [SerializeField] float maxInk; // Max and starting Ink
    [SerializeField] float rechargeRate; // TODO

    Arc arc;
    List<Vector3> points = new List<Vector3>();
    bool isHoldingMouse;
    float currentInk;

    // Start is called before the first frame update
    void Start()
    {
        currentInk = maxInk;
        AddInk(-50f); // DEBUG
    }

    // Update is called once per frame
    void Update()
    {
        if(!isHoldingMouse)
        {
            RechargeInk();
        }
        // Add point to existing Arc
        else if (arc != null)
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
            arc.Initialize(this.transform.position, Vector3.Distance(arcStartingPoint.position, this.transform.position), this);

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

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag(Tags.Projectile))
        {
            GameManager.Instance.GameOver();
        }
    }

    void RechargeInk()
    {
        AddInk(rechargeRate*Time.deltaTime);
    }

    public void AddInk(float amout)
    {
        currentInk += amout;
        currentInk = Mathf.Clamp(currentInk, 0, maxInk);

        // Update UI 
        canvasManager.DisplayInk(currentInk, maxInk);
    }
}
