using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{

    public static SoundManager instance { get; private set; }
    [SerializeField] private float masterVolumeSFX = 1f;
    [SerializeField] private float masterVolumeBGM = 1f;
    [SerializeField] private AudioClip[] BGMs;
    [SerializeField] private AudioClip[] SFXs;

    Dictionary<string, AudioSource> BGMDic = new Dictionary<string, AudioSource>();
    Dictionary<string, AudioClip> SFXDic = new Dictionary<string, AudioClip>();

    AudioSource bgmPlayer;
    AudioSource sfxPlayer;


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

    private void Start()
    {
        if (bgmPlayer != null)
            bgmPlayer.Play();   
    }

    public void PlayBGM(string name)
    {
        if (!BGMDic.ContainsKey(name))
            return;
        StopBGM();
        bgmPlayer = BGMDic[name];
        bgmPlayer.Play();
        
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



}
