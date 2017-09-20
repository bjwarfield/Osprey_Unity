using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControl : MonoBehaviour
{
    //player speeed
    public float speed = 1.0f;
    public float horizontalRange = 3f;
    public float verticalRange = 6f;

    public GameObject bullet;
    public GameObject[] turrets;
    public GameObject missile;
    public GameObject[] missilePods;
    public float bulletPerSec = 10;
    public float missilePerSec;
    private float lastShot;
    private float lastLaunch;
    public bool godMode = true;
    public GameObject gib = null;

    private Transform thisTransform = null;

	// Use this for initialization
	void Start ()
    {
        thisTransform = GetComponent<Transform>();
	}
	
	// Update is called once per frame
	void Update ()
    {
        //move player with input keys
        Vector3 v3 = new Vector3(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"), 0.0f);
        thisTransform.Translate(speed * Time.deltaTime * v3.normalized);

        v3 = new Vector3(
            Mathf.Clamp(thisTransform.position.x, -horizontalRange, horizontalRange),
            Mathf.Clamp(thisTransform.position.y, -verticalRange, verticalRange),
            0.0f
            );
        thisTransform.position = v3;


        //fire shot
        if (Input.GetButton("Fire1") && Time.time > lastShot + (1.0f / bulletPerSec))
        //if (Time.time > lastShot + (1.0f / bulletPerSec))
        {
            foreach(GameObject turret in turrets)
            {
                Instantiate(bullet,turret.transform.position, turret.transform.rotation);
                lastShot = Time.time;
            }
            
        }

        if(Input.GetButton("Fire2") && Time.time > lastLaunch + (1.0f / missilePerSec))
        {
            foreach(GameObject pod in missilePods)
            {
                Instantiate(missile, pod.transform.position, pod.transform.rotation);
                lastLaunch = Time.time;
            }
        }

	}
    //---------------------------------------------------


    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("EnemyBullet") || other.CompareTag("Enemy"))
        {
            if (gib)
            {
                Instantiate(gib, thisTransform.position, gib.transform.rotation);
            }
            gameObject.SetActive(false);
            Invoke("Respawn", 3.0f);
        }
    }


    void Respawn()
    {
        thisTransform.position = new Vector3(0.0f, -verticalRange, 0.0f);
        gameObject.SetActive(true);
    }
}
