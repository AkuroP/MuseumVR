﻿using UnityEngine;
using System.Collections;
using UnityEditor;

#if UNITY_EDITOR
[CustomEditor(typeof(CrystalCutter)), CanEditMultipleObjects]
public class CrystalCutterEditor : Editor 
{
    public override void OnInspectorGUI()
    {
        CrystalCutter myTarget = (CrystalCutter)target;

        DrawDefaultInspector();

        if(GUILayout.Button("DoPlaneCut"))
        {
            myTarget.DoPlaneCut();
        }
      
    }
}
#endif