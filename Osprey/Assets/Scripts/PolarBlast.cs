using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class PolarBlast : MonoBehaviour
{

    public List<Transform> transformChain;
    public int chainJumps;
    private QuadraticBezierChain beam;
    public GameObject parent;
    public Vector3 offsetFromParent;
    private Quaternion rotation;
    // Use this for initialization

    private Transform thisTransform;
	void Start () {
        beam = GetComponent<QuadraticBezierChain>();
        thisTransform = GetComponent<Transform>();
        rotation = thisTransform.rotation;
	}

    // Update is called once per frame
    void Update() {
        List<GameObject> allEnemies = new List<GameObject>(GameObject.FindGameObjectsWithTag("Enemy"));

        transformChain.Clear();
        transformChain.Add(thisTransform);

        for (int i = 0; i < chainJumps; i++) {
            
            if(allEnemies.Count < 1)
            {
                break;
            }

            float nearestSqrDistance = Mathf.Infinity;
            GameObject nearestEnemy = null;


            foreach (GameObject enemy in allEnemies)
            {
                float sqrDistance = (enemy.transform.position - transformChain[0].position).sqrMagnitude;
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

        List<QuadraticBezierPoints> chain = new List<QuadraticBezierPoints>();

        for (int i = 0; i < transformChain.Count; i++)
        {
            Vector3 start, pull, end;

            start = transformChain[i].position;
            if(i == 0)
            {
                pull = transformChain[i].position + transformChain[i].up * 3.0f;
                if (i == transformChain.Count - 1)
                {
                    end = transformChain[i].position + transformChain[i].up * 4.0f;
                }
                else
                {
                    end = transformChain[i + 1].position;
                }
                    
            }
            else if( i == transformChain.Count - 1)
            {
                pull = transformChain[i].position + (transformChain[i].position - chain[i - 1].p1).normalized * 4;
                end = transformChain[i].position + (transformChain[i].position - chain[i - 1].p1).normalized * 8;
            }
            else
            {
                pull = transformChain[i].position + (transformChain[i].position - chain[i - 1].p1).normalized * 4.0f;
                end = transformChain[i + 1].position;
            }

            start.z = pull.z = end.z = 0;
            
            QuadraticBezierPoints link = new QuadraticBezierPoints(start, pull, end);

            chain.Add(link);
        }
        beam.SetBezierChain(chain);
	}

    private void LateUpdate()
    {
        thisTransform.position = parent.transform.position + offsetFromParent;
        thisTransform.rotation = rotation;
    }
}
