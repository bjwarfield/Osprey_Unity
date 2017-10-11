using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerLaserBeam : Entity{
    public float speed;
    public QuadraticBezierChain beamPath;
    private float startTime;

	void Awake()
    {

    }
	
	// Update is called once per frame
	void Update () {
		
	}
    public void init(QuadraticBezierChain path)
    {
        beamPath = path;
    }
}
