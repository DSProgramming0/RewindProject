
using UnityEngine;

public class TimeManager : MonoBehaviour
{
    [SerializeField] private AudioSource audio;

    private float pitchMin;
    private float pitchRegainControl;

    private float pitchRegain = 1f;
    private float slowDownFactor = 0.05f;
    private float slowDownLength = 5f;
    private float slowedPitch = .4f;

    void Start()
    {
        DoSlowMotion();
    }

    void Update()
    {
        Time.timeScale += (1f / slowDownLength) * Time.unscaledDeltaTime;
        Time.timeScale = Mathf.Clamp(Time.timeScale, 0F , 1F);

        audio.pitch += (1f / pitchRegainControl) * Time.deltaTime;
        audio.pitch = Mathf.Clamp(audio.pitch, pitchMin, 1F);
    }

    public void DoSlowMotion()
    {
        Time.timeScale = slowDownFactor;

        Time.fixedDeltaTime = Time.timeScale * 0.02f;
        ChangeAudioPitch(slowedPitch , pitchRegain);
    }

    public void ChangeAudioPitch(float _newPitch, float _pitchRegain)
    {
        audio.pitch = _newPitch;
        pitchMin = _newPitch;

        pitchRegainControl = _pitchRegain;
    }
   
}
