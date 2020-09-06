using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Experimental.Rendering.Universal;

public class PostProcessingManager : MonoBehaviour
{
    public static PostProcessingManager instance;
    [SerializeField] private TimeManager timeManager;
    [SerializeField] private Volume volumeSlot;
    [SerializeField] private VolumeStates currentVolumeState;


    public VolumeProfile standardProfile;
    public VolumeProfile slowedProfile;
    public VolumeProfile rewindingProfile;

    void Awake()
    {
        instance = this;
    }

    public void setVolumeState(VolumeStates _desiredVolume)
    {
        currentVolumeState = _desiredVolume;

        switch (currentVolumeState)
        {
            case VolumeStates.STANDARD:
                volumeSlot.profile = standardProfile;
                timeManager.usingTimePoints = false;
                break;
            case VolumeStates.SLOWED:
                volumeSlot.profile = slowedProfile;
                timeManager.usingTimePoints = true;
                break;
            case VolumeStates.REWINDING:
                volumeSlot.profile = rewindingProfile;
                timeManager.usingTimePoints = true;
                break;
            default:
                volumeSlot.profile = standardProfile;
                break;
        }
    }   
}

public enum VolumeStates
{
    STANDARD,
    SLOWED,
    REWINDING
}
