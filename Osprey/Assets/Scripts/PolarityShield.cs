using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PolarityShield : Entity {
    public float gravStrength;
    public float gravMax;
    public Vector3 focusOffset; 

    private void Start()
    {
        thisTransform = GetComponent<Transform>();
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("EnemyBullet"))
        {
            EnemyBulletControl bullet = other.gameObject.GetComponent<EnemyBulletControl>();
            //Debug.Log(bullet.getPolarity());
            if (bullet.Polarity== polarity)
            {
                bullet.speed = 0;
                //bullet.transform.Translate((thisTransform.position) * Time.deltaTime * gravStrength, Space.Self);
                Vector3 diff = thisTransform.position - bullet.transform.position + focusOffset;
                //float ang = Mathf.Atan2(diff.y, diff.x) * Mathf.Rad2Deg;
                
                //bullet.transform.rotation = Quaternion.AngleAxis(ang - 90, Vector3.forward);
                //bullet.transform.position = Vector3.Slerp(bullet.transform.position, bullet.transform.position + focusOffset, 1.0f * Time.deltaTime);
                //bullet.speed += gravStrength;
                //bullet.speed = Mathf.Min(bullet.speed, gravMax);
                float lerpValue = Mathf.InverseLerp(0, diff.magnitude, gravStrength * Time.deltaTime);
                //Debug.Log(lerpValue);
                bullet.transform.position = Vector3.Lerp(bullet.transform.position, transform.position + focusOffset, lerpValue);




            }
        }
    }

    private void OnDrawGizmos()
    {
        
        Gizmos.DrawCube(transform.position + focusOffset, new Vector3(.2f, .2f, .2f));
    }
}
