using Core;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Security.Cryptography;
using UnityEngine;
using UnityEngine.Audio;

enum PowerUpType { Shield, Ink, Pierce }

public class PowerUp : MonoBehaviour
{
    [SerializeField] PowerUpType type;
    [Header("Audio")]
    [SerializeField] AudioClip powerUpSfx;
    [SerializeField][Range(0f, 1f)] float powerUpSfxVolume;
    [SerializeField] AudioMixerGroup mixerGroup;

    Rigidbody2D rb;
    PowerupSpawner spawner;
    PlayerController player;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Start()
    {
        rb.velocity = new Vector2(Random.Range(-0.5f, 0.5f), Random.Range(-0.5f, 0.5f));     
    }

    private void Update()
    {
        if (DistanceFromCenter() > 10f)
            Destroy(this.gameObject);
    }

    public void Initialize(PowerupSpawner spawner, PlayerController player)
    {
        this.spawner = spawner;
        this.player = player;
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag(Tags.Arc) || collision.gameObject.CompareTag(Tags.Player))
        {
            switch (type)
            {
                case PowerUpType.Shield:
                    ActivateShield();
                    break;
                case PowerUpType.Ink:
                    ActivateInk();
                    break;
                case PowerUpType.Pierce:
                    ActivatePierce();   
                    break;
            }

            AudioManager.Instance.PlayAudioClip(powerUpSfx, mixerGroup, powerUpSfxVolume);

            if (collision.gameObject.CompareTag(Tags.Arc))
                Destroy(collision.gameObject);
            Destroy(this.gameObject);
        }
    }

    float DistanceFromCenter()
    {
        return this.transform.position.magnitude;
    }

    void ActivateInk()
    {
        player.AddInk(99999f);
    }

    void ActivateShield()
    {
        player.AddShield();
    }

    void ActivatePierce()
    {
        player.ActivatePierce();
    }

    void OnDestroy()
    {
        spawner.CanSpawnNow();
    }
}
