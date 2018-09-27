using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;

namespace UTJ.FFD
{
    [ExecuteInEditMode]
    [AddComponentMenu("Deformer/FFD")]
    public class FFD : MonoBehaviour
    {
        [SerializeField] FFDLattice m_lattice;
        [SerializeField] Mesh m_baseMesh;
        Mesh m_editMesh;
        bool m_skinned = false;

        PinnedList<Vector3> m_points, m_pointsBase;
        PinnedList<Vector3> m_normals, m_normalsBase;
        PinnedList<Vector4> m_tangents, m_tangentsBase;
        PinnedList<FFDAPI.ffdWeights8> m_weights;


        void ReleaseResources()
        {
            FFDAPI.SafeDispose(ref m_points); FFDAPI.SafeDispose(ref m_pointsBase);
            FFDAPI.SafeDispose(ref m_normals); FFDAPI.SafeDispose(ref m_normalsBase);
            FFDAPI.SafeDispose(ref m_tangents); FFDAPI.SafeDispose(ref m_tangentsBase);
            FFDAPI.SafeDispose(ref m_weights);
        }

        Mesh GetTargetMesh()
        {
            var smr = GetComponent<SkinnedMeshRenderer>();
            if (smr) { return smr.sharedMesh; }

            var mf = GetComponent<MeshFilter>();
            if (mf) { return mf.sharedMesh; }

            return null;
        }

        void SetTargetMesh(Mesh mesh)
        {
            var smr = GetComponent<SkinnedMeshRenderer>();
            if (smr) { smr.sharedMesh = mesh; }

            var mf = GetComponent<MeshFilter>();
            if (mf) { mf.sharedMesh = mesh; }
        }

        bool SetupMeshData()
        {
            var tmesh = GetTargetMesh();
            if (tmesh == null)
            {
                Debug.LogWarning("Target mesh is null.");
                return false;
            }
            else if (!tmesh.isReadable)
            {
                Debug.LogWarning("Target mesh is not readable.");
                return false;
            }

            var smr = GetComponent<SkinnedMeshRenderer>();
            if (smr != null && smr.bones.Length > 0)
                m_skinned = true;

            m_baseMesh = tmesh;
            m_editMesh = Instantiate(m_baseMesh);
            SetTargetMesh(m_editMesh);

            // allocate vertex buffers
            ReleaseResources();
            m_pointsBase = new PinnedList<Vector3>(m_baseMesh.vertices);
            m_normalsBase = new PinnedList<Vector3>(m_baseMesh.normals);
            m_tangentsBase = new PinnedList<Vector4>(m_baseMesh.tangents);
            m_points = m_pointsBase;
            m_normals = m_normalsBase;
            m_tangents = m_tangentsBase;

            return true;
        }

        bool SetupLatticeWeights()
        {
            FFDAPI.SafeDispose(ref m_weights);
            m_weights = new PinnedList<FFDAPI.ffdWeights8>(m_pointsBase.Count);

            FFDAPI.ffdLatticeData lattice = default(FFDAPI.ffdLatticeData);
            FFDAPI.ffdLatticeWeightsData weights = default(FFDAPI.ffdLatticeWeightsData);
            m_lattice.GetLatticeData(ref lattice);
            weights.weights = m_weights;
            weights.num_vertices = m_weights.Count;
            FFDAPI.ffdLatticeSetup(ref lattice, ref weights, m_points);
            return true;
        }

        bool ApplyDeform()
        {
            if (m_baseMesh == null || m_editMesh == null || m_baseMesh.vertexCount != m_editMesh.vertexCount)
            {
                if (!SetupMeshData())
                    return false;
                if (!SetupLatticeWeights())
                    return false;
            }
            return true;
        }


        void OnDestroy()
        {
            ReleaseResources();
        }
    }
}
