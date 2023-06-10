using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{

    public static SoundManager instance { get; private set; }
    [SerializeField] private float masterVolumeSFX = 0.4f;
    [SerializeField] private float masterVolumeBGM = 0.4f;
    [SerializeField] private AudioClip[] BGMs;
    [SerializeField] private AudioClip[] SFXs;

    Dictionary<string, AudioSource> BGMDic = new Dictionary<string, AudioSource>();
    Dictionary<string, AudioClip> SFXDic = new Dictionary<string, AudioClip>();

    private AudioSource bgmPlayer;
    private AudioSource sfxPlayer;


    private void Awake()
    {
        if (instance != null)
            return;
        instance = this;
        AfterAwake();
    }

    private void AfterAwake()
    {
        sfxPlayer = GetComponent<AudioSource>();

        foreach (var b in BGMs) {
            var temp = gameObject.AddComponent<AudioSource>();
            temp.clip = b;
            temp.playOnAwake = false;
            temp.volume = masterVolumeBGM;
            temp.loop = true;
            BGMDic.Add(b.name, temp);
        }

        bgmPlayer = BGMDic["Intro"];

        foreach (var s in SFXs) {
            SFXDic.Add(s.name, s);
        }
    }

    public void PlayBGM(string name)
    {
        if (!BGMDic.ContainsKey(name))
            return;
        StopBGM();
        bgmPlayer = BGMDic[name];
        bgmPlayer.volume = masterVolumeBGM;
        bgmPlayer.Play();
        
    }

    public void PlayBGM()
    {
        if (bgmPlayer != null) {
            bgmPlayer.volume = masterVolumeBGM;
            bgmPlayer.Play();  
        }
    }


    public void PauseBGM()
    {
        bgmPlayer.Pause();
    }

    public void StopBGM()
    {
        bgmPlayer.Stop();
    }


    public void PlaySFX(string name)
    {
        if (!SFXDic.ContainsKey(name))
            return;

        sfxPlayer.PlayOneShot(SFXDic[name], masterVolumeSFX);
    }

    public void SetVolumeBGM(float volume)
    {
        masterVolumeBGM = volume;
        if (bgmPlayer != null)
            bgmPlayer.volume = masterVolumeBGM;
    }

    public void SetVolumeSFX(float volume)
    {
        masterVolumeSFX = volume;
    }



}
