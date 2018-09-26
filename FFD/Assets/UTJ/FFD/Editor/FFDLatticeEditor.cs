using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;


namespace UTJ.FFD
{
    [CustomEditor(typeof(FFDLattice))]
    public class FFDLatticeEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            DrawDefaultInspector();
        }
    }
}
