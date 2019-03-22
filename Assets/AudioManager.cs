using UnityEngine;
using System.Collections;

public class AudioManager : MonoBehaviour {

    public float minPitch = 0.9f;
    public float maxPitch = 1.1f;
    public AudioSource BGMPlayer;
    private static AudioManager _instance;

    public static AudioManager Instance
    {
        get { return _instance; }
    }
    public AudioSource audioPlayer;

    void Awake()
    {
        _instance = this;
    }
    public void RandomPlay(float volume,params AudioClip[] audioClips)
    {
        int index = Random.Range(0, audioClips.Length);
        float pitch = Random.Range(minPitch, maxPitch);
        audioPlayer.clip = audioClips[index];
        audioPlayer.volume = volume;
        audioPlayer.pitch = pitch;
        audioPlayer.Play();
    }
    public void RandomPlay(params AudioClip[] audioClips)
    {
        int index = Random.Range(0, audioClips.Length);
        float pitch = Random.Range(minPitch, maxPitch);
        audioPlayer.clip = audioClips[index];
        audioPlayer.pitch = pitch;
        audioPlayer.Play();
    }

    public void EndBGM()
    {
        BGMPlayer.gameObject.SetActive(false);
    }
    public void BeginBGM()
    {
        BGMPlayer.gameObject.SetActive(true);
    }
    
}
