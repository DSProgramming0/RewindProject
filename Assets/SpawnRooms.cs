using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnRooms : MonoBehaviour
{
    public LayerMask whatIsRoom;
    public LevelGeneration levelGen;

    [SerializeField] Collider[] roomDetection;

    // Update is called once per frame
    void Update()
    {
        roomDetection = Physics.OverlapSphere(transform.position, 0.1f, whatIsRoom);
        if(roomDetection.Length == 0 && levelGen.stopGenerating == true)
        {
            Debug.Log("Spawning");
            int rand = Random.Range(0, levelGen.emptyBlocks.Length);
            GameObject spawnedFloorBlock = Instantiate(levelGen.emptyBlocks[rand], transform.position, Quaternion.identity);
            levelGen.modifyListExternal(true, levelGen.spawnedFloorObjects, spawnedFloorBlock);
            Destroy(gameObject);
        }
    }
}
