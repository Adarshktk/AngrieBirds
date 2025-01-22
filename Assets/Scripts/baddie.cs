using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class baddie : MonoBehaviour
{

    [SerializeField] private float maxHealth = 8f;
    [SerializeField] private GameObject baddieDeathParticles;
    [SerializeField] private AudioClip baddieDieSound;

    //private AudioSource audioSource;

    private float currentHealth;
    private float thresholdVelocity = 0.2f;

    private void Awake()
    {
       // audioSource = GetComponent<AudioSource>();
        currentHealth = maxHealth;
    }

    public void DamageBaddie(float damageAmount)
    {
        currentHealth -= damageAmount;

        if(currentHealth < 0f)
        {
            Die();
        }
    }

    private void Die()
    {
        
        GameManager.Instance.RemoveBaddie(this);
        Instantiate(baddieDeathParticles, transform.position, Quaternion.identity);
       
        AudioSource.PlayClipAtPoint(baddieDieSound, transform.position);
        Destroy(gameObject);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        float impactVelocity = collision.relativeVelocity.magnitude;
        if(impactVelocity > thresholdVelocity)
        {
            DamageBaddie(impactVelocity);
        }
        
    }

}
