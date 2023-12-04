
using Unity.Collections;
using Unity.Jobs;
using UnityEngine;


public class MeshDeformation : MonoBehaviour
{
    public float deformationRadius = 1.0f;
    public float deformationStrength = 0.1f;
    public MeshDuplicator meshInfo;

    private Vector3 _origin;
    private JobHandle _jobHandle;
    private MeshDeformationJob _job;
    private NativeArray<Vector3> _verticesArray;
    private NativeArray<Vector3> _updatedVerticesArray;

    private void Start()
    {
        if (meshInfo == null)
            return;
        
        _verticesArray = new NativeArray<Vector3>(meshInfo.vertices , Allocator.Persistent);
        _updatedVerticesArray = new NativeArray<Vector3>(meshInfo.vertices.Length, Allocator.Persistent);    
    }
    private void OnDestroy()
    {
        _verticesArray.Dispose();
        _updatedVerticesArray.Dispose();    
    }
    private void FixedUpdate()
    {
        if(meshInfo == null) return;

        _origin = transform.position;

        RaycastHit hit;
        
        if (Physics.Raycast(_origin, -transform.up, out hit, 100f, 1 << 8))
        {
            Debug.DrawRay(_origin, -transform.up * Vector3.Distance(transform.position, hit.point), Color.red);

            ScheduleJob();
        }
    }

    private void ScheduleJob()
    {
        _job = new MeshDeformationJob(deformationRadius, deformationStrength, _origin, _verticesArray, _updatedVerticesArray);


        _jobHandle = _job.Schedule(_verticesArray.Length, 64);

        _jobHandle.Complete();

        meshInfo.meshFilter.mesh.vertices = _updatedVerticesArray.ToArray();
        meshInfo.meshFilter.mesh.RecalculateBounds();
        meshInfo.meshFilter.mesh.RecalculateNormals();
        meshInfo.meshCollider.sharedMesh = meshInfo.meshFilter.mesh;
    }
    private void CompleteJob()
    {

    }
}
