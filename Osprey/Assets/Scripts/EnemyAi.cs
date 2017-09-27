using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAi : Entity {
    public float speed = 1.0f;

    public GameObject playerTarget;
    void Start () {
        thisTransform = GetComponent<Transform>();
        thisRenderer.material.mainTexture = textures[(int)polarity];
    }
	
	void Update () {

        foreach(GameObject barrel in gunBarrel)
        {
            if (Time.time - lastShot > 1 / shotFireRate)
            {
                if (playerTarget.activeSelf)
                {
                    Vector3 diff = playerTarget.transform.position - barrel.transform.position;
                    float ang = Mathf.Atan2(diff.y, diff.x) * Mathf.Rad2Deg;
                    barrel.transform.rotation = Quaternion.AngleAxis(ang -90, Vector3.forward);
                }
                Instantiate(shot[(int)polarity], barrel.transform.position, barrel.transform.rotation);
                lastShot = Time.time;
            }

        }
        
	}
}
