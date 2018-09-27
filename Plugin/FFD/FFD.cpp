#include "pch.h"
#include "FFD.h"

struct ffdMeshData
{
    int         *indices = nullptr;
    float3      *vertices = nullptr;
    float3      *normals = nullptr;
    float4      *tangents = nullptr;
    float2      *uv = nullptr;
    int         num_vertices = 0;
    int         num_triangles = 0;
    float4x4    transform = float4x4::identity();
};

struct ffdSkinData
{
    Weights4    *weights = nullptr;
    float4x4    *bones = nullptr;
    float4x4    *bindposes = nullptr;
    int         num_vertices = 0;
    int         num_bones = 0;
    float4x4    root = float4x4::identity();
};

struct ffdLatticeData
{
    float3 *points = nullptr;
    int div_s = 2;
    int div_t = 2;
    int div_u = 2;
    float4x4 transform = float4x4::identity();
};

struct ffdLatticeWeightsData
{
    Weights8 *weights = nullptr;
    int num_vertices = 0;
};


template<int NumInfluence>
static void SkinningImpl(
    int num_vertices, const RawVector<float4x4>& poses, const Weights<NumInfluence> weights[],
    const float3 ipoints[], const float3 inormals[], const float4 itangents[],
    float3 opoints[], float3 onormals[], float4 otangents[])
{
    parallel_invoke(
        [&]() {
        if (ipoints && opoints) {
            for (int vi = 0; vi < num_vertices; ++vi) {
                const auto& w = weights[vi];
                float3 p = ipoints[vi];
                float3 rp = float3::zero();
                for (int bi = 0; bi < NumInfluence; ++bi) {
                    rp += mul_p(poses[w.indices[bi]], p) * w.weights[bi];
                }
                opoints[vi] = rp;
            }
        }
    },
        [&]() {
        if (inormals && onormals) {
            for (int vi = 0; vi < num_vertices; ++vi) {
                const auto& w = weights[vi];
                float3 n = inormals[vi];
                float3 rn = float3::zero();
                for (int bi = 0; bi < NumInfluence; ++bi) {
                    rn += mul_v(poses[w.indices[bi]], n) * w.weights[bi];
                }
                onormals[vi] = normalize(rn);
            }
        }
    },
        [&]() {
        if (itangents && otangents) {
            for (int vi = 0; vi < num_vertices; ++vi) {
                const auto& w = weights[vi];
                float4 t = itangents[vi];
                float4 rt = float4::zero();
                for (int bi = 0; bi < NumInfluence; ++bi) {
                    rt += mul_v(poses[w.indices[bi]], t) * w.weights[bi];
                }
                otangents[vi] = rt;
            }
        }
    });
}

ffdAPI void ffdApplySkinning(
    ffdSkinData *skin,
    const float3 ipoints[], const float3 inormals[], const float4 itangents[],
    float3 opoints[], float3 onormals[], float4 otangents[])
{
    RawVector<float4x4> poses;
    poses.resize(skin->num_bones);

    auto iroot = invert(skin->root);
    for (int bi = 0; bi < skin->num_bones; ++bi) {
        poses[bi] = skin->bindposes[bi] * skin->bones[bi] * iroot;
    }
    SkinningImpl(skin->num_vertices, poses, skin->weights, ipoints, inormals, itangents, opoints, onormals, otangents);
}

ffdAPI void ffdApplyReverseSkinning(
    ffdSkinData *skin,
    const float3 ipoints[], const float3 inormals[], const float4 itangents[],
    float3 opoints[], float3 onormals[], float4 otangents[])
{
    RawVector<float4x4> poses;
    poses.resize(skin->num_bones);

    auto iroot = invert(skin->root);
    for (int bi = 0; bi < skin->num_bones; ++bi) {
        poses[bi] = invert(skin->bindposes[bi] * skin->bones[bi] * iroot);
    }
    SkinningImpl(skin->num_vertices, poses, skin->weights, ipoints, inormals, itangents, opoints, onormals, otangents);
}


ffdAPI void ffdLatticeReset(ffdLatticeData *lattice)
{
    // todo
}

ffdAPI void ffdLatticeSetup(ffdLatticeData *lattice, ffdLatticeWeightsData *weights, const float3 points[])
{
    // todo
}

ffdAPI void ffdLatticeApplyDeform(ffdLatticeData *lattice, ffdLatticeWeightsData *weights,
    const float3 ipoints[], const float3 inormals[], const float4 itangents[],
    float3 opoints[], float3 onormals[], float4 otangents[])
{
    // todo
}
