using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GibOnTrigger : MonoBehaviour {

    public GameObject gib = null;
    private Transform thisTransform = null;

	// Use this for initialization
	void Start () {
        thisTransform = GetComponent<Transform>();
	}
	


    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("PlayerBullet") || other.CompareTag("Player"))
        {
            if (gib)
            {
                Instantiate(gib, thisTransform.position, gib.transform.rotation);
            }
            Destroy(gameObject);
        }
    }
}
