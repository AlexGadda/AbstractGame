using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileSpawner : MonoBehaviour
{
    [SerializeField] GameObject projectilePrefab;
    [SerializeField] float minRadius;
    [SerializeField] float maxRadius;
    [SerializeField] Transform projectileParent;
    [SerializeField] float speed;
    [SerializeField] ParticleManager particleManager;

    [Header("Difficulty curve")]
    public AnimationCurve spawnRateCurve;
    public AnimationCurve projectileSpeedCurve;


    Vector2 center, randomPoint, targetPoint;
    float randomDistance, angle;
    Projectile projectile;
    Coroutine spawnCoroutine;

    // Start is called before the first frame update
    void Start()
    {
        center = transform.position;
    }

    // Coroutine to spawn Projectiles
    IEnumerator ProjectileSpawn()
    {
        yield return new WaitForSeconds(4f); // Initial wait 

        while (true)
        {
            // Random point
            randomDistance = Random.Range(minRadius, maxRadius);
            randomPoint = Random.insideUnitCircle.normalized * randomDistance;

            // Rotation
            angle = Mathf.Atan2(randomPoint.y, randomPoint.x) * Mathf.Rad2Deg;

            projectile = GameObject.Instantiate(projectilePrefab, randomPoint, Quaternion.identity, projectileParent).GetComponent<Projectile>();
            projectile.Initialize(projectileSpeedCurve.Evaluate(GameManager.Instance.score), angle, particleManager);
            //Debug.Log("Speed: " + projectileSpeedCurve.Evaluate(GameManager.Instance.score) + " | SpawnRate: " + spawnRateCurve.Evaluate(GameManager.Instance.score));

            yield return new WaitForSeconds(spawnRateCurve.Evaluate(GameManager.Instance.score)); 
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
