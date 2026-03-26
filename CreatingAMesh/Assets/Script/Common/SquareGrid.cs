namespace ProceduralMeshes.Script.Common
{
    using Unity.Mathematics;
    using UnityEngine;
    using static Unity.Mathematics.math;
    public struct SquareGrid : IMeshGenerator
    {

        public int VertexCount => 4 * Resolution * Resolution;
        public int IndexCount => 6 * Resolution * Resolution;

        public Bounds Bounds => new Bounds(float3(0.0), float3(1f, 0f, 1f));
        public int JobLength => Resolution * Resolution;

        public int Resolution { get; set; }

        public void Execute<S>(int i, S streams) where S : struct, IMeshStreams
        {
            int vi = 4 * i;
            int ti = 2 * i;
            
            int z = i / Resolution;
            int x = i - Resolution * z;

            var coordinates = float4(x, x + 1f, z, z + 1f) / Resolution - 0.5f;

            var vertex = new Vertex();
            vertex.position.xz = coordinates.xz;
            vertex.normal.y = 1f;
            vertex.tangent.xw = float2(1f, -1f);
            streams.SetVertex(vi + 0, vertex);

            vertex.position.xz = coordinates.yz;
            vertex.texCoord0 = float2(1f, 0f);
            streams.SetVertex(vi + 1, vertex);

            vertex.position.xz = coordinates.xw;
            vertex.texCoord0 = float2(0f, 1f);
            streams.SetVertex(vi + 2, vertex);

            vertex.position.xz = coordinates.yw;
            vertex.texCoord0 = float2(1f);
            streams.SetVertex(vi + 3, vertex);

            streams.SetTriangle(ti + 0, vi + int3(0, 2, 1));
            streams.SetTriangle(ti + 1, vi + int3(1, 2, 3));
        }
    }
}