using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BGScroller : MonoBehaviour {

    public float vertSpeed = 0.1f;
    public float horzFactor = 0.1f;
    private float time;
    private Vector2 offset;
    public GameObject player;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        time += Time.deltaTime;

        if (player.activeSelf)
        {
            offset.Set(player.transform.position.x * horzFactor, time * vertSpeed);
        }
        else
        {
            offset.Set(offset.x * 0.9f , time * vertSpeed);
        }
        
        GetComponent<Renderer>().material.mainTextureOffset = offset;
	}
}
