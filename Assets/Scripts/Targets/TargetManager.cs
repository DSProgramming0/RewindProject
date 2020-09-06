using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetManager : MonoBehaviour
{
    public  List<CheckCubeHit> targetsInScene;

    void Start()
    {
        targetsInScene = new List<CheckCubeHit>();

        GameManager.instance.onLevelReset += resetTargets;
    }

    private void resetTargets()
    {
        foreach (CheckCubeHit target in targetsInScene)
        {
            target.resetTarget();
        }
    }
}
