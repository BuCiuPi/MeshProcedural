namespace ProceduralMeshes.Script.Common
{
    using Unity.Burst;
    using Unity.Collections;
    using Unity.Jobs;
    using UnityEngine;
    [BurstCompile(FloatPrecision.Standard, FloatMode.Fast, CompileSynchronously = true)]
    public struct MeshJob<G, S> : IJobFor 
        where G : struct, IMeshGenerator 
        where S : struct, IMeshStreams
    {
        private G _generator;
        
        [WriteOnly]
        private S _streams;
        public void Execute(int index)
        {
            _generator.Execute(index, _streams);
        }

        public static JobHandle ScheduleParallel(Mesh.MeshData meshData, JobHandle dependency)
        {
            var job = new MeshJob<G, S>();
            job._streams.Setup(meshData, job._generator.VertexCount, job._generator.IndexCount);
            return job.ScheduleParallel(job._generator.JobLength, 1, dependency);
        }
    }
}