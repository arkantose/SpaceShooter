using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    //Sound effects
    [SerializeField]
    AudioClip laser, powerUp, explosion;
    
    [SerializeField]
    float sfxVolume;

   

    [SerializeField]
    AudioSource backgroundMusicSource;

    [SerializeField]
    AudioClip[] backgroundMusicClips;


    AudioSource audioSource;

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.K))
        {
            ChangeMusic();
        }
    }

    public void ChangeMusic()
    {
        backgroundMusicSource.clip = backgroundMusicClips[Random.Range(0, backgroundMusicClips.Length)];
        backgroundMusicSource.Play();    

    }
         
    public void PlayLaserSound()
    {
       audioSource.PlayOneShot(laser, sfxVolume);
    }

    public void PlayPowerUpSound()
    {
       audioSource.PlayOneShot(powerUp, sfxVolume);
    }

    public void PlayExplosionSound()
    {
        audioSource.PlayOneShot(explosion, sfxVolume);
    }


}
