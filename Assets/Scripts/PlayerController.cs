using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using LivelyChatBubbles;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    /* ---- MOVEMENT ---- */
    public InputAction LeftAction;
    public InputAction RightAction;
    public InputAction DownAction;
    public InputAction UpAction;

    public float speed;

    Rigidbody2D rigidbody2d;
    float horizontal;
    float vertical;

    /* ---- HEALTH ---- */
    public int maxHealth = 5;
    int currentHealth;
    public int health { get { return currentHealth; } }

    /* ---- DAMAGE ZONE INVINCIBILITY ---- */
    public float timeInvincible = 2.0f;
    bool isInvincible;
    float damageCooldown;

    /* ---- ANIMATION ---- */
    Animator animator;
    Vector2 moveDirection = new Vector2(0, -0.01f);

    /* ---- PROJECTILE ---- */
    public GameObject projectilePrefab;
    public InputAction launchAction;

    /* ---- TALK ---- */
    public InputAction talkAction;

    /* ---- AUDIO ---- */
    AudioSource audioSource;
    public AudioClip playerHitClip;
    public AudioClip throwCogClip;
    public AudioClip questCompleteClip;

    // Start is called before the first frame update
    void Start()
    {
        /* ---- MOVEMENT ---- */

        // QualitySettings.vSyncCount = 0;
        // Application.targetFrameRate = 10;

        LeftAction.Enable();
        RightAction.Enable();
        DownAction.Enable();
        UpAction.Enable();

        rigidbody2d = GetComponent<Rigidbody2D>();

        /* ---- HEALTH ---- */
        currentHealth = maxHealth;

        /* ---- ANIMATION ---- */
        animator = GetComponent<Animator>();

        /* ---- PROJECTILE ---- */
        launchAction.Enable();
        launchAction.performed += Launch;

        /* ---- TALK ---- */
        talkAction.Enable();
        talkAction.performed += FindFriend;

        /* ---- AUDIO ---- */
        audioSource = GetComponent<AudioSource>();
    }

    public void PlaySound(AudioClip clip)
    {
        audioSource.PlayOneShot(clip);
    }

    public void ChangeHealth(int amount)
    {
        if (amount < 0)
        {
            if (isInvincible)
            {
                return;
            }
            isInvincible = true;
            damageCooldown = timeInvincible;

            /* ---- ANIMATION ---- */
            animator.SetTrigger("Hit");
            /* ---- AUDIO ---- */
            PlaySound(playerHitClip);
        }

        currentHealth = Mathf.Clamp(currentHealth + amount, 0, maxHealth);
        UnityEngine.Debug.Log("health: " + currentHealth + "/" + maxHealth);

        UIHandler.instance.SetHealthValue(currentHealth / (float)maxHealth);
    }

    // Update is called once per frame
    void Update()
    {
        /* ---- MOVEMENT ---- */

        // UnityEngine.Debug.Log("Hi");
        // UnityEngine.Debug.Log(Keyboard.current);
        // UnityEngine.Debug
        // Changed to "Both"
        // added Input System Package, but maybe doesn't do anything
        // added EventSystem and clicked twice, but maybe doesn't do anything

        horizontal = 0.0f;
        if (LeftAction.IsPressed())
        {
            horizontal = -speed;
        }
        else if (RightAction.IsPressed())
        {
            horizontal = speed;
        }
        // UnityEngine.Debug.Log("keyboard horizontal: " + horizontal);

        vertical = -0.0f;
        if (DownAction.IsPressed())
        {
            vertical = -speed;
        }
        else if (UpAction.IsPressed())
        {
            vertical = speed;
        }
        // UnityEngine.Debug.Log("keyboard vertical: " + vertical);

        /* ---- DAMAGE ZONE INVINCIBILITY ---- */
        if (isInvincible)
        {
            damageCooldown -= Time.deltaTime;
            if (damageCooldown < 0)
            {
                isInvincible = false;
            }
        }

        /* ---- ANIMATION ---- */
        if (!Mathf.Approximately(horizontal, 0.0f) || !Mathf.Approximately(vertical, 0.0f))
        {
            moveDirection.Set(horizontal, vertical);
            // moveDirection.Normalize();
        }
        float magnitude = new Vector2(horizontal, vertical).magnitude;

        // UnityEngine.Debug.Log("moveDirection " + moveDirection);
        // UnityEngine.Debug.Log("magnitude " + magnitude);

        animator.SetFloat("Look X", moveDirection.x);
        animator.SetFloat("Look Y", moveDirection.y);
        animator.SetFloat("Speed", magnitude);
    }

    void FixedUpdate()
    {
        Vector2 position = rigidbody2d.position; // transform.position;
        position.x = position.x + horizontal * Time.deltaTime;
        position.y = position.y + vertical * Time.deltaTime;
        // transform.position = position;
        rigidbody2d.MovePosition(position);
    }

    /* ---- PROJECTILE ---- */
    void Launch(InputAction.CallbackContext context)
    {
        GameObject projectileObject = Instantiate(projectilePrefab, rigidbody2d.position + Vector2.up * 0.5f, Quaternion.identity);

        Projectile projectile = projectileObject.GetComponent<Projectile>();
        // UnityEngine.Debug.Log(moveDirection);
        projectile.Launch(moveDirection.normalized, 800);

        /* ---- ANIMATION ---- */
        animator.SetTrigger("Launch");
        /* ---- AUDIO ---- */
        PlaySound(throwCogClip);
    }

    /* ---- TALK ---- */
    void FindFriend(InputAction.CallbackContext context)
    {
        // UnityEngine.Debug.Log("Raycast attempted");

        RaycastHit2D hit = Physics2D.Raycast(rigidbody2d.position + Vector2.up * 0.4f, moveDirection, 1.5f, LayerMask.GetMask("Friend"));
        if (hit.collider != null)
        {
            UnityEngine.Debug.Log("Raycast has hit the object " + hit.collider.gameObject);

            FriendController friend = hit.collider.GetComponent<FriendController>();
            if (friend != null)
            {
                friend.GiveInstructions();
            }
        }
        else
        {
            UnityEngine.Debug.Log("Nope");
        }
    }
}
