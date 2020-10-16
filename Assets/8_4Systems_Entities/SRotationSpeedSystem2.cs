using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Jobs;
using Unity.Mathematics;
using Unity.Transforms;

//[DisableAutoCreation]
public class SRotationSpeedSystem2 : SystemBase
{
    [BurstCompile]
    struct SRotationSpeedSystem2_Job : IJobParallelFor
    {
        [DeallocateOnJobCompletion] public NativeArray<ArchetypeChunk> Chunks;

        //原示例中 ArchetypeChunkComponentType 已经过时了
        public ComponentTypeHandle<CRotationData84> CRotationData84_Type;
        [ReadOnly] public ComponentTypeHandle<CSpeedData84> CSpeedData_Type;
        
        
        //public ComponentTypeHandle<COutput> COutput_Type;
        public float DeltaTime;

        public void Execute(int chunkIndex)
        {
            var chunk = Chunks[chunkIndex];
            var chunkRotation = chunk.GetNativeArray(CRotationData84_Type);
            var chunkSpeed = chunk.GetNativeArray(CSpeedData_Type);
            var instanceCount = chunk.Count;

            for (int i = 0; i < instanceCount; i++)
            {
                var rotation = chunkRotation[i];
                rotation.Value.value.x = 10;
                var speed = chunkSpeed[i];
                //rotation.Value = math.mul(math.normalize(rotation.Value), quaternion.AxisAngle(math.up(), speed.RadiansPerSecond * DeltaTime));
                chunkRotation[i] = rotation;
            }
        }
    }

    EntityQuery m_Query;   

   protected override void OnCreate()
   {
       var queryDesc = new EntityQueryDesc
       {
           All = new ComponentType[]{ typeof(CRotationData84), ComponentType.ReadOnly<CSpeedData84>() }
       };

       m_Query = GetEntityQuery(queryDesc);
   }

   protected override void OnUpdate()
   {
        //已经过时
        //var rotationType = GetArchetypeChunkComponentType<RotationQuaternion>();
        //var rotationSpeedType = GetArchetypeChunkComponentType<RotationSpeed>(true);
        
        var chunks = m_Query.CreateArchetypeChunkArray(Allocator.TempJob);

        var rotationsSpeedJob = new SRotationSpeedSystem2_Job
        {
            Chunks = chunks,
            DeltaTime = Time.DeltaTime,
            CRotationData84_Type = GetComponentTypeHandle<CRotationData84>(false),
            CSpeedData_Type = GetComponentTypeHandle<CSpeedData84>(true)            
        };
        this.Dependency = rotationsSpeedJob.Schedule(chunks.Length, 32, this.Dependency);
   }
}
