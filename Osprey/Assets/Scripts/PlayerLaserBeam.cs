using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerLaserBeam : Entity{
    public float speed;
    public QuadraticBezierChain beamPath;
    private float startTime;
    private List<GameObject> targets;

    private void Start()
    {
        thisTransform = GetComponent<Transform>();
    }
    void Awake()
    {
        startTime = Time.time;
    }
	
	// Update is called once per frame
	void Update () {
        if (beamPath)
        {
            thisTransform.position = beamPath.GetPoint((Time.time - startTime) * speed);

            //TODO: track targets in list
            //update points if they move, freeze points if they die
        }
            
	}
    public void Init(QuadraticBezierChain path, List<GameObject> objs)
    {
        beamPath = path;
        targets = new List<GameObject>(objs);

    }
}
