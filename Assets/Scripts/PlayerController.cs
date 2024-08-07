using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
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
    [SerializeField][Tooltip("After consuming all Ink, can't create new Arc before recharging until X.")] float rechargeTreshold;

    Arc arc;
    bool isHoldingMouse;
    bool hasShield = false;
    float currentInk;
    bool rechargeWait = false; // After reaching "0" (circa) ink, must wait until currentInk reaches recharge Treshold

    void Start()
    {
        currentInk = maxInk;
        AddInk(-50f); // DEBUG
    }

    void Update()
    {
        if(!isHoldingMouse)
        {
            RechargeInk();
        }
        // Add point to existing Arc
        else if (arc != null && currentInk > 0)
        {
            arc.AddPoint(arcStartingPoint.position);
        }
    }

    public void OnShoot(InputAction.CallbackContext context)
    {
        // Mouse 1 down
        if (context.started && !rechargeWait)
        {
            isHoldingMouse = true;

            // Create a new Arc
            arc = GameObject.Instantiate(arcPrefab, arcParent).GetComponent<Arc>();
            arc.Initialize(this.transform.position, Vector3.Distance(arcStartingPoint.position, this.transform.position), this);
        }
        // Mouse 1 up
        else if (context.canceled)
        {
            isHoldingMouse = false;

            // Making it move
            arc?.Shoot();

            // Reset
            arc = null;
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag(Tags.Projectile))
        {
            if (hasShield)
            {
                Destroy(collision.gameObject);
                hasShield = false;
                canvasManager.ShowShield(false);
            }
            else
            {
                GameManager.Instance.GameOver();
            }
        }
    }

    void RechargeInk()
    {
        if(currentInk <= 10f) 
            // Reached "0" -> must wait for reachrge time 
            rechargeWait = true;
   
        AddInk(rechargeRate*Time.deltaTime);

        if (currentInk >= rechargeTreshold)
            rechargeWait = false;
    }

    public void AddInk(float amout)
    {
        currentInk += amout;
        currentInk = Mathf.Clamp(currentInk, 0, maxInk);

        // Update UI 
        canvasManager.DisplayInk(currentInk, maxInk);
    }

    public void AddShield()
    {
        canvasManager.ShowShield(true);
        hasShield = true;
    }
}
