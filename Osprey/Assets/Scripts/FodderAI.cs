using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FodderAI : MonoBehaviour {

    public float speed = 1.0f;
    public float cosAmplitude = 0.1f;
    public float cosFrequency = 10.0f;
    

    private Transform thisTransform = null;

    private float time = 0.0f;
	// Use this for initialization
	void Start () {
        thisTransform = GetComponent<Transform>();
        time  += Random.value;
	}
	
	// Update is called once per frame
	void Update () {
        time += Time.deltaTime;
        thisTransform.Translate(thisTransform.up * -speed * Time.deltaTime);
        //adjust position by sin wave

        thisTransform.Translate(thisTransform.right * cosAmplitude * Mathf.Cos(time * cosFrequency * 2 * Mathf.PI));
        

    }
}
