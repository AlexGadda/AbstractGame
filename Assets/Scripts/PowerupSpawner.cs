using System.Collections;
using UnityEngine;

public class PowerupSpawner : MonoBehaviour
{
    [SerializeField] PlayerController player;
    [SerializeField] GameObject[] powerUps;
    [SerializeField] Transform powerupParent;
    [SerializeField] float spawnAfter;
    [SerializeField] float minRadius;
    [SerializeField] float maxRadius;
    [SerializeField] float minTime;
    [SerializeField] float maxTime;

    int[] weights;
    float randomDistance, angle, timeToSpawn;
    Vector2 randomPoint;
    GameObject powerUp;
    PowerUp powerUp_script;
    bool canSpawn = true; // False if spawned but not picked up or destroyed 

    // Start is called before the first frame update
    void Start()
    {
        // Setup weights
        weights = new int[powerUps.Length];
        for(int i=0; i< powerUps.Length; i++)
        {
            weights[i] = 1;
        }

        StartCoroutine(PowerUpSpawn(spawnAfter));
    }

    // Coroutine to spawn Projectiles
    IEnumerator PowerUpSpawn(float spawnAfter)
    {
        yield return new WaitForSeconds(spawnAfter); // Initial wait 

        while (true)
        {
            // Random point
            randomDistance = Random.Range(minRadius, maxRadius);
            randomPoint = Random.insideUnitCircle.normalized * randomDistance;

            // Wait random time to spawn
            timeToSpawn = Random.Range(minTime, maxTime);
            yield return new WaitForSeconds(timeToSpawn);
            while(!canSpawn) // Wait until can spawn 
            {
                timeToSpawn = Random.Range(minTime/2, maxTime/2);
                yield return new WaitForSeconds(timeToSpawn);
            }

            // Randomly select what to spawn
            powerUp = GetWeightedRandomElement(powerUps, weights);
            powerUp_script = Instantiate(powerUp, randomPoint, Quaternion.identity, powerupParent).GetComponent<PowerUp>();
            powerUp_script.Initialize(this, player);

            canSpawn = false;
        }
    }

    GameObject GetWeightedRandomElement(GameObject[] array, int[] weights)
    {
        if (array.Length != weights.Length)
        {
            Debug.LogError("Array and weights must have the same length!");
            return null;
        }

        // Calculate the total weight
        int totalWeight = 0;
        foreach (int weight in weights)
        {
            totalWeight += weight;
        }

        // Generate a random number between 0 and the total weight
        int randomWeight = Random.Range(0, totalWeight);

        // Determine which element is selected based on the random weight
        int cumulativeWeight = 0;
        for (int i = 0; i < array.Length; i++)
        {
            cumulativeWeight += weights[i];
            if (randomWeight < cumulativeWeight)
            {
                IncreaseOtherWeights(i);
                return array[i];
            }
        }

        // Fallback (should never happen)
        return null;
    }

    void IncreaseOtherWeights(int index)
    {
        for(int i=0; i<weights.Length; i++)
        {
            if(i!=index)
                weights[i] += 1;
        }
    }

    public void CanSpawnNow()
    {
        canSpawn = true;
    }
}
