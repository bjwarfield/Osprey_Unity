using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnObjects : MonoBehaviour {

    public GameObject objToSpawn = null;
    public float spawnDelay = 1.0f;
    private float lastSpawn = 0;
	// Use this for initialization
	void Start () {
		
	}
	
	
	void Update () {

        if(Time.time > lastSpawn + spawnDelay)
        {
            Instantiate(objToSpawn, GetComponent<Transform>().position, GetComponent<Transform>().rotation);
            lastSpawn = Time.time;
            
        }

	}
}
