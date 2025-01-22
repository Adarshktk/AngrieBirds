using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
//using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class AngieBird : MonoBehaviour
{
    private Rigidbody2D rb;
    private CircleCollider2D circleCollider;
    private bool hasBeenLaunched;
    private bool isFacingTheDirection;


    [SerializeField] private AudioClip angieSound;
    private AudioSource audioSource;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        circleCollider = GetComponent<CircleCollider2D>();
        audioSource = GetComponent<AudioSource>();

    }
    private void Start()
    {
        rb.isKinematic = true;
        circleCollider.enabled = false;

    }

    private void FixedUpdate()
    {
        if (hasBeenLaunched && isFacingTheDirection)
        {
            transform.right = rb.velocity;
        }
        
    }

    public void LaunchBird(Vector2 direction, float force)
    {
        
        rb.isKinematic = false;
        circleCollider.enabled = true;
        rb.velocity = Vector2.zero;


        //apply the force
        rb.AddForce(direction * force, ForceMode2D.Impulse);

        hasBeenLaunched = true;
        isFacingTheDirection = true;

    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        isFacingTheDirection = false;
        GameManager.Instance.CheckForLastShot();
        SoundManager.Instance.PlayClip(angieSound, audioSource);
        Destroy(this);
    }

}
