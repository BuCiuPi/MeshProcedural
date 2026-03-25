namespace ProceduralMeshes.Script.Common
{
    public struct SquareGrid : IMeshGenerator
    {

        public int VertexCount => 0;
        public int IndexCount => 0;
        public int JobLength => 0;
        
        public void Execute<S>(int i, S stream) where S : struct, IMeshStreams
        {
        }
    }
}