using UnityEngine.Audio;
using System;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public Sound[] sounds;
    // Start is called before the first frame update
    void Awake()
    {
        foreach (Sound s in sounds){
            s.source = gameObject.AddComponent<AudioSource>();
            s.source.clip = s.clip;
            s.source.volume = s.volume;
            s.source.pitch = s.pitch;
        }
    }

    void Start () {
        Play("bgm");
    }
    // Update is called once per frame
    public void Play(string name)
    {
        Sound s = Array.Find(sounds, sound => sound.name == name);
        if (s == null){
            Debug.LogWarning("Sound: " + name + " not found!");
        }
        s.source.Play();
    }

    public void Stop(string name)
    {
        Sound s = Array.Find(sounds, sound => sound.name == name);
        if (s == null){
            Debug.LogWarning("Sound: " + name + " not found!");
        }
        s.source.Stop();
    }

    public void SetVolume(string name, float vol)
    {
        Sound s = Array.Find(sounds, sound => sound.name == name);
        if (s == null){
            Debug.LogWarning("Sound: " + name + " not found!");
        }
        s.source.volume = vol;
    }
}
