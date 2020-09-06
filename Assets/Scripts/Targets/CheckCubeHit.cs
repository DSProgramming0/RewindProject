using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckCubeHit : MonoBehaviour
{
    private TargetManager manager;

    private Vector3 startPos;
    private Quaternion startRot;  

    private float detectionThreshold = 0.5f;
    [SerializeField] private float cubeHitValue;
    private bool hasBeenHit = false;

    private MeshRenderer mesh;
    private Material startMaterial;
    public Material hitMaterial;

    void Awake()
    {
        manager = FindObjectOfType<TargetManager>();
    }

    void Start()
    {
        mesh = GetComponent<MeshRenderer>();
        startMaterial = mesh.material;

        cubeHitValue =  Random.Range(1f, 2.5f);

        Invoke("addDataToList", .5f);
    }

    void addDataToList()
    {
        manager.targetsInScene.Add(this);

        startPos = transform.position;
        startRot = transform.rotation;
    }

    // Update is called once per frame
    void OnCollisionEnter(Collision collision)
    {
        // Debug-draw all contact points and normals
        foreach (ContactPoint contact in collision.contacts)
        {
            Debug.DrawRay(contact.point, contact.normal, Color.white);
        }

        // Play a sound if the colliding objects had a big impact.
        if (collision.relativeVelocity.magnitude > detectionThreshold)
        {
            mesh.material = hitMaterial;
            rewardPoints();
        }

        if (collision.gameObject.GetComponent<Explosion>())
        {
            mesh.material = hitMaterial;
            rewardPoints();
        }
    }

    private void rewardPoints()
    {
        if(hasBeenHit == false)
        {
            GameManager.instance.recieveTargetHitPoints(cubeHitValue);
            SoundManager.Play3DSound(SoundManager.Sound.TargetHit, transform.position);
            hasBeenHit = true;
        }
    }

    public void resetTarget()
    {
        mesh.material = startMaterial;
        hasBeenHit = false;

        transform.position = startPos;
        transform.rotation = startRot;
    }
}
