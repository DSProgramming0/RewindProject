using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Experimental.Rendering.Universal;

public class PostProcessingManager : MonoBehaviour
{
    public static PostProcessingManager instance;
    [SerializeField] private Volume volume;

    public VolumeProfile standardProfile;
    public VolumeProfile rewindingProfile;

    void Awake()
    {
        instance = this;
    }

    public void setProfile(VolumeProfile _profile)
    {
        volume.profile = _profile;
    }

    public void removeProfile()
    {
        volume.profile = standardProfile;
    }
}
