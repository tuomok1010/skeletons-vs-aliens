using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicController : MonoBehaviour
{
    [SerializeField] GameObject backgroundAmbient;
    [SerializeField] GameObject menuMusic;

    AudioSource backgroundAmbientAudioSource;
    AudioSource menuMusicAudioSource;

    float originalMenuMusicVolume;

    // Start is called before the first frame update
    void Start()
    {
        backgroundAmbientAudioSource = backgroundAmbient.GetComponent<AudioSource>();
        if (!backgroundAmbientAudioSource)
            Debug.Log("Error! In MusicController.cs: void Start(): Could not find backgroundAmbientAudioSource in " + gameObject.name);

        menuMusicAudioSource = menuMusic.GetComponent<AudioSource>();
        if(!menuMusicAudioSource)
            Debug.Log("Error! In MusicController.cs: void Start(): Could not find menuMusicAudioSource in " + gameObject.name);
        else
        {
            originalMenuMusicVolume = menuMusicAudioSource.volume;
        }
    }

    // Update is called once per frame
    void Update()
    {
        switch (GameManager.gameState)
        {
            case GameManager.GameState.MENU:
            {
                if (!backgroundAmbientAudioSource.isPlaying)
                    backgroundAmbientAudioSource.Play();

                if (!menuMusicAudioSource.isPlaying)
                    menuMusicAudioSource.Play();

            } break;

            case GameManager.GameState.GAME:
            {
                if (!backgroundAmbientAudioSource.isPlaying)
                    backgroundAmbientAudioSource.Play();

                menuMusicAudioSource.Stop();

            } break;

            case GameManager.GameState.PAUSE:
            {
                if (!backgroundAmbientAudioSource.isPlaying)
                    backgroundAmbientAudioSource.Play();

                menuMusicAudioSource.volume = originalMenuMusicVolume;

                if (!menuMusicAudioSource.isPlaying)
                    menuMusicAudioSource.Play();

            } break;

            case GameManager.GameState.END:
            {
                if (!backgroundAmbientAudioSource.isPlaying)
                    backgroundAmbientAudioSource.Play();

                menuMusicAudioSource.volume = originalMenuMusicVolume;

                if (!menuMusicAudioSource.isPlaying)
                    menuMusicAudioSource.Play();

            } break;

            default:
            {
                menuMusicAudioSource.Stop();

            } break;

        }
    }
}
