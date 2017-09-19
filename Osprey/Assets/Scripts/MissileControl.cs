using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MissileControl : MonoBehaviour {

    public float missileAccelerationForce = 10.0f;
    public float maxVelocity = 10.0f;
    public float initialVelocity = 10.0f;


    private Transform thisTransform = null;
    private Rigidbody thisBody = null;
	// Use this for initialization
	void Start () {
        thisTransform = GetComponent<Transform>();
        thisBody = GetComponent<Rigidbody>();

        //thisBody.AddForce(thisTransform.up * initialVelocity, ForceMode.VelocityChange);
	}
	
	// Update is called once per frame
	void FixedUpdate () {
        float nearestDistance = Mathf.Infinity;
        GameObject nearestObj = null;
        foreach(GameObject obj in GameObject.FindGameObjectsWithTag("Enemy"))
        {
            float distance = (thisTransform.position - obj.transform.position).sqrMagnitude;
            if(distance < nearestDistance)
            {
                nearestDistance = distance;
                nearestObj = obj;
            }
        }


        if(nearestObj)
        {
            Vector3 diff = nearestObj.transform.position - thisTransform.position;
            float ang = Mathf.Atan2(diff.y, diff.x) * Mathf.Rad2Deg;
            
            Debug.Log(ang);
            thisTransform.rotation = Quaternion.AngleAxis(ang -90, Vector3.forward);

        }
        else
        {
            thisTransform.rotation = Quaternion.AngleAxis(0.0f, Vector3.forward);
        }
	}
}
