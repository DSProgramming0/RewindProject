using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeBody : MonoBehaviour
{
    [SerializeField] private TimeManager timeManager;

    public bool isRewinding = false;
    private bool isPlayer;

    public float recordTime = 5f;

    List<PointInTime> pointsInTime;
    Rigidbody rb;

    // Start is called before the first frame update
    void Start()
    {
        GameManager.instance.onLevelReset += clearPointsInTime;
        isPlayer = GetComponent<CharacterController>();

        pointsInTime = new List<PointInTime>();

        if(!isPlayer)
            rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            if(isPlayer && timeManager.canUseTimeSkills)
            {
                StartRewind();
            }
        }
        if (Input.GetKeyUp(KeyCode.R))
            StopRewind();      
    }

    void FixedUpdate()
    {
        if (isRewinding)        {
            
            Rewind();
        }
        else
            Record();
    }

    void Rewind()
    {
        if(pointsInTime.Count > 0)
        {            
            PostProcessingManager.instance.setVolumeState(VolumeStates.REWINDING);

            PointInTime pointInTime = pointsInTime[0];
            transform.position = pointInTime.position;
            transform.rotation = pointInTime.rotation;

            if(isPlayer)
            {
                timeManager.ChangeAudioPitch(-.46f, 1.4f);
            }

            pointsInTime.RemoveAt(0);
        }
        else
        {
            StopRewind();
        }        
    }

    void Record()
    {
        if (pointsInTime.Count > Mathf.Round(5F / Time.fixedDeltaTime))
        {
            pointsInTime.RemoveAt(pointsInTime.Count - 1);
        }       

        pointsInTime.Insert(0, new PointInTime(transform.position, transform.rotation));    
    }

    void StartRewind()
    {
        isRewinding = true;

        if (!isPlayer)
            rb.isKinematic = true;
    }

    void clearPointsInTime()
    {
        pointsInTime.Clear();
    }

    void StopRewind()
    {
        isRewinding = false;
        PostProcessingManager.instance.setVolumeState(VolumeStates.STANDARD);
        if (!isPlayer)
            rb.isKinematic = false;
    }
}

