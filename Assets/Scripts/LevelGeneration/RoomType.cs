using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomType : MonoBehaviour
{
    public List<GameObject> blocksInRoom;
    public int type;

    private bool stopCheck;

    void Start()
    {
        blocksInRoom = new List<GameObject>();
    }

    public void RoomDestruction()
    {
        Debug.Log("I am going to die! " + gameObject.name);
        Destroy(gameObject);
    }

    void Update()
    {
        if(LevelGeneration.instance.shouldResetLevel == true && !stopCheck)
        {
            RemoveSelf();
        }
    }

    public void RemoveSelf()
    {
        StartCoroutine(destroySelfOnDelay());
    }
    
    private IEnumerator destroySelfOnDelay()
    {
        yield return new WaitForSeconds(.5f);
        Destroy(this.gameObject);
        stopCheck = true;

    }

}
