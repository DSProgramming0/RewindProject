using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCollisions : MonoBehaviour
{
    PlayerMovement controller;
    private bool soundHasPlayed = false;
    [SerializeField] private GameObject currentSurface = null;

    void Start()
    {
        controller = GetComponent<PlayerMovement>();
    }

    void OnControllerColliderHit(ControllerColliderHit hit)
    {       
        if (hit.gameObject.CompareTag("StartingCube"))
        {
            Debug.Log("Hit start Cube");
            GameManager.instance.LevelStart();

            currentSurface = hit.gameObject;

            if(!soundHasPlayed)
            {
                SoundManager.Play3DSound(SoundManager.Sound.ReachedStartLine, transform.position);
            }

            if (currentSurface == hit.gameObject)
            {
                soundHasPlayed = true;
            }
        }
        else if (hit.gameObject.CompareTag("FinishCube"))
        {
            Debug.Log("Hit finish Cube");
            GameManager.instance.FinishLineReached();

            currentSurface = hit.gameObject;

            if (!soundHasPlayed)
            {
                SoundManager.Play3DSound(SoundManager.Sound.ReachedFinishLine, transform.position);
            }

            if (currentSurface == hit.gameObject)
            {
                soundHasPlayed = true;
            }

        }
        else if (hit.gameObject.CompareTag("DeathFloor"))
        {
            GameManager.instance.PlayerDied();

            currentSurface = hit.gameObject;

            if (!soundHasPlayed)
            {
                SoundManager.Play3DSound(SoundManager.Sound.Death, transform.position);
            }

            if (currentSurface == hit.gameObject)
            {
                soundHasPlayed = true;
            }
        }
        else
        {            
            return;
        }
    }

    void Update()
    {
        if (!controller.isGrounded)
        {
            soundHasPlayed = false;
            currentSurface = null;
        }
    }
}
