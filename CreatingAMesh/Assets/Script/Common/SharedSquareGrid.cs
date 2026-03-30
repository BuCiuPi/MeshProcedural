namespace ProceduralMeshes.Script.Common
{
    using UnityEngine;
    using static Unity.Mathematics.math;
    public struct SharedSquareGrid : IMeshGenerator
    {

        public int VertexCount => (Resolution + 1) * (Resolution + 1);
        public int IndexCount => 6 * Resolution * Resolution;
        public Bounds Bounds => new Bounds(float3(0.0), float3(1f, 0f, 1f));
        public int JobLength => Resolution + 1;
        public int Resolution { get; set; }

        public void Execute<S>(int u, S streams) where S : struct, IMeshStreams
        {
            int vi = (Resolution + 1) * u;
            int ti = 2 * Resolution * (u - 1);

            var vertex = new Vertex();
            vertex.normal.y = 1f;
            vertex.tangent.xw = float2(1f, -1f);

            vertex.position.x = -0.5f;
            vertex.position.z = (float)u / Resolution - 0.5f;
            vertex.texCoord0.y = (float)u / Resolution;
            streams.SetVertex(vi, vertex);

            vi += 1;

            for (int x = 1; x <= Resolution; x++, vi++, ti += 2)
            {
                vertex.position.x = (float)x / Resolution - 0.5f;
                vertex.texCoord0.x = (float)x / Resolution;
                streams.SetVertex(vi, vertex);

                if (u > 0)
                {
                    streams.SetTriangle(ti, vi + int3(-Resolution - 2, -1, -Resolution - 1));
                    streams.SetTriangle(ti + 1, vi + int3(-Resolution - 1, -1, 0));
                }
            }
        }
    }
}