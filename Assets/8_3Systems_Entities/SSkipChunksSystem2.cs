using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Jobs;
using Unity.Mathematics;
using Unity.Transforms;

//[DisableAutoCreation]
public class SSkipChunksSystem2 : SystemBase
{
	private EntityQuery m_Query;
	protected override void OnCreate()
    {
        // method1 
        m_Query = GetEntityQuery(ComponentType.ReadWrite<COutput>(), ComponentType.ReadOnly<CInputA>(), ComponentType.ReadOnly<CInputB>());        
    }

    protected override void OnUpdate()
    {
		var job = new SSkipChunksSystem2_Job()
        {
            CInputA_TypeHandle = GetComponentTypeHandle<CInputA>(true),         //true/fasle 用于指定是否可读
            CInputB_TypeHandle = GetComponentTypeHandle<CInputB>(true),
            COutput_TypeHandle = GetComponentTypeHandle<COutput>(false),
            LastSystemVersion = LastSystemVersion
        };

        job.Run(m_Query);
    }
}

[BurstCompile]
struct SSkipChunksSystem2_Job : IJobChunk
{
    [ReadOnly] public ComponentTypeHandle<CInputA> CInputA_TypeHandle;
    [ReadOnly] public ComponentTypeHandle<CInputB> CInputB_TypeHandle;
    public ComponentTypeHandle<COutput> COutput_TypeHandle;
    public uint LastSystemVersion;

    [BurstDiscard]
    public void Execute(ArchetypeChunk chunk, int chunkIndex, int firstEntityIndex)
    {
        var inputAChanged = chunk.DidChange(CInputA_TypeHandle, LastSystemVersion);
        var inputBChanged = chunk.DidChange(CInputB_TypeHandle, LastSystemVersion);
        
        // If neither component changed, skip the current chunk
        if (!(inputAChanged || inputBChanged))
            return;

        // 为了方便,去掉brust编译
        UnityEngine.Debug.Log("------------Change-------------");

        var InputA_Array = chunk.GetNativeArray(CInputA_TypeHandle);
        var InputB_Array = chunk.GetNativeArray(CInputB_TypeHandle);
        var OutPut_Array = chunk.GetNativeArray(COutput_TypeHandle);


        for (int i = 0; i < chunk.Count; i++)
        {
            OutPut_Array[i] = new COutput
            {
                Value = InputA_Array[i].Value + InputB_Array[i].Value
            };
        }
    }
}

/*
 * ArchetypeChunk.DidChange() in IJobChunk.Execute
 * 
 * 需要定义 public uint LastSystemVersion;
 * 
 */
