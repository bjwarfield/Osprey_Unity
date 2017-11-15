using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(QuadraticBezierChain))]
public class QuadBezierChainEditor : Editor {

    private QuadraticBezierChain qbc;
    private QuadraticBezierPoints[] cachedChain;

    private void OnEnable()
    {
        qbc = (QuadraticBezierChain)target;
        ResetPointCache();

    }

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
    }

    private void OnSceneGUI()
    {
        for(int i = 0; i < qbc.bezierChain.Length; i++)
        {

            GUIStyle _VertStyle = new GUIStyle(GUI.skin.FindStyle("ProfilerPaneSubLabel"));
            _VertStyle.fontSize = 8;
            _VertStyle.name = "VertLabel";
            _VertStyle.normal.textColor = Color.gray;

            //this is all kinds of broken
            //if (qbc.stayWithTransform)
            //{
            //    Handles.color = Color.cyan;
            //    float handleSize = 0.2f;

            //    qbc.bezierChain[i].p0 = Handles.FreeMoveHandle(qbc.GetSubDivisonPoint(i * qbc.subdivisionsPerSection), Quaternion.identity, handleSize, Vector3.zero, Handles.DotHandleCap) - qbc.transform.position;
            //    Handles.Label(qbc.bezierChain[i].p0, "P0");

            //    qbc.bezierChain[i].p1 =  Quaternion.Inverse(qbc.transform.rotation) * (Handles.FreeMoveHandle(qbc.transform.rotation * qbc.bezierChain[i].p1 + qbc.transform.position, Quaternion.identity, handleSize, Vector3.zero, Handles.DotHandleCap) - qbc.transform.position);
            //    Handles.Label(qbc.bezierChain[i].p1, "P1");

            //    qbc.bezierChain[i].p2 = Handles.FreeMoveHandle(qbc.GetSubDivisonPoint(i * qbc.subdivisionsPerSection + qbc.subdivisionsPerSection), Quaternion.identity, handleSize, Vector3.zero, Handles.DotHandleCap) - qbc.transform.position;
            //    Handles.Label(qbc.bezierChain[i].p2, "P2");

            //    Handles.color = Color.gray;

            //    Handles.DrawLine(qbc.bezierChain[i].p0, qbc.bezierChain[i].p1);
            //    Handles.DrawLine(qbc.bezierChain[i].p1, qbc.bezierChain[i].p2);
            //}
            //else

            {
                Handles.color = Color.magenta;
                float handleSize = 0.2f;
              
                qbc.bezierChain[i].p0 = Handles.FreeMoveHandle(qbc.bezierChain[i].p0, Quaternion.identity, handleSize, Vector3.zero, Handles.DotHandleCap);
                Handles.Label(qbc.bezierChain[i].p0, "P0");
                Handles.color = Color.cyan;
                qbc.bezierChain[i].p1 = Handles.FreeMoveHandle(qbc.bezierChain[i].p1, Quaternion.identity, handleSize, Vector3.zero, Handles.DotHandleCap);
                Handles.Label(qbc.bezierChain[i].p1, "P1");
                Handles.color = Color.magenta;
                qbc.bezierChain[i].p2 = Handles.FreeMoveHandle(qbc.bezierChain[i].p2, Quaternion.identity, handleSize, Vector3.zero, Handles.DotHandleCap);
                Handles.Label(qbc.bezierChain[i].p2, "P2");

                Handles.color = Color.gray;

                Handles.DrawLine(qbc.bezierChain[i].p0, qbc.bezierChain[i].p1);
                Handles.DrawLine(qbc.bezierChain[i].p1, qbc.bezierChain[i].p2);
                    
            }
            
            if (qbc.bezierChain[i] != cachedChain[i])
            {
                SmoothPoints(i, qbc.bezierChain[i].p1 == cachedChain[i].p1);
                break;
            }

        }
    }

    

    private void SmoothPoints(int index, bool endPoints)
    {
        //match endpoints and pulls of neighboring links
        if (endPoints)
        {
            
            if(index < qbc.bezierChain.Length - 1)
            {
                qbc.bezierChain[index + 1].p0 = qbc.bezierChain[index].p2;
                //qbc.bezierChain[index + 1].p1 -= (cachedChain[index + 1].p0 - qbc.bezierChain[index + 1].p0);
            }
            if(index > 0)
            {
                qbc.bezierChain[index - 1].p2 = qbc.bezierChain[index].p0;
                //qbc.bezierChain[index - 1].p1 -= (cachedChain[index - 1].p2 - qbc.bezierChain[index - 1].p2);
            }

            //qbc.bezierChain[index].p1 -= (cachedChain[index].p0 - qbc.bezierChain[index].p0) + (cachedChain[index].p2 - qbc.bezierChain[index].p2);

        }

        //align pulls
        for (int i = index; i > 0; i--)
        {
            qbc.bezierChain[i - 1].p1 = qbc.bezierChain[i].p0 + (qbc.bezierChain[i].p0 - qbc.bezierChain[i].p1).normalized * Vector3.Distance(qbc.bezierChain[i - 1].p1, qbc.bezierChain[i - 1].p2);
        }
        for (int i = index; i < qbc.bezierChain.Length - 1; i++)
        {
            qbc.bezierChain[i + 1].p1 = qbc.bezierChain[i].p2 + (qbc.bezierChain[i].p2 - qbc.bezierChain[i].p1).normalized * Vector3.Distance(qbc.bezierChain[i + 1].p1, qbc.bezierChain[i + 1].p0);
        }


        ResetPointCache();
    }

    void ResetPointCache()
    {
        cachedChain = new QuadraticBezierPoints[qbc.bezierChain.Length];
        for (int i = 0; i < qbc.bezierChain.Length; i++)
        {
            qbc.bezierChain[i].p0.z = 0;
            qbc.bezierChain[i].p1.z = 0;
            qbc.bezierChain[i].p2.z = 0;
            cachedChain[i] = new QuadraticBezierPoints(qbc.bezierChain[i]);
        }
    }

}
