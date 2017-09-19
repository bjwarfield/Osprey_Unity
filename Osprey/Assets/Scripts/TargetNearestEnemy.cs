using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetNearestEnemy : MonoBehaviour {
    private Transform thisTransform = null;
	// Use this for initialization
	void Start () {
        thisTransform = GetComponent<Transform>();
	}
	
	// Update is called once per frame
	void Update () {
        float nearestDistance = Mathf.Infinity;
        GameObject nearestEnemy = null;

        foreach (GameObject obj in GameObject.FindGameObjectsWithTag("Enemy"))
        {
            float distance = (thisTransform.position - obj.transform.position).sqrMagnitude;
            if(distance < nearestDistance)
            {
                nearestDistance = distance;
                nearestEnemy = obj;
            }
        }
        Debug.Log(nearestDistance);
        if (nearestEnemy)
        {
            //thisTransform.rotation = Quaternion.LookRotation(nearestEnemy.transform.position - thisTransform.position);
            //Vector3 targetPos = new Vector3(nearestEnemy.transform.position.x, nearestEnemy.transform.position.y, thisTransform.position.z);
            thisTransform.rotation = Quaternion.AngleAxis(Vector3.Angle(thisTransform.position, nearestEnemy.transform.position), Vector3.forward);

        }else
        {
            thisTransform.rotation = Quaternion.AngleAxis(0.0f, Vector3.forward);
        }
        
	}
}
