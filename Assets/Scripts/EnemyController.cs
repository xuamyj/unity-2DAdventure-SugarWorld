using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{

    public float enemySpeed;
    public bool movesVertically;
    public bool startReverseDirection;

    Rigidbody2D rigidbody2d;
    // float horizontal;
    // float vertical;

    /* ---- ANIMATION ---- */
    Animator animator;

    /* ---- REVERSE DIRECTION ---- */
    public float reverseTime;
    float timeUntilReverse;
    float direction; // 1 or -1

    /* ---- PROJECTILE ---- */
    bool aggressive = true;

    /* ---- AUDIO ---- */
    AudioSource audioSource;
    public AudioClip enemyFixedClip;

    /* ---- SMOKE PARTICLE ---- */
    public ParticleSystem smokeEffect;


    // Start is called before the first frame update
    void Start()
    {
        // copied from PlayerController.cs
        rigidbody2d = GetComponent<Rigidbody2D>();

        /* ---- REVERSE DIRECTION ---- */
        timeUntilReverse = reverseTime;
        if (startReverseDirection)
        {
            direction = -1;
        }
        else
        {
            direction = 1;
        }

        /* ---- ANIMATION ---- */
        animator = GetComponent<Animator>();

        /* ---- AUDIO ---- */
        audioSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        // horizontal = enemySpeed;
        // vertical = 0.0f;

        /* ---- PROJECTILE ---- */
        if (!aggressive)
        {
            return;
        }

        /* ---- ANIMATION ---- */
        if (movesVertically)
        {
            animator.SetFloat("Move X", 0);
            animator.SetFloat("Move Y", direction);
        }
        else
        {
            animator.SetFloat("Move X", direction);
            animator.SetFloat("Move Y", 0);
        }

        /* ---- REVERSE DIRECTION ---- */
        if (timeUntilReverse < 0)
        {
            direction = -direction;
            timeUntilReverse = reverseTime;
        }
        timeUntilReverse -= Time.deltaTime;


    }

    void OnCollisionEnter2D(Collision2D other)
    {
        /* --- DAMAGE --- */
        UnityEngine.Debug.Log("Entered collision");

        PlayerController player = other.gameObject.GetComponent<PlayerController>();
        if (player != null)
        {
            UnityEngine.Debug.Log("Is player");
            player.ChangeHealth(-1);
        }
    }

    void FixedUpdate()
    {
        /* ---- PROJECTILE ---- */
        if (!aggressive)
        {
            return;
        }

        Vector2 position = rigidbody2d.position;
        if (movesVertically)
        {
            position.y = position.y + direction * enemySpeed * Time.deltaTime;
        }
        else
        {
            position.x = position.x + direction * enemySpeed * Time.deltaTime;
        }
        rigidbody2d.MovePosition(position);
    }

    /* ---- PROJECTILE ---- */
    public void Fix()
    {
        aggressive = false;
        rigidbody2d.simulated = false; // no longer collide with projectiles or damage player

        /* ---- ANIMATION ---- */
        // animator.SetFloat("Move X", 0);
        // animator.SetFloat("Move Y", -1);

        /* ---- AUDIO ---- */
        audioSource.Stop();
        audioSource.PlayOneShot(enemyFixedClip);

        /* ---- SMOKE PARTICLE ---- */
        smokeEffect.Stop();
    }
}
