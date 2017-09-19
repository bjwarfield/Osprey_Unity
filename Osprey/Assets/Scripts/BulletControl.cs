using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletControl : MonoBehaviour {

    private Transform thisTransform = null;
    public float speed = 1.0f;

	// Use this for initialization
	void Start () {
        thisTransform = GetComponent<Transform>();
	}
	
	// Update is called once per frame
	void Update ()
    {
        //Vector3 movement = new Vector3(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"), 0.0f);
        //thisTransfrom.Translate(speed * Time.deltaTime * movement.normalized);
        thisTransform.Translate(thisTransform.up * speed * Time.deltaTime, Space.World);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy"))
        {
            Destroy(gameObject);
        }
    }
}
