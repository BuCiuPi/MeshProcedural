namespace ProceduralMeshes.Script.Common
{
    using System;
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
            MeshJob<FlatHexagonGrid, SingleStream>.ScheduleParallel,
            MeshJob<UVSphere, SingleStream>.ScheduleParallel
        };

        public enum MeshType
        {
            SquareGrid,
            SharedSquareGrid,
            SharedTriangleGrid,
            PointyHexagonGrid,
            FlatHexagonGrid,
            UVSphere
        }


        [SerializeField]
        MeshType meshType;

        private Vector3[] _vertices;
        private Vector3[] _normals;
        private Vector4[] _tangents;

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

            _vertices = null;
            _normals = null;
            _tangents = null;
        }

        private void OnValidate()
        {
            // enabled = true;
        }

        [Flags]
        public enum GizmoMode
        {
            Nothing = 0,
            Vertices = 1,
            Normals = 0b10,
            Tangents = 0b100,
        }

        [SerializeField]
        private GizmoMode gizmoMode;

        private void OnDrawGizmos()
        {
            if (gizmoMode == GizmoMode.Nothing || _mesh == null)
            {
                return;
            }

            bool isDrawVertices = (gizmoMode & GizmoMode.Vertices) != 0;
            bool isDrawNormals = (gizmoMode & GizmoMode.Normals) != 0;
            bool isDrawTangents = (gizmoMode & GizmoMode.Tangents) != 0;

            if (_vertices == null)
            {
                _vertices = _mesh.vertices;
            }
            if (isDrawNormals && _normals == null)
            {
                _normals = _mesh.normals;
            }
            if (isDrawTangents && _tangents == null)
            {
                _tangents = _mesh.tangents;
            }

            Transform t = transform;
            for (int index = 0; index < _vertices.Length; index++)
            {
                var position = t.TransformPoint(_vertices[index]);
                if (isDrawVertices)
                {
                    Gizmos.color = Color.cyan;
                    Gizmos.DrawSphere(position, 0.02f);
                }
                if (isDrawNormals)
                {
                    Gizmos.color = Color.green;
                    Gizmos.DrawRay(position, t.TransformDirection(_normals[index]) * .2f);
                }
                if (isDrawTangents)
                {
                    Gizmos.color = Color.red;
                    Gizmos.DrawRay(position, t.TransformDirection(_tangents[index].normalized) * .2f);
                }
            }
        }
    }
}