namespace ProceduralMeshes.Script.Common
{
    using Unity.Mathematics;
    public struct TriangleUInt16
    {
        public ushort a, b, c; 
        public static implicit operator TriangleUInt16 (int3 t) => new TriangleUInt16 { a = (ushort)t.x, b = (ushort)t.y, c = (ushort)t.z };
    }
}