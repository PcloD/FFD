using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace UTJ.FFD
{
    [ExecuteInEditMode]
    public class FFDLattice : MonoBehaviour
    {
#if UNITY_EDITOR
        [MenuItem("GameObject/Deformer/FFD Lattice", false, 10)]
        public static void CreateFFDLattice(MenuCommand menuCommand)
        {
            var go = new GameObject();
            go.name = "FFDLattice";
            go.AddComponent<FFDLattice>();
            Undo.RegisterCreatedObjectUndo(go, "FFDLattice");
        }
#endif

        [SerializeField] int m_s = 2;
        [SerializeField] int m_t = 2;
        [SerializeField] int m_u = 2;
        [SerializeField] Vector3[] m_points;
    }
}
