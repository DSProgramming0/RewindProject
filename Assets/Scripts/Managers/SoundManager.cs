using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class SoundManager
{
    public enum Sound
    {
        Jump, 
        Jump2,
        TargetHit,
        ReachedFinishLine,
        ReachedStartLine,
        Death,
        Continue,
        Explosion,
        backup
    }

    private static Dictionary<Sound, float> soundTimerDictionary;

    public static void Initialize()
    {
        soundTimerDictionary = new Dictionary<Sound, float>();
        soundTimerDictionary[Sound.Jump] = 0;
    }

    public static void Play3DSound(Sound sound , Vector3 position)
    {
        if (CanPlaySound(sound))
        {
            GameObject soundGameObject = new GameObject("Sound");
            soundGameObject.transform.position = position;
            AudioSource audioSource = soundGameObject.AddComponent<AudioSource>();
            audioSource.clip = GetAudioClip(sound, false);
            audioSource.volume = 0.9f;
            audioSource.maxDistance = 150f;
            audioSource.spatialBlend = 1f;
            audioSource.rolloffMode = AudioRolloffMode.Linear;
            audioSource.dopplerLevel = 0f;
            audioSource.Play();

            DestroyOnDelay.Destroy(soundGameObject, 2F);
        }
    }

    public static void Play3DSoundFromList(int index, Vector3 position)
    {
        GameObject soundGameObject = new GameObject("Sound");
        soundGameObject.transform.position = position;
        AudioSource audioSource = soundGameObject.AddComponent<AudioSource>();
        audioSource.volume = 0.9f;
        audioSource.maxDistance = 150f;
        audioSource.spatialBlend = 1f;
        audioSource.rolloffMode = AudioRolloffMode.Linear;
        audioSource.dopplerLevel = 0f;
        audioSource.PlayOneShot(GameAssets.instance.explosionSoundClips[index]);

        DestroyOnDelay.Destroy(soundGameObject, 2F);
    }

    public static IEnumerator playOnDelay(Sound sound, AudioSource audio, bool isBackgroundMusic)
    {
        yield return new WaitForSeconds(.5f);        
        audio.clip = GetAudioClip(sound, true);
        audio.Play();
        audio.priority = 256;
        audio.pitch = 0.95f;
    }

    private static bool CanPlaySound(Sound sound)
    {
        switch (sound)
        {
            default:
                return true;
            case Sound.backup:
                if (soundTimerDictionary.ContainsKey(sound))
                {
                    float lastTimePlayed = soundTimerDictionary[sound];
                    float playerMoveTimerMax = .05f;
                    if(lastTimePlayed + playerMoveTimerMax < Time.time)
                    {
                        soundTimerDictionary[sound] = Time.time;
                        return true;
                    }
                    else {
                        return false;
                    }
                }
                else {
                    return true;
                }
        }
    }

    private static AudioClip GetAudioClip(Sound sound, bool isBackgroundMusic)
    {
        foreach (GameAssets.soundAudioClip soundAudioClip in GameAssets.instance.soundAudioClipArray)
        { //run throough and check each soundAudioClip in the GameAssets audioClipArray
            if (soundAudioClip.sound == sound)
            {//if the sound in the array is equal to the sound we are trying to pass
                return soundAudioClip.audioClip;
            }
        }

        Debug.LogError("Clip " + sound + " was not found");
        return null;
    }  
}
