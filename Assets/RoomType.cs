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
        LevelGeneration.instance.modifyListExternal(false, LevelGeneration.instance.spawnedFloorObjects, this.gameObject);
        StartCoroutine(destroySelfOnDelay());
    }
    
    private IEnumerator destroySelfOnDelay()
    {
        yield return new WaitForSeconds(2f);
        Destroy(this.gameObject);
        stopCheck = true;

    }

}
