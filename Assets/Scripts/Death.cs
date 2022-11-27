using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Death : MonoBehaviour
{
    public AudioClip deathClip;
    public AudioSource audioSource;
    public GameObject explosion;

    void Start()
    {
        audioSource.clip = deathClip;
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.tag == "Player")
        {
            foreach (Collider2D collider in gameObject.GetComponents<Collider2D>())
            {
                collider.enabled = false;
            }
            GameObject.FindGameObjectWithTag("AudioManager").GetComponent<AudioManager>().soundSource.Stop();
            audioSource.Play();
            Instantiate(explosion, col.transform.localPosition, col.transform.localRotation);
        }
    }
}
