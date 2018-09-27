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
        [SerializeField] int m_s = 2;
        [SerializeField] int m_t = 2;
        [SerializeField] int m_u = 2;
        [SerializeField] Vector3[] m_points;
        PinnedList<Vector3> m_pinnedPoints;

#if UNITY_EDITOR
        [MenuItem("GameObject/Deformer/FFD Lattice", false, 10)]
        public static void CreateFFDLattice(MenuCommand menuCommand)
        {
            var go = new GameObject();
            go.name = "FFDLattice";
            go.AddComponent<FFDLattice>();
            Undo.RegisterCreatedObjectUndo(go, "FFDLattice");
        }

        void OnValidate()
        {
            ValidateLattice();
        }
#endif

        void ValidateLattice()
        {
            m_s = Mathf.Clamp(m_s, 2, 32);
            m_t = Mathf.Clamp(m_t, 2, 32);
            m_u = Mathf.Clamp(m_u, 2, 32);

            int numPoints = m_s * m_t * m_u;
            if (m_points == null || m_points.Length != numPoints)
            {
                m_points = new Vector3[numPoints];

                var size = GetComponent<Transform>().localScale;

                for (int z = 0; z < m_u; ++z)
                {
                    for (int y = 0; y < m_t; ++y)
                    {
                        for (int x = 0; x < m_s; ++x)
                        {

                        }
                    }
                }

                FFDAPI.SafeDispose(ref m_pinnedPoints);
                m_pinnedPoints = new PinnedList<Vector3>(m_points);
            }
        }

        public void GetLatticeData(ref FFDAPI.ffdLatticeData v)
        {
            v.points = m_pinnedPoints;
            v.div_s = m_s;
            v.div_t = m_t;
            v.div_u = m_u;
            v.transform = GetComponent<Transform>().localToWorldMatrix;
        }

        void OnDestroy()
        {
            if (m_pinnedPoints != null)
                m_pinnedPoints.Dispose();
        }
    }
}
