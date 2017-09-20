using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateBackAndForth2D : MonoBehaviour {

    public float startAngle = 0.0f;
    public float range = 0.0f;
    public float period = 1.0f;
    private bool forth = true;


    //private float startTime;
    private float timer = 0.0f;
    private float angle;

    private Transform thisTransform = null;

	// Use this for initialization
	void Start () {
        thisTransform = GetComponent<Transform>();
        //startTime = Time.time;

        startAngle = startAngle % 360;
        if (startAngle < 0) startAngle += 360;
        
        //angle = thisBody.transform.rotation.eulerAngles.z;
	}
	
	// Update is called once per frame
	void Update () {


        if (forth)
        {
            angle = startAngle + ((timer / period) * range);
            
        }
        else
        {
            angle = startAngle + ((1-(timer / period)) * range);
        }

        thisTransform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
        timer += Time.deltaTime;

        if(timer >= period)
        {
            timer = 0;
            forth = !forth;
        }

       

	}
}
