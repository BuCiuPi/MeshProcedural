namespace ProceduralMeshes.Script.Common
{
    public interface IMeshGenerator
    {
        int VertexCount { get; }
        int IndexCount { get; }
        
        int JobLength { get; }
        
        void Execute<S>(int i, S stream) where S : struct, IMeshStreams; 
    }
}