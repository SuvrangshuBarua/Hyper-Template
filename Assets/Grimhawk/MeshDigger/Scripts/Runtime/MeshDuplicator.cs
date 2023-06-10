using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteAlways]
public class MeshDuplicator : MonoBehaviour
{
    private Mesh _originalMesh;
    private Mesh _clonedMesh;
    [HideInInspector] public MeshFilter meshFilter;
    [HideInInspector] public MeshCollider meshCollider;
    [HideInInspector] public Vector3[] vertices;
    [SerializeField] private int[] _triangles;

    private void Start()
    {
        meshFilter = GetComponent<MeshFilter>();
        meshCollider= GetComponent<MeshCollider>();
        _originalMesh = meshFilter.sharedMesh;

        vertices = new Vector3[_originalMesh.vertices.Length];
        _triangles = new int[_originalMesh.triangles.Length];

        _clonedMesh = new Mesh();

        _clonedMesh.name = "Clone";
        _clonedMesh.vertices= _originalMesh.vertices;
        _clonedMesh.triangles= _originalMesh.triangles;
        _clonedMesh.normals = _originalMesh.normals;
        _clonedMesh.uv = _originalMesh.uv;

        meshFilter.mesh = _clonedMesh;

        vertices = _clonedMesh.vertices;
        _triangles = _clonedMesh.triangles;


    }
}
