using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyOnInvisible : MonoBehaviour {

    public GameObject destroyTarget = null;

    private void OnBecameInvisible()
    {
        if (destroyTarget)
        {
            Destroy(destroyTarget);
        }else
        {
            Destroy(gameObject);
            //Debug.Log("Depop!");
        }
        
    }
}
