using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;
using UnityEngine.UI;

public class ClearRenderTexture : MonoBehaviour
{
    [SerializeField] VideoPlayer videoplayer;
    void Awake()
    {
        videoplayer.targetTexture.Release();
    }
}
