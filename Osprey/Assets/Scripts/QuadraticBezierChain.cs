using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
    public int subdivisionsPerSection;
    public int totalSubdivisions;
    public QuadraticBezierPoints[] bezierChain;

    //there is a hidden line renderer thing here

    public bool useLineRenderer;

    public bool stayWithTransform;
    public bool useTransformScale;


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

        //draw
        for (int i = 1; i < subDivisionPoints.Length; i++)
        {
            Gizmos.DrawLine(subDivisionPoints[i - 1], subDivisionPoints[i]);
        }

        for(int i = 0; i < bezierChain.Length; i++)
        {
            Gizmos.DrawSphere(bezierChain[i].p0, 0.2f);
            Gizmos.DrawSphere(bezierChain[i].p1, 0.2f);
            Gizmos.DrawSphere(bezierChain[i].p2, 0.2f);
        }
    }

    // Update is called once per frame
    void Update()
    {
        CheckRecalculate();
    }

    private void CheckRecalculate()
    {
        if (subDivisionPoints == null)
        {
            //init to 16, user can change this later
            subdivisionsPerSection = 16;
            oneTimeRecalculate = true;
        }

        if (useLineRenderer && lineRenderer == null)
        {
            GetLineRenderer();
            oneTimeRecalculate = true;
        }

        if (bezierChain == null)
        {
            bezierChain = new QuadraticBezierPoints[1];
            bezierChain[0] = new QuadraticBezierPoints(-2 * Vector3.right, 2 * Vector3.up, 2 * Vector3.right);
            oneTimeRecalculate = true;
        }

        if (oneTimeRecalculate || continualRecalculate)
        {
            oneTimeRecalculate = false;
            RecalculateSubdivisions();
        }
    }

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

    private void ApplySubdivisionBoundary()
    {
        Mathf.Clamp(subdivisionsPerSection, 1, 100);
    }

    private void RecalculateSubdivisions()
    {
        ApplySubdivisionBoundary();

        totalSubdivisions = subdivisionsPerSection * bezierChain.Length;
        int subDivisionLength = totalSubdivisions + bezierChain.Length;

        subDivisionPoints = new Vector3[subDivisionLength];

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
            return Vector3.zero;
        }
        
    }
}