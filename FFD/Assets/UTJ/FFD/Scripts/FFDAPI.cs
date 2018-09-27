using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;

namespace UTJ.FFD
{
    public static class FFDAPI
    {
        public struct ffdWeights8
        {
            public float w0, w1, w2, w3, w4, w5, w6, w7;
            public int i0, i1, i2, i3, i4, i5, i6, i7;
        }

        public struct ffdMeshData
        {
            public IntPtr indices;  // int*
            public IntPtr vertices; // Vectror3*
            public IntPtr normals;  // Vector3*
            public IntPtr tangents; // Vector4*
            public IntPtr uv;       // Vector2*
            public int num_vertices;
            public int num_triangles;
            public Matrix4x4 transform;
        }

        public struct ffdSkinData
        {
            public IntPtr weights;   // BoneWeight*
            public IntPtr bones;     // Matrix4x4*
            public IntPtr bindposes; // Matrix4x4*
            public int num_vertices;
            public int num_bones;
            public Matrix4x4 root;
        };

        public struct ffdLatticeData
        {
            public IntPtr points; // Vectror3*
            public int div_s;
            public int div_t;
            public int div_u;
            public Matrix4x4 transform;
        }

        public struct ffdLatticeWeightsData
        {
            public IntPtr weights; // ffdWeights8*
            public int num_vertices;
        }

        [DllImport("FFD")] public static extern void ffdApplySkinning(ref ffdSkinData skin,
            IntPtr ipoints, IntPtr inormals, IntPtr itangents,
            IntPtr opoints, IntPtr onormals, IntPtr otangents);
        [DllImport("FFD")] public static extern void ffdApplyReverseSkinning(ref ffdSkinData skin,
            IntPtr ipoints, IntPtr inormals, IntPtr itangents,
            IntPtr opoints, IntPtr onormals, IntPtr otangents);

        [DllImport("FFD")] public static extern void ffdLatticeReset(ref ffdLatticeData lattice);
        [DllImport("FFD")] public static extern void ffdLatticeSetup(ref ffdLatticeData lattice, ref ffdLatticeWeightsData weights, IntPtr points);
        [DllImport("FFD")] public static extern void ffdLatticeApplyDeform(ref ffdLatticeData lattice, ref ffdLatticeWeightsData weights,
            IntPtr ipoints, IntPtr inormals, IntPtr itangents,
            IntPtr opoints, IntPtr onormals, IntPtr otangents);

        // utrils
        public static void SafeDispose<T>(ref PinnedList<T> v) where T : struct
        {
            if (v != null)
            {
                v.Dispose();
                v = null;
            }
        }

    }
}
