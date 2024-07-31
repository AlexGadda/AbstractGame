using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileSpawner : MonoBehaviour
{
    [SerializeField] float minRadius;
    [SerializeField] float maxRadius;

    [SerializeField] GameObject circle; // DEBUG
        
    Vector2 center, randomPoint;
    float randomDistance;

    // Start is called before the first frame update
    void Start()
    {
        center = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        randomDistance = Random.Range(minRadius, maxRadius);
        randomPoint = Random.insideUnitCircle.normalized * randomDistance;

        GameObject.Instantiate(circle, randomPoint, Quaternion.identity); // DEBUG
    }
}
