using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MissileControl : MonoBehaviour {

    public float missileAccelerationForce = 10.0f;
    public float maxVelocity = 10.0f;
    public float initialVelocity = 10.0f;
    private float maxSqrVelocity;

    private Transform thisTransform = null;
    private Rigidbody thisBody = null;

    private GameObject nearestObj = null;
    // Use this for initialization
    void Start () {
        thisTransform = GetComponent<Transform>();
        thisBody = GetComponent<Rigidbody>();

        thisBody.AddForce(thisTransform.up * initialVelocity, ForceMode.VelocityChange);

        float nearestDistance = Mathf.Infinity;

        foreach (GameObject obj in GameObject.FindGameObjectsWithTag("Enemy"))
        {
            float distance = (thisTransform.position - obj.transform.position).sqrMagnitude;
            if (distance < nearestDistance)
            {
                nearestDistance = distance;
                nearestObj = obj;
            }
        }

        maxSqrVelocity = maxVelocity * maxVelocity;
    }
	
	// Update is called once per frame
	void FixedUpdate ()
    {



        if(nearestObj)
        {
            Vector3 diff = nearestObj.transform.position - thisTransform.position;
            float ang = Mathf.Atan2(diff.y, diff.x) * Mathf.Rad2Deg;
            
            //Debug.Log(ang);
            thisTransform.rotation = Quaternion.AngleAxis(ang -90, Vector3.forward);

        }
        else
        {
            //thisTransform.rotation = Quaternion.AngleAxis(0.0f, Vector3.forward);
        }

        thisBody.AddForce(thisTransform.up * missileAccelerationForce * Time.deltaTime, ForceMode.Acceleration);

        if(thisBody.velocity.sqrMagnitude > maxSqrVelocity)
        {
            thisBody.velocity = thisBody.velocity.normalized * maxVelocity;
        }
	}

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy"))
        {
            Destroy(gameObject);
        }
    }

}
