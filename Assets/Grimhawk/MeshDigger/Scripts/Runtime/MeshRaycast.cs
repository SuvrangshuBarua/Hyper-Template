using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeshRaycast : MonoBehaviour
{
    private RaycastHit hit;
    private Transform _transform;
    private Vector3[] updatedVertices;
    public MeshDuplicator meshDuplicator;
    public float deformationRadius = 1f;
    public float deformationStrength = 0.1f;
    private void Start()
    {
        _transform = transform;
        updatedVertices = new Vector3[meshDuplicator.vertices.Length];  
        updatedVertices = meshDuplicator.vertices;
    }
    private void FixedUpdate()
    {
        if(Physics.Raycast(_transform.position, -transform.up, out hit, 100f, 1 << 8))
        {
            Debug.DrawRay(_transform.position, -_transform.up * Vector3.Distance(_transform.position, hit.point), Color.red);
            for (int i = 0; i < meshDuplicator.vertices.Length; i++)
            {
                Vector3 direction = meshDuplicator.vertices[i] - _transform.position;
                float distance = direction.sqrMagnitude;   

                if(distance <= deformationRadius)
                {
                    float displacement = (1 - distance / deformationRadius) * deformationStrength;
                    updatedVertices[i] = meshDuplicator.vertices[i] + direction.normalized * displacement;  
                }
                else
                    updatedVertices[i] = meshDuplicator.vertices[i];
            }
            meshDuplicator.meshFilter.mesh.vertices = updatedVertices;
            meshDuplicator.meshFilter.mesh.RecalculateBounds();
            meshDuplicator.meshFilter.mesh.RecalculateNormals();
            meshDuplicator.meshCollider.sharedMesh = meshDuplicator.meshFilter.mesh;
        }
        
    }
#if UNITY_EDITOR
    private void OnDrawGizmos()
    {
        
    }
#endif
}
