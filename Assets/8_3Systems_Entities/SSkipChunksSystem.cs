using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Jobs;
using Unity.Mathematics;
using Unity.Transforms;

[DisableAutoCreation]
public class SSkipChunksSystem : SystemBase
{
	private EntityQuery m_Query;
	protected override void OnCreate()
    {
		// method1 
        m_Query = GetEntityQuery(ComponentType.ReadWrite<COutput>(), ComponentType.ReadOnly<CInputA>(), ComponentType.ReadOnly<CInputB>());
        
        m_Query.SetChangedVersionFilter(new ComponentType[]   //修改过滤,提高计算效率
        {
            ComponentType.ReadWrite<CInputA>(),
            ComponentType.ReadWrite<CInputB>()
        });		
    }

    protected override void OnUpdate()
    {
        var job = new SSkipChunksSystem_Job()
        {
            CInputA_TypeHandle = GetComponentTypeHandle<CInputA>(),
            CInputB_TypeHandle = GetComponentTypeHandle<CInputB>(),
            COutput_TypeHandle = GetComponentTypeHandle<COutput>()           
        };

        this.Dependency = job.ScheduleParallel(m_Query, this.Dependency);
    }
}

[BurstCompile]
struct SSkipChunksSystem_Job : IJobChunk
{

    [ReadOnly] public ComponentTypeHandle<CInputA> CInputA_TypeHandle;
    [ReadOnly] public ComponentTypeHandle<CInputB> CInputB_TypeHandle;
    public ComponentTypeHandle<COutput> COutput_TypeHandle;

    public void Execute(ArchetypeChunk chunk, int chunkIndex, int firstEntityIndex)
    {
        var InputA_Array = chunk.GetNativeArray(CInputA_TypeHandle);
        var InputB_Array = chunk.GetNativeArray(CInputB_TypeHandle);
        var OutPut_Array = chunk.GetNativeArray(COutput_TypeHandle);

        for (int i = 0; i < chunk.Count; i++)
        {
            OutPut_Array[i] = new COutput {
                Value = InputA_Array[i].Value + InputB_Array[i].Value
            };
        }
    }
}

/*
 * SetChangedVersionFilter in EntityQuery
 * 
 * 
 */
