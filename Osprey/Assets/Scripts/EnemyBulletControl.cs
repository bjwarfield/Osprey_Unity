using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBulletControl : Entity {
	
    public float speed = 1.0f;

	void Start () {
        thisTransform = GetComponent<Transform>();
        //thisRenderer = GetComponent<SpriteRenderer>();
	}
	
	void Update () {
        thisTransform.Translate(thisTransform.forward * speed * Time.deltaTime, Space.World);
	}

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Destroy(gameObject);
        }
    }

}
