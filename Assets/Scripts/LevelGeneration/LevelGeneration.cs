using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelGeneration : MonoBehaviour
{
    public static LevelGeneration instance;
    public List<GameObject> spawnedObjects;
    public List<GameObject> spawnedFloorObjects;

    public bool shouldResetLevel;

    private Vector3 LevelGenStartPos;
    public Transform[] startingPositions;
    public GameObject[] blocks; //index corresponds to different room types
    public GameObject[] emptyBlocks;

    private int direction;
    public float moveAmount;

    [SerializeField] private float timeBetweenBlock;
    public float startTimeBetweenMove = 5f;

    public float minX;
    public float maxX;
    public float minY;
    public bool stopGenerating;
    private bool deleteComplete;

    public LayerMask roomLayerMask;

    private int downCounter;

    [SerializeField] private GameObject lastSpawnedObj;
    [SerializeField] private List<GameObject> blocksInRoom;
    
    void Awake()
    {
        instance = this;
    }
    // Start is called before the first frame update
    void Start()
    {
        GameManager.instance.onLevelReset += resetGeneratedLevel;

        int randStartinPos = Random.Range(0, startingPositions.Length);
        transform.position = startingPositions[randStartinPos].position;

        Instantiate(blocks[0], transform.position, Quaternion.identity);

        direction = Random.Range(1, 6);

        spawnedFloorObjects = new List<GameObject>();
        spawnedObjects = new List<GameObject>();
        blocksInRoom = new List<GameObject>();
    }

    void Update()
    {
        if(timeBetweenBlock <= 0 && stopGenerating == false) //Checks if it is time to spawn next block and if it should generate using bool
        {
            Move();
            timeBetweenBlock = startTimeBetweenMove;
        }
        else
        {
            timeBetweenBlock -= Time.deltaTime;
        }

        if(stopGenerating == true && shouldResetLevel == false)
        {
            chooseFinishBlock();
        }
    }

    public void modifyListExternal(bool _shouldAdd, List<GameObject> _list, GameObject _objectToAdd)
    {
        if (_shouldAdd)
            _list.Add(_objectToAdd);
        else
            _list.Remove(_objectToAdd);
    }

    public void resetGeneratedLevel()
    {
        Debug.Log("Reseting from " + this + " class.");
        shouldResetLevel = true; //Stios choosingFinal Block
        StartCoroutine(startResetting());
    }

    private IEnumerator startResetting()
    {
        yield return new WaitForSeconds(1.5f);
        Debug.Log("Clearing Lists");
        spawnedFloorObjects.Clear();
        spawnedObjects.Clear();
        lastSpawnedObj = null;

        yield return new WaitForSeconds(.5f);
        ResetLevel();
        yield return new WaitForSeconds(.1f);

        shouldResetLevel = false;

        StopCoroutine(startResetting());
    }

    private void ResetLevel()
    {
        int randStartinPos = Random.Range(0, startingPositions.Length);
        transform.position = startingPositions[randStartinPos].position;

        Instantiate(blocks[0], transform.position, Quaternion.identity);
        direction = Random.Range(1, 6);
        
        timeBetweenBlock = 0;
        stopGenerating = false;
    }

    private void chooseFinishBlock()
    {
        blocksInRoom = lastSpawnedObj.GetComponent<RoomType>().blocksInRoom; //Changes color of each block in a room
        foreach (GameObject block in blocksInRoom)
        {
            MeshRenderer mesh = block.GetComponent<MeshRenderer>();
            mesh.material.color = Color.red;
            mesh.gameObject.tag = "FinishCube";
        }
    }
    
    private void Move()
    {
        if (direction == 1 || direction == 2) //Right
        {
            if (transform.position.x < maxX) //Check if the next object can spawn right of previous one without hitting borders
            {
                downCounter = 0;

                Vector3 newPos = new Vector3(transform.position.x + moveAmount, transform.position.y, transform.position.z);
                transform.position = newPos;

                int rand = Random.Range(0, blocks.Length);
                lastSpawnedObj = Instantiate(blocks[rand], transform.position, Quaternion.identity);
                spawnedObjects.Add(lastSpawnedObj);

                direction = Random.Range(1, 6);
                if(direction == 3)
                {
                    direction = 2;
                } else if (direction == 4)
                {
                    direction = 5;
                }
            }
            else //change direction
            {
                direction = 5;
            }
        }
        else if (direction == 3 || direction == 4) //Left
        {
            if (transform.position.x > minX) 
            {
                downCounter = 0;
                Vector3 newPos = new Vector3(transform.position.x - moveAmount, transform.position.y, transform.position.z);
                transform.position = newPos;

                int rand = Random.Range(0, blocks.Length);
                lastSpawnedObj = Instantiate(blocks[rand], transform.position, Quaternion.identity);
                spawnedObjects.Add(lastSpawnedObj);

                direction = Random.Range(3, 6);             
            }
            else
            {
                direction = 5;
            }
             
        }
        else if (direction == 5) //Down
        {
            downCounter++;
            if(transform.position.z > minY)
            {
                if (downCounter >= 2)
                {

                    lastSpawnedObj = Instantiate(blocks[3], transform.position, Quaternion.identity);
                    spawnedObjects.Add(lastSpawnedObj);

                }
                else
                {

                    int randBottomRoom = Random.Range(1, 4);

                    if (randBottomRoom == 2)
                    {
                        randBottomRoom = 1;
                    }

                    lastSpawnedObj = Instantiate(blocks[randBottomRoom], transform.position, Quaternion.identity);
                    spawnedObjects.Add(lastSpawnedObj);
                }

                Vector3 newPos = new Vector3(transform.position.x, transform.position.y, transform.position.z - moveAmount);
                transform.position = newPos;

                int rand = Random.Range(2, 4);
                lastSpawnedObj = Instantiate(blocks[rand], transform.position, Quaternion.identity);
                spawnedObjects.Add(lastSpawnedObj);

                direction = Random.Range(1, 6);
            }
            else
            {
                stopGenerating = true;
               
            }
        }
    }
}
