using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour {
    private static AudioManager instance;
    public static AudioManager Instance {
        get { return instance; }
        set { instance = value; }
    }

    public float minPitch = 0.9f;
    public float maxPitch = 1.1f;
    public AudioSource src;
    public AudioSource bgmSrc;

    void Awake () {
        if (instance == null) {
            instance = this;
        } else if (instance != this) {
            Destroy (gameObject);
        }

        DontDestroyOnLoad (gameObject);
    }
    public void RandomPlay (params AudioClip[] clips) {
        float pitch = Random.Range (minPitch, maxPitch);
        int index = Random.Range (0, clips.Length);
        AudioClip clip = clips[index];
        src.pitch = pitch;
        src.clip = clip;
        src.Play ();
    }

    public void StopBGM () {
        bgmSrc.Stop ();
    }
}