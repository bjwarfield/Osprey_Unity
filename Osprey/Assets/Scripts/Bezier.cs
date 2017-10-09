using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bezier : MonoBehaviour {

    public LineRenderer line;

    public Transform point0, point1, point2;

    private Vector3[] positions = new Vector3[50];
    private int numPoints = 50;


	// Use this for initialization
	void Start () {
        line.positionCount = numPoints;
        

    }
	
	// Update is called once per frame
	void Update () {
        drawQuadraticCurve();
    }

    //Vector 3 lerp?
    //
    private Vector3 Linear(float t, Vector3 p0, Vector3 p1)
    {
        //P = P0 + t(P1-P0), 0<= t <= 1
        t = Mathf.Clamp(t, 0, 1);

        return p0 + (t * (p1 - p0));
    }

    private Vector3 Quadratic(float t, Vector3 p0, Vector3 p1, Vector3 p2)
    {
        t = Mathf.Clamp(t, 0, 1);
        //B(t) = (1-t)^2 P0 + 2(1-t) t P1 + t^2 P2 , 0 < t < 1

        return ((1 - t) * (1 - t) * p0) + (2* (1 - t) * t * p1) + (t * t * p2);
    }

    private void DrawLinearCurve()
    {
        for(int i = 0; i < numPoints; i++)
        {
            float t = i / ((float) numPoints -1.0f);
            positions[i] = Linear(t, point0.position, point1.position);
        }
        line.SetPositions(positions);
    }

    private void drawQuadraticCurve()
    {
        for (int i = 0; i < numPoints; i++)
        {
            float t = i / ((float)numPoints - 1.0f);
            positions[i] = Quadratic(t, point0.position, point1.position, point2.position);
        }
        line.SetPositions(positions);
    } 
}
