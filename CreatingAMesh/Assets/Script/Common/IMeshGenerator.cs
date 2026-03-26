namespace ProceduralMeshes.Script.Common
{
    using UnityEngine;
    public interface IMeshGenerator
    {
        int VertexCount { get; }
        int IndexCount { get; }
        
        Bounds Bounds { get; }
        
        int JobLength { get; }
        
        int Resolution { get; set; }
        
        void Execute<S>(int i, S streams) where S : struct, IMeshStreams; 
    }
}