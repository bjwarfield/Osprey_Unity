using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBulletControl : MonoBehaviour {
	
    private Transform thisTransform = null;
    public float speed = 1.0f;

	void Start () {
        thisTransform = GetComponent<Transform>();
	}
	
	void Update () {
        thisTransform.Translate(thisTransform.up * speed * Time.deltaTime, Space.World);
	}

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Destroy(gameObject);
        }
    }
}
