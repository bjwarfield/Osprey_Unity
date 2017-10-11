using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Class defining the three points in each quadratic link in the chain
//param p0: Starting Point
//param p1: Pull point, Defines curvature between p0 and p2
//param p2: End Point
[System.Serializable]
public class QuadraticBezierPoints
{
    public Vector3 p0, p1, p2;
    public QuadraticBezierPoints(Vector3 start, Vector3 pull, Vector3 end)
    {
        p0 = start;
        p1 = pull;
        p2 = end;
    }
}

[ExecuteInEditMode]
public class QuadraticBezierChain : MonoBehaviour
{

    //line segments per link in chain
    public int subdivisionsPerSection;
    //total line segments in chain
    private int totalSubdivisions;
    //container holding control points in each link
    public QuadraticBezierPoints[] bezierChain;

    //there is a hidden line renderer thing here

    public bool useLineRenderer;

    public bool stayWithTransform;
    public bool useTransformScale;
    public bool showGizmos;

    //each line segment in chain
    private Vector3[] subDivisionPoints;

    //flag for users to see editor changes at runtime
    public bool continualRecalculate;
    //flag set when functions are called that modify positions/tangents
    private bool oneTimeRecalculate;

    private LineRenderer lineRenderer;



    // Use this for initialization
    void Start()
    {
    }

    private void OnDrawGizmos()
    {
        CheckRecalculate();

        
        if (showGizmos)
        {

            for (int i = 1; i < subDivisionPoints.Length; i++)
            {
                Gizmos.DrawLine(subDivisionPoints[i - 1], subDivisionPoints[i]);
            }

            for (int i = 0; i < bezierChain.Length; i++)
            {
                Gizmos.DrawSphere(bezierChain[i].p0, 0.2f);
                Gizmos.DrawSphere(bezierChain[i].p1, 0.2f);
                Gizmos.DrawSphere(bezierChain[i].p2, 0.2f);
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        CheckRecalculate();
    }

    //calculate all points on curve chain
    private void CheckRecalculate()
    {
        //set default division count if not set
        if (subDivisionPoints == null)
        {
            //init to 16, user can change this later
            subdivisionsPerSection = 16;
            oneTimeRecalculate = true;
        }

        //check for line renderer


        if (useLineRenderer && lineRenderer == null)
        {
            GetLineRenderer();
            oneTimeRecalculate = true;
        }else if(!useLineRenderer && lineRenderer)
        {
            Destroy(lineRenderer);

        }

        //create basic line curve if null
        if (bezierChain == null)
        {
            bezierChain = new QuadraticBezierPoints[1];
            bezierChain[0] = new QuadraticBezierPoints(Vector3.right, Vector3.up, Vector3.right);
            oneTimeRecalculate = true;
        }

        //calculate and draw points
        if (oneTimeRecalculate || continualRecalculate)
        {
            oneTimeRecalculate = false;
            RecalculateSubdivisions();
        }
    }

    //Capture line renderer component. Create one if one does not exist
    private void GetLineRenderer()
    {
        lineRenderer = GetComponent<LineRenderer>();
        if (lineRenderer == null)
        {
            lineRenderer = gameObject.AddComponent<LineRenderer>();
            lineRenderer.startWidth = 0.25f;
            lineRenderer.endWidth = 0.25f;
        }
    }

    


    private void RecalculateSubdivisions()
    {
        //bind range of subdivisions
        Mathf.Clamp(subdivisionsPerSection, 1, 100);

        totalSubdivisions = subdivisionsPerSection * bezierChain.Length;
        int subDivisionLength = totalSubdivisions + bezierChain.Length;

        subDivisionPoints = new Vector3[subDivisionLength];
        //subDivisionPoints = new Vector3[totalSubdivisions];

        int subdivisionIndex = 0;

        for(int n = 0; n < bezierChain.Length; n++)
        {
            float t;
            float one_minus_t;

            for(int i = 0; i <= subdivisionsPerSection; i++)
            {
                t = (float)i / (float)subdivisionsPerSection;
                one_minus_t = 1 - t;

                subDivisionPoints[subdivisionIndex] = (one_minus_t * one_minus_t) * bezierChain[n].p0
                                                    + (2 * one_minus_t * t) * bezierChain[n].p1
                                                    + (t * t) * bezierChain[n].p2;

           

                if (useTransformScale)
                {
                    subDivisionPoints[subdivisionIndex].x = subDivisionPoints[subdivisionIndex].x * transform.lossyScale.x;
                    subDivisionPoints[subdivisionIndex].y = subDivisionPoints[subdivisionIndex].y * transform.lossyScale.y;
                    subDivisionPoints[subdivisionIndex].z = subDivisionPoints[subdivisionIndex].z * transform.lossyScale.z;
                }

                //sngle * point + position
                if (stayWithTransform)
                {
                    subDivisionPoints[subdivisionIndex] = transform.rotation * subDivisionPoints[subdivisionIndex] + transform.position;
                }
                subdivisionIndex++;
            }
        }
  
        UpdateLineRenderer(subDivisionLength);
        //UpdateLineRenderer(totalSubdivisions);

    }

    private void UpdateLineRenderer(int length)
    {
        if (useLineRenderer)
        {
            if(lineRenderer == null){
                GetLineRenderer();
            }


            lineRenderer.positionCount = length;

            for(int i = 0; i < length; i++)
            {
                lineRenderer.SetPosition(i, subDivisionPoints[i]);
            }
        }
        else if(lineRenderer)
        {
            Destroy(lineRenderer);
            lineRenderer = null;
        }
    }

    public void SetBezierChain(List<QuadraticBezierPoints> chain, bool recalculateSubdivisions = true)
    {
        bezierChain = new QuadraticBezierPoints[chain.Count];
        for (int i = 0; i < bezierChain.Length; i++)
        {
            bezierChain[i] = chain[i];
        }
        if (recalculateSubdivisions)
        {
            oneTimeRecalculate = true;
        }
    }

    public void SetBezierChain(QuadraticBezierPoints[] chain, bool recalculateSubdivisions = true)
    {
        bezierChain = chain;
        if (recalculateSubdivisions)
        {
            oneTimeRecalculate = true;
        }

    }

    //get a curve from the chain of bezier curves
    public QuadraticBezierPoints GetCurveFromChain(int chainIndex)
    {
        if(chainIndex >= 0 && chainIndex < bezierChain.Length)
        {
            return bezierChain[chainIndex];
        }
        else
        {
            return null;
        }
    }

    //set a curve in the chain of bezier curves
    public bool SetCurveInChain(int chainIndex, QuadraticBezierPoints curve, bool recalculateSubdivisions = true)
    {
        if(chainIndex >= 0 && chainIndex > bezierChain.Length)
        {
            bezierChain[chainIndex] = curve;
            if (recalculateSubdivisions)
            {
                oneTimeRecalculate = true;
            }
            return true;
        }else
        {
            return false;
        }
    }


    public Vector3 GetSubDivisonPoint(int index)
    {
        if(index >=0 && index < subDivisionPoints.Length)
        {
            return subDivisionPoints[index];
        }
        else
        {
            return Vector3.negativeInfinity;
        }
        
    }


    //return a array of distances from origin of vectors in chain 
    public float[] GetLengths(Vector3[] controlPoints)
    {
        // lets create lengths for each control point.
        float[] lengths = new float[controlPoints.Length];
        float totalDistance = 0;
        float distance = 0;

        // go from the first, to the second to last
        for (var i = 0; i < controlPoints.Length - 1; i++)
        {
            // set the array value to the distance
            lengths[i] = totalDistance;
            // then get the next distance
            distance = Vector3.Distance(controlPoints[i], controlPoints[i + 1]);
            totalDistance += distance;
        }
        // set the last length
        lengths[lengths.Length - 1] = totalDistance;
        return lengths;
    }

    //retrun a vector point on the chain x distance from the origin point
    public Vector3 GetPoint (float distance)
    {
        

        float[] lengths = GetLengths(subDivisionPoints);

        // get the total distance of the points.
        var totalDistance = lengths[lengths.Length - 1];

        // if distance is negative, return a proportional distance in the negative direction of the first segment
        if (distance <= 0)
        {
            return subDivisionPoints[0]
                + (subDivisionPoints[0] - subDivisionPoints[1]).normalized
                * (-distance);
        }
        
        //if distance is to great, return a proportional distance in the direction of the last segment
        if (distance >= totalDistance)
        {
            return subDivisionPoints[subDivisionPoints.Length - 1]
                + (subDivisionPoints[subDivisionPoints.Length - 1] - subDivisionPoints[subDivisionPoints.Length - 2]).normalized 
                * (distance-totalDistance);
        }
        

        // lets find the first point that is below the distance
        // but, who's next point is above the distance
        var index = 0;
        while (index < lengths.Length - 1 && lengths[index + 1] < distance)
            index++;

        // get the percentage of travel from the current length to the next
        // where the distance is.
        var amount = Mathf.InverseLerp(lengths[index], lengths[index + 1], distance);
        // we use that, to get the actual point
        return Vector3.Lerp(subDivisionPoints[index], subDivisionPoints[index + 1], amount);
    }
}