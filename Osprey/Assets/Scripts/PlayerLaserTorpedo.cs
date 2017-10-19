using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerLaserTorpedo : Entity{
    public float speed;
    public QuadraticBezierChain targetPath;
    private float startTime;
    public List<Transform> transformPath;
    private Vector3 zeroStart, zeroPull;

    private void Start()
    {
        thisTransform = GetComponent<Transform>();
    }
    void Awake()
    {
        startTime = Time.time;
        targetPath = GetComponent<QuadraticBezierChain>();
    }
	
	// Update is called once per frame
	void Update () {
        if (targetPath)
        {
            
            targetPath.SetBezierChain(TransformsToBezierPoints(transformPath));
            thisTransform.position = targetPath.GetPoint((Time.time - startTime) * speed);
            

        }
            
	}
    public void Init(List<Transform> path)
    {
        transformPath = new List<Transform>(path);
        targetPath.SetBezierChain(TransformsToBezierPoints(transformPath));
        zeroStart = transformPath[0].position;
        zeroPull = transformPath[0].position + transformPath[0].up * 3.0f;
    }


    private void OnTriggerEnter(Collider other)
    {
        
            if (other.CompareTag("Enemy"))
            {
                  Instantiate(gibs, thisTransform.position, Quaternion.FromToRotation(thisTransform.position, targetPath.GetPoint((Time.time - startTime + 0.1f) * speed)));
            }
        
    }

    public List<QuadraticBezierPoints> TransformsToBezierPoints(List<Transform> transformList)
    {
        List<QuadraticBezierPoints> chain = new List<QuadraticBezierPoints>();

        for (int i = 0; i < transformList.Count; i++)
        {
            Vector3 start, pull, end;

            start = transformList[i].position;
            if (i == 0)
            {
                //pull = transformList[i].position + transformList[i].up * 3.0f;
                start = zeroStart;
                pull = zeroPull;
                if (i == transformList.Count - 1)
                {
                    end = transformList[i].position + transformList[i].up * 4.0f;
                }
                else
                {
                    end = transformList[i + 1].position;
                }
                
            }
            else if (i == transformList.Count - 1)
            {
                pull = transformList[i].position + (transformList[i].position - chain[i - 1].p1).normalized * 4;
                end = transformList[i].position + (transformList[i].position - chain[i - 1].p1).normalized * 8;
            }
            else
            {
                pull = transformList[i].position + (transformList[i].position - chain[i - 1].p1).normalized * 4.0f;
                end = transformList[i + 1].position;
            }


            start.z = pull.z = end.z = 0;

            QuadraticBezierPoints link = new QuadraticBezierPoints(start, pull, end);

            chain.Add(link);
        }

        return chain;
    }

    
}
