namespace VerticesAndTriangles
{
    using UnityEngine;
    [RequireComponent(typeof(MeshFilter), typeof(MeshRenderer))]
    public class SimpleProceduralMesh : MonoBehaviour
    {
        private void OnEnable()
        {
            var mesh = new Mesh
            {
                name = "Procedural Mesh"
            };
            
            mesh.vertices = new Vector3[]
            {
                Vector3.zero, Vector3.right, Vector3.up, new Vector3(1, 1)
            };

            mesh.triangles = new int[]
            {
                0, 2, 1, 1, 2, 3
            };

            mesh.normals = new Vector3[]
            {
                Vector3.back, Vector3.back, Vector3.back, Vector3.back
            };

            mesh.uv = new Vector2[]
            {
                Vector2.zero, Vector2.right, Vector2.up, Vector2.one
            };
            
            mesh.tangents = new Vector4[]
            {
                new Vector4(1, 0, 0, -1),
                new Vector4(1, 0, 0, -1),
                new Vector4(1, 0, 0, -1),
                new Vector4(1, 0, 0, -1),
            };

            GetComponent<MeshFilter>().mesh = mesh;

        }

        private void OnValidate()
        {
            OnEnable();
        }
    }
}