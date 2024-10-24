using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WinCollectible : MonoBehaviour
{
    /* ---- AUDIO ---- */
    public AudioClip questCompleteClip;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    void OnTriggerEnter2D(Collider2D other)
    {
        PlayerController controller = other.GetComponent<PlayerController>();
        if (controller != null)
        {
            controller.ChangeHealth(controller.maxHealth);

            /* ---- AUDIO ---- */
            controller.PlaySound(questCompleteClip);

            Destroy(gameObject);
        }
    }
}
