using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class PolarBlast : MonoBehaviour
{

    public List<Transform> transformChain;
    public int chainJumps;
    //private QuadraticBezierChain beamPath;
    public Player3DControl parent;
    public PlayerLaserTorpedo beamProjectile;
    //private List<GameObject> targets;
    public Vector3 offsetFromParent;
    private Quaternion rotation;
    // Use this for initialization
    private bool fired;

    private Transform thisTransform;
	void Start () {
        //beamPath = GetComponent<QuadraticBezierChain>();
        thisTransform = GetComponent<Transform>();
        rotation = thisTransform.rotation;
        fired = false;
        //targets = new List<GameObject>();
	}

    

    

    // Update is called once per frame
    void Update() {
        if (fired)
        {
            fired = false;
            parent.energy = 0.0f;
        }

        if (Input.GetButtonDown("Fire3"))
        {
            if(parent.energy / 20 >= 1)
            {
                for(int i = 1; i <= parent.energy / 20; i++)
                {
                    
                    PlayerLaserTorpedo beam = Instantiate(beamProjectile, thisTransform.position, thisTransform.rotation) as PlayerLaserTorpedo;
                    Vector3 offset = new Vector3();
                    offset += thisTransform.up * 3 * i;
                    offset.y -= (i-1) * 3;
                    GetPath(offset);
                    beam.Init(transformChain, offset);
                    
                }
                fired = true;
            }

        }
	}

    


    private void GetPath(Vector3 offset)
    {
        List<GameObject> allEnemies = new List<GameObject>(GameObject.FindGameObjectsWithTag("Enemy"));


        transformChain.Clear();
        transformChain.Add(thisTransform);

        for (int i = 0; i < chainJumps; i++)
        {

            if (allEnemies.Count < 1)
            {
                break;
            }

            float nearestSqrDistance = Mathf.Infinity;
            GameObject nearestEnemy = null;


            foreach (GameObject enemy in allEnemies)
            {
                float sqrDistance;

                if (i == 0)
                {
                    sqrDistance = (enemy.transform.position - transformChain[0].position + offset).sqrMagnitude;
                }else
                {
                    sqrDistance = (enemy.transform.position - transformChain[i].position).sqrMagnitude;
                }
                if (sqrDistance < nearestSqrDistance)
                {
                    nearestSqrDistance = sqrDistance;
                    nearestEnemy = enemy;
                }
            }

            if (nearestEnemy)
            {
                transformChain.Add(nearestEnemy.transform);
                allEnemies.Remove(nearestEnemy);
            }
        }

    }


    private void LateUpdate()
    {
        thisTransform.position = parent.transform.position + offsetFromParent;
        thisTransform.rotation = rotation;
    }
}
