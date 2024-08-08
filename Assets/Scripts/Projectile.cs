using Core;
using UnityEngine;
using UnityEngine.Audio;

public class Projectile : MonoBehaviour
{
    [Header("Audio")]
    [SerializeField] AudioClip idle_sfx;
    [SerializeField][Range(0f, 1f)] float idle_sfxVolume;
    [SerializeField] AudioClip explosion_sfx;
    [SerializeField][Range(0f, 1f)] float explosionVolume;
    [SerializeField] AudioMixerGroup audioMixerGroup;

    Rigidbody2D rigidBody;
    Renderer renderer;
    bool soundPlayed = false;

    // Start is called before the first frame update
    void Awake()
    {
        rigidBody = GetComponent<Rigidbody2D>();
        renderer = GetComponent<Renderer>();
    }

    void Update()
    {
        if (renderer.isVisible && !soundPlayed)
        {
            AudioManager.Instance.PlayAudioClip(idle_sfx, audioMixerGroup, idle_sfxVolume);
            soundPlayed = true;
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag(Tags.Arc))
        {
            AudioManager.Instance.PlayAudioClip(explosion_sfx, audioMixerGroup, explosionVolume);
            if(!collision.gameObject.GetComponent<Arc>().isPierce)
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
