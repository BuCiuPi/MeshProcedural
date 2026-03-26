namespace ProceduralMeshes.Script.Common
{
    using System.Runtime.CompilerServices;
    using System.Runtime.InteropServices;
    using Unity.Collections;
    using Unity.Collections.LowLevel.Unsafe;
    using Unity.Mathematics;
    using UnityEngine;
    using UnityEngine.Rendering;
    public struct SingleStream : IMeshStreams
    {
        [StructLayout(LayoutKind.Sequential)]
        struct Stream0
        {
            public float3 position;
            public float3 normal;
            public float4 tangent;
            public float2 texCoord0;
        }
        
        [NativeDisableContainerSafetyRestriction]
        private NativeArray<TriangleUInt16> _triangles;
        
        [NativeDisableContainerSafetyRestriction]
        private NativeArray<Stream0> _stream0;

        public void Setup(Mesh.MeshData meshData, Bounds bounds, int vertexCount, int indexCount)
        {
            var descriptor = new NativeArray<VertexAttributeDescriptor>(4, Allocator.Temp, NativeArrayOptions.UninitializedMemory);
            descriptor[0] = new VertexAttributeDescriptor(VertexAttribute.Position);
            descriptor[1] = new VertexAttributeDescriptor(VertexAttribute.Normal);
            descriptor[2] = new VertexAttributeDescriptor(VertexAttribute.Tangent, VertexAttributeFormat.Float32, 4);
            descriptor[3] = new VertexAttributeDescriptor(VertexAttribute.TexCoord0, VertexAttributeFormat.Float32, 2);
           
            meshData.SetVertexBufferParams(vertexCount, descriptor);
            descriptor.Dispose();
            
            meshData.SetIndexBufferParams(indexCount, IndexFormat.UInt16);
            
            meshData.subMeshCount = 1;
            meshData.SetSubMesh(0, new SubMeshDescriptor(0, indexCount)
            {
                bounds = bounds,
                vertexCount = vertexCount,
            }, MeshUpdateFlags.DontRecalculateBounds | MeshUpdateFlags.DontValidateIndices);
            
            _stream0 = meshData.GetVertexData<Stream0>();
            _triangles = meshData.GetIndexData<ushort>().Reinterpret<TriangleUInt16>(2);
        }
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void SetVertex(int index, Vertex data)
        {
            _stream0[index] = new Stream0()
            {
                position = data.position,
                normal = data.normal,
                tangent = data.tangent,
                texCoord0 = data.texCoord0
            };
        }
        public void SetTriangle(int index, int3 triangle)
        {
            _triangles[index] = triangle; 
        }
    }
}