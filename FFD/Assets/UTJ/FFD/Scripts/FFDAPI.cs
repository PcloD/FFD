using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;

namespace UTJ.FFD
{
    public static class FFDAPI
    {
        public struct ffdMeshData
        {
            public IntPtr indices;
            public IntPtr vertices;
            public IntPtr normals;
            public IntPtr tangents;
            public IntPtr uv;
            public int num_vertices;
            public int num_triangles;
            public Matrix4x4 transform;
        }

        public struct ffdSkinData
        {
            public IntPtr weights;
            public IntPtr bones;
            public IntPtr bindposes;
            public int num_vertices;
            public int num_bones;
            public Matrix4x4 root;
        };

        public struct ffdLatticeData
        {
            public IntPtr weights;
            public IntPtr lattice_points;
            public IntPtr lattice_base_points;
            public int num_vertices;
            public int div_s;
            public int div_t;
            public int div_u;
            public Matrix4x4 root;
        }

        [DllImport("FFD")] public static extern void ffdApplySkinning(ref ffdSkinData skin,
            IntPtr ipoints, IntPtr inormals, IntPtr itangents,
            IntPtr opoints, IntPtr onormals, IntPtr otangents);
        [DllImport("FFD")] public static extern void ffdApplyReverseSkinning(ref ffdSkinData skin,
            IntPtr ipoints, IntPtr inormals, IntPtr itangents,
            IntPtr opoints, IntPtr onormals, IntPtr otangents);

        [DllImport("FFD")] public static extern void ffdLatticeReset(ref ffdLatticeData lattice);
        [DllImport("FFD")] public static extern void ffdLatticeSetup(ref ffdLatticeData lattice, IntPtr points);
        [DllImport("FFD")] public static extern void ffdLatticeApplyDeform(ref ffdLatticeData lattice,
            IntPtr ipoints, IntPtr inormals, IntPtr itangents,
            IntPtr opoints, IntPtr onormals, IntPtr otangents);
    }
}
