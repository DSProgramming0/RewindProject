using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TimeManager : MonoBehaviour
{
    [SerializeField] private AudioSource audio;
    [SerializeField] private TimeBody playerTimeBody;

    private float maxTimePoints = 100f;
    private float timePoints;
    public bool canUseTimeSkills = true;
    public bool usingTimePoints = false;

    private float pitchMin;
    private float pitchRegainControl;

    private float pitchRegain = 1f;
    private float slowDownFactor = 0.05f;
    private float slowDownLength = 5f;
    private float slowedPitch = .4f;

    [SerializeField] private bool isSlowed = false;

    void Start()
    {
        DoSlowMotion();

        TimePoints = maxTimePoints;
    }

    public float TimePoints
    {
        get { return timePoints; }
        set { timePoints = value; }
    }

    public void decreaseTimePoints()
    {
        TimePoints -= 15f * Time.unscaledDeltaTime;        
    }

    private void refillTimePoints()
    {
        TimePoints += 30f * Time.unscaledDeltaTime;
        //Debug.Log("Calling refill");
    }

    void Update()
    {
        //Debug.Log(TimePoints);
        
        if(Time.timeScale == 1 && !playerTimeBody.isRewinding)
        {
            PostProcessingManager.instance.setVolumeState(VolumeStates.STANDARD);
            isSlowed = false;
        }

        if(TimePoints >= 30)        
            canUseTimeSkills = true;        
        else
            canUseTimeSkills = false;

        if (usingTimePoints)
        {
            decreaseTimePoints();
        }
        else if (!usingTimePoints)
        {
            if(TimePoints < 100)
                refillTimePoints();
        }        

        Time.timeScale += (1f / slowDownLength) * Time.unscaledDeltaTime;
        Time.timeScale = Mathf.Clamp(Time.timeScale, 0F , 1F);

        audio.pitch += (1f / pitchRegainControl) * Time.deltaTime;
        audio.pitch = Mathf.Clamp(audio.pitch, pitchMin, 1F);
    }

    public void DoSlowMotion()
    {
        //Debug.Log("Slowed");
        PostProcessingManager.instance.setVolumeState(VolumeStates.SLOWED);
        Time.timeScale = slowDownFactor;

        Time.fixedDeltaTime = Time.timeScale * 0.02f;
        ChangeAudioPitch(slowedPitch , pitchRegain);

        isSlowed = true;
    }

    public void ChangeAudioPitch(float _newPitch, float _pitchRegain)
    {
        audio.pitch = _newPitch;
        pitchMin = _newPitch;

        pitchRegainControl = _pitchRegain;
    }
   
}
