namespace ProceduralMeshes.Script.Common
{
    using UnityEngine;

    [RequireComponent(typeof(MeshFilter), typeof(MeshRenderer))]
    public class ProceduralMesh : MonoBehaviour
    {
        [SerializeField, Range(1, 100)]
        private int resolution = 1;
        private Mesh _mesh;

        private static MeshJobScheduleDelegate[] jobs =
        {
            MeshJob<SquareGrid, MultiStream>.ScheduleParallel,
            MeshJob<SharedSquareGrid, SingleStream>.ScheduleParallel,
            MeshJob<SharedTriangleGrid, SingleStream>.ScheduleParallel,
            MeshJob<PointyHexagonGrid, SingleStream>.ScheduleParallel,
            MeshJob<FlatHexagonGrid, SingleStream>.ScheduleParallel
        };

        public enum MeshType
        {
            SquareGrid,
            SharedSquareGrid,
            SharedTriangleGrid,
            PointyHexagonGrid,
            FlatHexagonGrid
        }
        
        [SerializeField]
        MeshType meshType;

        private void Awake()
        {
            _mesh = new Mesh()
            {
                name = "Procedural Mesh"
            };
            GetComponent<MeshFilter>().mesh = _mesh;
        }

        private void GenerateMesh()
        {
            Mesh.MeshDataArray meshDataArray = Mesh.AllocateWritableMeshData(1);
            Mesh.MeshData meshData = meshDataArray[0];
            jobs[(int)meshType](_mesh, meshData, resolution, default).Complete();
            Mesh.ApplyAndDisposeWritableMeshData(meshDataArray, _mesh);
        }

        private void Update()
        {
            GenerateMesh();
            // enabled = false;
        }

        private void OnValidate()
        {
            // enabled = true;
        }
    }
}