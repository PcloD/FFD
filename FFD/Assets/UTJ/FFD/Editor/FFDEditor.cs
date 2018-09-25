using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;


namespace UTJ.FFD
{
    [CustomEditor(typeof(FFD))]
    public class FFDEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            DrawDefaultInspector();
        }
    }
}
