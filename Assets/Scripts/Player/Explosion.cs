using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Explosion : MonoBehaviour
{
    [SerializeField] private float delay = .1f;
    [SerializeField] private float power = 700f;
    [SerializeField] private float radius = 5f;
    [SerializeField] private ParticleSystem explosionEffect;

    private int soundIndex;

    float countdown;

    void Start()
    {
        countdown = delay;
        soundIndex = Random.Range(0, GameAssets.instance.explosionSoundClips.Count -1);
    }

    void Update()
    {
        countdown -= Time.deltaTime;
        if(countdown <= 0f)
        {
            Explode();
        }
    }

    void Explode()
    {
        Instantiate(explosionEffect, transform.position, transform.rotation);
        SoundManager.Play3DSoundFromList(soundIndex, transform.position);

        Collider[] colliders = Physics.OverlapSphere(transform.position, radius);

        foreach(Collider nearbyObject in colliders)
        {
           Rigidbody rb = nearbyObject.GetComponent<Rigidbody>();
            if(rb != null)
            {
                rb.AddExplosionForce(power, transform.position, radius);
            }

           // Store a list of gameobjects in a tighter radius, destroy and play particles
            if (nearbyObject.tag == "Destroyable")
            {
                Destroy(rb.gameObject);
            }
        }

        Destroy(gameObject);
    }   
}
