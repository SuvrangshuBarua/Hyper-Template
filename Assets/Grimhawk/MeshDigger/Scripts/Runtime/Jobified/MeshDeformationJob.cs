using System.Collections;
using System.Collections.Generic;
using Unity.Collections;
using Unity.Jobs;
using UnityEngine;

public struct MeshDeformationJob : IJobParallelFor
{
    [ReadOnly]
    public readonly float deformationRadius;
    [ReadOnly]
    public readonly float deformationStrength;
    [ReadOnly]
    public readonly Vector3 raycastOrigin;

    [ReadOnly]
    public readonly NativeArray<Vector3> vertices;
    public NativeArray<Vector3> updatedVertices;

    public MeshDeformationJob(float deformationRadius, float deformationStrength, Vector3 raycastOrigin, NativeArray<Vector3> vertices, NativeArray<Vector3> updatedVertices)
    {
        this.deformationRadius = deformationRadius;
        this.deformationStrength = deformationStrength;
        this.raycastOrigin = raycastOrigin;
        this.vertices = vertices;
        this.updatedVertices = updatedVertices;
    }

    public void Execute(int index)
    {
        Vector3 direction = vertices[index] - raycastOrigin;
        float distance = direction.sqrMagnitude;

        if(distance <= deformationRadius)
        {
            float displacement = (1 - distance / deformationRadius) * deformationStrength;
            updatedVertices[index] = vertices[index] + direction.normalized * displacement;
        }
        else
        {
            updatedVertices[index] = vertices[index];
        }
    }
}
