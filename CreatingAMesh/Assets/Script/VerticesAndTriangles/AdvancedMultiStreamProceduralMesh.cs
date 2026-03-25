namespace VerticesAndTriangles
{
    using Unity.Collections;
    using Unity.Mathematics;
    using static Unity.Mathematics.math;
    using UnityEngine;
    using UnityEngine.Rendering;
    [RequireComponent(typeof(MeshFilter), typeof(MeshRenderer))]
    public class AdvancedMultiStreamProceduralMesh : MonoBehaviour
    {
        private void OnEnable()
        {
            int vertexAttributeCount = 4;
            int vertexCount = 4;
            int triangleIndexCount = 6;

            Mesh.MeshDataArray meshDataArray = Mesh.AllocateWritableMeshData(1);
            Mesh.MeshData meshData = meshDataArray[0];

            var vertexAttributes = new NativeArray<VertexAttributeDescriptor>(vertexAttributeCount, Allocator.Temp, NativeArrayOptions.UninitializedMemory);

            vertexAttributes[0] = new VertexAttributeDescriptor(dimension:3);
            vertexAttributes[1] = new VertexAttributeDescriptor(VertexAttribute.Normal, dimension:3, stream:1);
            vertexAttributes[2] = new VertexAttributeDescriptor(VertexAttribute.Tangent, VertexAttributeFormat.Float16, dimension:4, stream:2);
            vertexAttributes[3] = new VertexAttributeDescriptor(VertexAttribute.TexCoord0, VertexAttributeFormat.Float16, dimension:2, stream:3);

            meshData.SetVertexBufferParams(vertexCount, vertexAttributes);
            vertexAttributes.Dispose();

            NativeArray<float3> position = meshData.GetVertexData<float3>();
            position[0] = 0f;
            position[1] = right();
            position[2] = up();
            position[3] = float3(1f, 1f, 0f);

            NativeArray<float3> normal = meshData.GetVertexData<float3>(1);
            normal[0] = normal[1] = normal[2] = normal[3] = back();
            
            var h0 = half(0f);
            var h1 = half(1f);
            
            NativeArray<half4> tangent = meshData.GetVertexData<half4>(2);
            tangent[0] = tangent[1] = tangent[2] = tangent[3] = new half4(h1, h0, h0, half(-1f));
            
            NativeArray<half2> texCoord = meshData.GetVertexData<half2>(3);
            texCoord[0] = h0;
            texCoord[1] = half2(h1, h0);
            texCoord[2] = half2(h0, h1);
            texCoord[3] = h1; 
            
            meshData.SetIndexBufferParams(triangleIndexCount, IndexFormat.UInt16);
            NativeArray<ushort> triangleIndices = meshData.GetIndexData<ushort>();
            triangleIndices[0] = 0;
            triangleIndices[1] = 2;
            triangleIndices[2] = 1;
            triangleIndices[3] = 1;
            triangleIndices[4] = 2;
            triangleIndices[5] = 3;
            
            var bounds = new Bounds(new Vector3(0.5f, 0.5f), new Vector3(1f, 1f));
            
            meshData.subMeshCount = 1;
            meshData.SetSubMesh(0, new SubMeshDescriptor(0, triangleIndexCount)
            {
                bounds = bounds,
                vertexCount = vertexCount,
            }, MeshUpdateFlags.DontRecalculateBounds);
            
            var mesh = new Mesh()
            {
                bounds = bounds,
                name = "Procedural Mesh Advanced Multi Stream"
            };

            Mesh.ApplyAndDisposeWritableMeshData(meshDataArray, mesh);
            GetComponent<MeshFilter>().mesh = mesh;
        }

        private void OnValidate()
        {
            OnEnable();
        }
    }
}