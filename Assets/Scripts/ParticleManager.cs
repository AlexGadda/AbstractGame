using UnityEngine;

public class ParticleManager : MonoBehaviour
{
    [SerializeField] GameObject[] particlesObj;
    [SerializeField] ParticleSystem[] particleSystems;
    [SerializeField] int emitQuantity;

    int currentIndex = 0;

    public void PlayAt(Vector2 position, Quaternion rotation)
    {
        particlesObj[currentIndex].transform.position = position;
        particlesObj[currentIndex].transform.rotation = rotation;

        particleSystems[currentIndex].Emit(emitQuantity);

        currentIndex++;
        if(currentIndex >= particlesObj.Length)
            currentIndex = 0;
    }
}
