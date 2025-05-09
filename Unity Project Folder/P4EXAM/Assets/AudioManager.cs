using System;
using Unity.VisualScripting;
using UnityEngine;
using Random = UnityEngine.Random;
using System.Collections;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;
    public Sound[] musicSounds, sfxSounds;
    public SoundArray[] sfxSoundArray;
    public AudioSource musicSource, sfxSource;

    private void Start()
    {
        StartCoroutine(playMusic());
    }

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    /// <summary>
    /// Plays music with the corresponding string name
    /// </summary>
    /// <param name="name"></param>
    public void PlayMusic(string name)
    {
        Sound sound = Array.Find(musicSounds, x => x.name == name);

        if (sound == null)
        {
            Debug.Log("Sound not found or is null");
        }
        else
        {
            musicSource.clip = sound.clip;
            musicSource.Play();
        }
    }

    /// <summary>
    /// Plays Sound effect with corresponding string name
    /// </summary>
    /// <param name="name"></param>
    public void PlaySFX(string name)
    {
        Sound sound = Array.Find(sfxSounds, x => x.name == name);

        if (sound == null)
        {
            Debug.Log("Sound not found or is null");
        }
        else
        {
            sfxSource.PlayOneShot(sound.clip);
        }
    }
    /// <summary>
    /// Plays a random sound from an array of sounds with corresponding array name
    /// </summary>
    /// <param name="name"></param>
    public void PlaySFXArrayRandom(string name)
    {
        SoundArray soundArray = Array.Find(sfxSoundArray, x => x.name == name);

        if (soundArray == null)
        {
            Debug.Log("Sound not found or is null");
        }
        else
        {
            int i;
            i = Random.Range(0, soundArray.clip.Length);

            sfxSource.PlayOneShot(soundArray.clip[i]);
        }
    }

    public void PlaySFXArrayAt(string name, int index)
    {
        SoundArray soundArray = Array.Find(sfxSoundArray, x => x.name == name);

        if (soundArray == null)
        {
            Debug.Log("Sound not found or is null");
        }
        else
        {
            sfxSource.PlayOneShot(soundArray.clip[index]);
        }
    }

    IEnumerator playMusic(){
        while (true)
        {
            int polyrytmePlaytime = 229;
            int harpeskifterPlaytime = 245;

            int whichSong = Random.Range(0, 2);
            if (whichSong == 0)
            {
                PlayMusic("Polyrytme");
                yield return new WaitForSeconds(polyrytmePlaytime);
            } 
            else if (whichSong == 1)
            {
                PlayMusic("Harpeskifter");
                yield return new WaitForSeconds(harpeskifterPlaytime);
            }
        }
    }
    

    /* 
     public void PlaySFXVariance(string name, float variance)
     {
         Sound sound = Array.Find(sfxSounds, x => x.name == name);

         if (sound == null)
         {
             Debug.Log("Sound not found or is null");
         }
         else
         {
             float _pitchVariance = Random.Range(-variance, variance);
             sfxSource.pitch = _pitchVariance;
              sfxSource.PlayOneShot(sound.clip);
         }
     }

     */



}