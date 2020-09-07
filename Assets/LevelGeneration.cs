using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelGeneration : MonoBehaviour
{
    public Transform[] startingPositions;
    public GameObject[] blocks;

    private int direction;
    public float moveAmount;

    private float timeBetweenBlock;
    public float startTimeBetweenMove = 0.25f;

    // Start is called before the first frame update
    void Start()
    {
        int randStartinPos = Random.Range(0, startingPositions.Length);
        transform.position = startingPositions[randStartinPos].position;

        Instantiate(blocks[0], transform.position, Quaternion.identity);

        direction = Random.Range(1, 6);
    }

    void Update()
    {
        if(timeBetweenBlock <= 0)
        {
            Move();
            timeBetweenBlock = startTimeBetweenMove;
        }
        else
        {
            timeBetweenBlock -= Time.deltaTime;
        }
    }

    private void Move()
    {
        if (direction == 1 || direction == 2) //Right
        {
            Vector3 newPos = new Vector3(transform.position.x + moveAmount, transform.position.y, transform.position.z);
            transform.position = newPos;
        }
        else if (direction == 3 || direction == 4) //Left
        {
            Vector3 newPos = new Vector3(transform.position.x - moveAmount, transform.position.y, transform.position.z);
            transform.position = newPos;
        }
        else if (direction == 5) //Down
        {
            Vector3 newPos = new Vector3(transform.position.x, transform.position.y, transform.position.z - moveAmount);
            transform.position = newPos;
        }

        Instantiate(blocks[0], transform.position, Quaternion.identity);
        direction = Random.Range(1, 6);

    }
}
