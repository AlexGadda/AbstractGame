using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    Rigidbody2D rigidBody;

    // Start is called before the first frame update
    void Awake()
    {
        rigidBody = GetComponent<Rigidbody2D>();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag(Tags.Arc))
        {
            // Destroy Arc and Projectile
            Destroy(collision.gameObject);
            Destroy(this.gameObject);
        }
    }

    public void Initialize(float speed, float rotationAngle)
    {
        this.transform.Rotate(0, 0, rotationAngle);
        rigidBody.velocity = -this.transform.right * speed; // Set speed
    }
}
