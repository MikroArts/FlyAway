using System;
using UnityEngine;
using UnityEngine.UI;

public class AudioManager : MonoBehaviour
{
    public AudioClip[] clips;
    public AudioSource soundSource;
    public GameObject player;
    
    void FixedUpdate()
    {        
        if (player)
        {
            PlayEngine();
        }
    }
    private void PlayEngine()
    {

        if (player.GetComponent<PlayerController>().moveSpeed == 0)
            return;
        if (Input.GetMouseButton(0))
        {
            if (!soundSource.isPlaying)
            {
                soundSource.PlayOneShot(clips[0]);
            }
        }
        else
        {
            soundSource.Stop();
        }
        if (player.GetComponent<PlayerController>().isDead)
        {
            soundSource.Stop();
        }
    }

    public void toggle_Sound_On_Off()
    {
        if (AudioListener.volume==0)
        {
            AudioListener.volume = .5f;
        }
        else
        {
            AudioListener.volume = 0;
        }
    }
}
