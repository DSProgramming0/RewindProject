using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeBody : MonoBehaviour
{
    private bool isRewinding = false;
    private bool isPlayer;

    public float recordTime = 5f;

    List<PointInTime> pointsInTime;
    Rigidbody rb;

    // Start is called before the first frame update
    void Start()
    {
        isPlayer = GetComponent<CharacterController>();

        pointsInTime = new List<PointInTime>();

        if(!isPlayer)
            rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.R))
            StartRewind();
        if (Input.GetKeyUp(KeyCode.R))
            StopRewind();        
    }

    void FixedUpdate()
    {
        if (isRewinding)
            Rewind();
        else
            Record();
    }

    void Rewind()
    {
        if(pointsInTime.Count > 0)
        {
            PostProcessingManager.instance.setProfile(PostProcessingManager.instance.rewindingProfile);

            PointInTime pointInTime = pointsInTime[0];
            transform.position = pointInTime.position;
            transform.rotation = pointInTime.rotation;
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

        if(!isPlayer)
            rb.isKinematic = true;
    }

    void StopRewind()
    {
        isRewinding = false;
        PostProcessingManager.instance.removeProfile();

        if (!isPlayer)
            rb.isKinematic = false;
    }
}

