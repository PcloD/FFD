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

        PinnedList<Vector3> m_points;
        PinnedList<Vector3> m_normals;
        PinnedList<Vector3> m_tangents;
    }
}
