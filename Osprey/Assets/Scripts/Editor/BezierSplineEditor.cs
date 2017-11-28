﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System;

[CustomEditor(typeof(BezierSpline))]
public class BezierSplineEditor : Editor {


    private BezierSpline spline;
    private Transform handleTransform;
    private Quaternion handleRotation;

    //curve granularity, # of steps per curve
    //private const int lineSteps = 16;
    private const float directionScale = 2f;

    private void OnSceneGUI()
    {
        spline = target as BezierSpline;
        handleTransform = spline.transform;
        handleRotation = Tools.pivotRotation == PivotRotation.Local ?
            handleTransform.rotation : Quaternion.identity;

        Vector3 p0 = ShowPoint(0);
        for (int i = 1; i < spline.ControlPointCount; i += 3)
        {
            Vector3 p1 = ShowPoint(i);
            Vector3 p2 = ShowPoint(i + 1);
            Vector3 p3 = ShowPoint(i + 2);

            Handles.color = Color.gray;
            Handles.DrawLine(p0, p1);
            Handles.DrawLine(p2, p3);

            Handles.DrawBezier(p0, p3, p1, p2, Color.white, null, 2f);
            p0 = p3;
        }
        ShowDirections();
    }

    private const int stepsPerCurve = 10;

    private void ShowDirections()
    {
        Handles.color = Color.green;
        Vector3 point = spline.GetLerpPoint(0f);
        Handles.DrawLine(point, point + spline.GetDirection(0f) * directionScale);
        int steps = stepsPerCurve * spline.CurveCount;
        for (int i = 1; i <= steps; i++)
        {
            point = spline.GetLerpPoint(i / (float)steps);
            Handles.DrawLine(point, point + spline.GetDirection(i / (float)steps) * directionScale);
        }
    }

    private const float handleSize = 0.04f;
    private const float pickSize = 0.06f;

    private int selectedIndex = -1;

    private static Color[] modeColors =
    {
        Color.white,
        Color.yellow,
        Color.cyan
    };

    private Vector3 ShowPoint(int index)
    {
        Vector3 point = handleTransform.TransformPoint(spline.GetControlPoint(index));
        float size = HandleUtility.GetHandleSize(point);
        if(index == 0)
        {
            size *= 2f;
        }

        Handles.color = modeColors[(int)spline.GetControlPointMode(index)] ;
        if (Handles.Button(point, handleRotation, size * handleSize, size * pickSize, Handles.DotHandleCap))
        {
            selectedIndex = index;
            Repaint();
        }
        if (selectedIndex == index)
        {
            EditorGUI.BeginChangeCheck();
            point = Handles.DoPositionHandle(point, handleRotation);
            if (EditorGUI.EndChangeCheck())
            {
                Undo.RecordObject(spline, "Move Point");
                EditorUtility.SetDirty(spline);
                spline.SetControlPoint(index, handleTransform.InverseTransformPoint(point));
            }
        }
        return point;
    }
    public override void OnInspectorGUI()
    {
        //DrawDefaultInspector();
        spline = target as BezierSpline;

        EditorGUI.BeginChangeCheck();
        bool loop = EditorGUILayout.Toggle("Loop", spline.ClosedLoop);
        if (EditorGUI.EndChangeCheck())
        {
            Undo.RecordObject(spline, "Toggle Loop");
            EditorUtility.SetDirty(spline);
            spline.ClosedLoop = loop;
        }

        EditorGUI.BeginChangeCheck();
        bool useLineRenderer = EditorGUILayout.Toggle("Use Line Renderer", spline.useLineRenderer);
        if (EditorGUI.EndChangeCheck())
        {
            Undo.RecordObject(spline, "Toggle LineRenderer");
            EditorUtility.SetDirty(spline);
            spline.useLineRenderer = useLineRenderer;
        }

        EditorGUI.BeginChangeCheck();
        bool showGizmos = EditorGUILayout.Toggle("Toggle Spline Gizoms", spline.showGizmos);
        if (EditorGUI.EndChangeCheck())
        {
            Undo.RecordObject(spline, "Toggle Spline Gizmos");
            EditorUtility.SetDirty(spline);
            spline.showGizmos = showGizmos;
        }

        if (selectedIndex >= 0 && selectedIndex < spline.ControlPointCount)
        {
            DrawSelectedPointInspector();
            using (new EditorGUI.DisabledScope())
            {
                EditorGUILayout.IntField("Curve Index", (selectedIndex+1)/3);
                EditorGUILayout.FloatField("Distance at Point", spline.GetLenthAt((selectedIndex+1)/3* BezierSpline.linesPerCurve));
            }
        }

        using(new EditorGUI.DisabledScope())
        {
            EditorGUILayout.FloatField("Spline Length", spline.GetTotalLength());
        }

        if (GUILayout.Button("Add Curve"))
        {
            Undo.RecordObject(spline, "Add Curve");
            spline.AddCurve();
            EditorUtility.SetDirty(spline);
        }

        //if (selectedIndex >= 0 && selectedIndex < spline.ControlPointCount && spline.CurveCount > 1)
        if (spline.CurveCount > 1)
        {
            if (GUILayout.Button("Remove Curve")) 
            {
                Undo.RecordObject(spline, "Remove Curve");
                spline.RemoveCurve();
                EditorUtility.SetDirty(spline);
            }
        }
        Repaint();
    }

    private void DrawSelectedPointInspector()
    {
        GUILayout.Label("Selected Point");
        EditorGUI.BeginChangeCheck();
        Vector3 point = EditorGUILayout.Vector3Field("Position", spline.GetControlPoint(selectedIndex));
        if(EditorGUI.EndChangeCheck())
        {
            Undo.RecordObject(spline, "Move Point");
            EditorUtility.SetDirty(spline);
            spline.SetControlPoint(selectedIndex, point);
        }

        EditorGUI.BeginChangeCheck();
        BezierControlPointMode mode = (BezierControlPointMode)
            EditorGUILayout.EnumPopup("Mode", spline.GetControlPointMode(selectedIndex));
        if (EditorGUI.EndChangeCheck())
        {
            Undo.RecordObject(spline, "Change Point Mode");
            spline.SetControlPointMode(selectedIndex, mode);
            EditorUtility.SetDirty(spline);
        }
    }

   
}
