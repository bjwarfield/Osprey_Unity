using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerLaserTorpedo : Entity{
    public float speed;
    public QuadraticBezierChain targetPath;
    private float startTime;
    public List<Transform> transformPath;
    public Vector3[] vectorPath;
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
        Vector3 offset = new Vector3((transformPath[0].up * 5.0f).x, (transformPath[0].up * 5.0f).y, 0);
        
        transformPath = new List<Transform>(path);
        vectorPath = new Vector3[path.Count];
        targetPath.SetBezierChain(TransformsToBezierPoints(transformPath));
        zeroStart = transformPath[0].position;
        zeroPull = transformPath[0].position + offset;
        
    }

    public void Init(List<Transform> path, Vector3 offset)
    {
        transformPath = new List<Transform>(path);
        vectorPath = new Vector3[path.Count];
        targetPath.SetBezierChain(TransformsToBezierPoints(transformPath));
        zeroStart = transformPath[0].position;
        zeroPull = transformPath[0].position + offset;

        
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
        
        for(int i = 0; i < vectorPath.Length; i++)
        {
            if(transformList[i])
            {
                vectorPath[i] = transformList[i].position;
            }
        }

        List<QuadraticBezierPoints> chain = new List<QuadraticBezierPoints>();

        for (int i = 0; i < transformList.Count; i++)
        {
            Vector3 start, pull, end;

            start = vectorPath[i];

            //set initial positions of curve
            if (i == 0)
            {
                //pull = transformList[i].position + transformList[i].up * 3.0f;
                start = zeroStart;
                pull = zeroPull;
                if (i == transformList.Count - 1)
                {
                    end = zeroPull + Vector3.up * 4.0f;
                }
                else
                {
                    end = vectorPath[i + 1];
                }
                
            }
            //set end position of curve
            else if (i == transformList.Count - 1)
            {
                pull = vectorPath[i] + (vectorPath[i] - chain[i - 1].p1).normalized * 4;
                end = vectorPath[i] + (vectorPath[i] - chain[i - 1].p1).normalized * 8;
            }
            //set middle positions of curve
            else
            {
                pull = vectorPath[i] + (vectorPath[i] - chain[i - 1].p1).normalized * 4.0f;
                end = vectorPath[i + 1];
            }


            start.z = pull.z = end.z = 0;

            QuadraticBezierPoints link = new QuadraticBezierPoints(start, pull, end);

            chain.Add(link);
        }

        return chain;
    }

    
}
