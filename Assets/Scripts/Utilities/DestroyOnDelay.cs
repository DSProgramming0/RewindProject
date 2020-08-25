using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyOnDelay : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(destroyThisOnDelay());
    }

    IEnumerator destroyThisOnDelay()
    {
        yield return new WaitForSeconds(2f);
        Destroy(this.gameObject);
    }
}
