namespace ProceduralMeshes.Script.Common
{
    using Unity.Mathematics;
    using static Unity.Mathematics.math;
    using UnityEngine;
    public struct CubeSphere : IMeshGenerator
    {
        struct Side
        {
            public float3 uvOrigin;
            public float3 uVector;
            public float3 vVector;
            public float3 normal;
            public float4 tangent;
        }
        
        public int VertexCount => 4 * Resolution * Resolution; 
        public int IndexCount => 6 * Resolution * Resolution; 
        public Bounds Bounds => new Bounds(Vector3.zero, new Vector3(2f, 2f, 2f));
        public int JobLength => Resolution * Resolution;
        public int Resolution { get; set; }
        
        public void Execute<S>(int u, S streams) where S : struct, IMeshStreams
        {
            var side = new Side()
            {
                uvOrigin = -1f,
                uVector = 2f * right(),
                vVector = 2f * up(),
                normal = back(),
                tangent = float4(1f, 0f, 0f, -1f) 
            };
            int vi = 4 * Resolution * u;
            int ti = 2 * Resolution * u;
            
            float3 uA = side.uvOrigin + side.uVector * u / Resolution;
            float3 uB = side.uvOrigin + side.uVector * (u + 1) / Resolution;
            
            for (int v = 0; v < Resolution; v++, vi += 4, ti += 2)
            {
                float3 pA = uA + side.vVector * v / Resolution;
                float3 pB = uB + side.vVector * v / Resolution;
                float3 pC = uA + side.vVector * (v + 1) / Resolution;
                float3 pD = uB + side.vVector * (v + 1) / Resolution;

                var vertex = new Vertex();
                vertex.normal = side.normal;
                vertex.tangent = side.tangent;

                vertex.position = pA;
                streams.SetVertex(vi, vertex);
                
                vertex.position = pB;
                vertex.texCoord0 = float2(1f, 0f);
                streams.SetVertex(vi + 1, vertex);
                
                vertex.position = pC;
                vertex.texCoord0 = float2(0f, 1f);
                streams.SetVertex(vi + 2, vertex);
                
                vertex.position = pD;
                vertex.texCoord0 = float2(1f);
                streams.SetVertex(vi + 3, vertex);
            }
        }
    }
}