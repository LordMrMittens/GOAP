using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    [SerializeField] AudioSource musicPlayer;
    [SerializeField] AudioClip[] musicClips;
    [SerializeField] GameObject nightAmbience;

    void Update()
    {
        if (nightAmbience.activeInHierarchy != WorldStatusManager.WSMInstance.isDark)
        {
            nightAmbience.SetActive(WorldStatusManager.WSMInstance.isDark);
        }

        if (musicPlayer.isPlaying == false)
        {
            int randomTrackIndex = Random.Range(0,musicClips.Length);
            if(musicPlayer.clip != musicClips[randomTrackIndex]){
                musicPlayer.clip = musicClips[randomTrackIndex];
                musicPlayer.Play();
            }
        }

    }
}
