using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileSpawner : MonoBehaviour
{
    [SerializeField] GameObject projectilePrefab;
    [SerializeField] float minRadius;
    [SerializeField] float maxRadius;
    [SerializeField] Transform projectileParent;


    Vector2 center, randomPoint, targetPoint;
    float randomDistance, angle;
    GameObject projectile;
    Coroutine spawnCoroutine;

    // Start is called before the first frame update
    void Start()
    {
        center = transform.position;
    }

    // Coroutine to spawn Projectiles
    IEnumerator ProjectileSpawn()
    {
        yield return new WaitForSeconds(2f); // Initial wait    

        while (true)
        {
            // Random point
            randomDistance = Random.Range(minRadius, maxRadius);
            randomPoint = Random.insideUnitCircle.normalized * randomDistance;

            // Rotation
            angle = Mathf.Atan2(randomPoint.y, randomPoint.x) * Mathf.Rad2Deg;

            projectile = GameObject.Instantiate(projectilePrefab, randomPoint, Quaternion.identity, projectileParent);
            projectile.transform.Rotate(0, 0, angle);

            yield return new WaitForSeconds(1f);
        }
    }

    public void StartSpawning()
    {
        spawnCoroutine = StartCoroutine(ProjectileSpawn());
    }

    public void StopSpawning()
    {
        StopCoroutine(spawnCoroutine);
    }
}
