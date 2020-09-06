using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[DefaultExecutionOrder(-100)]
public class GameAssets : MonoBehaviour
{
    private static GameAssets _instance;

    void Awake()
    {
        _instance = this;        
    }

    public static GameAssets instance
    {
        get {
            return _instance;
            }
    }   
   
    [Header("SoundClips")]
    public List<soundAudioClip> soundAudioClipArray;

    [Header("ExplosionVariationSounds")]
    public List<AudioClip> explosionSoundClips;

    [System.Serializable]
    public class soundAudioClip
    {
        public SoundManager.Sound sound;
        public AudioClip audioClip;
    }
}
