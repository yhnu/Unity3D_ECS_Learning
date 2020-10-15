using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Jobs;
using Unity.Mathematics;
using Unity.Transforms;

//[DisableAutoCreation]
public class SRotationSpeedSystem : SystemBase
{
	private EntityQuery m_Query;
	protected override void OnCreate()
    {
		// method1 
        m_Query = GetEntityQuery(ComponentType.ReadOnly<Rotation>(), ComponentType.ReadOnly<CRotationSpeed>());
		
		// method2
		//var queryDescription = new EntityQueryDesc()
		//{
		//	None = new ComponentType[]
		//	{
		//		typeof(Static)
		//	},
		//	All = new ComponentType[]
		//	{
		//		ComponentType.ReadWrite<Rotation>(),	//ComponentType指定类型的同时标记是否对数据读写
		//		ComponentType.ReadOnly<RotationSpeed>()
		//	},
		//  Any = new ComponentType[]
		//  {
		//  }
		//};
		//m_Query = GetEntityQuery(queryDescription)
		
		// method3
		//var queryDescription0 = new EntityQueryDesc
		//{
		//	All = new ComponentType[] {typeof(Rotation)}
		//};
		//var queryDescription1 = new EntityQueryDesc
		//{
		//	All = new ComponentType[] {typeof(RotationSpeed)}
		//};
		//m_Query = GetEntityQuery(new EntityQueryDesc[] {queryDescription0, queryDescription1});
		
	
        //...
    }

    protected override void OnUpdate()
    {
        var job = new SRotationSpeedSystemJob()
        {
            RotationTypeHandle = GetComponentTypeHandle<Rotation>(false),
            RotationSpeedTypeHandle = GetComponentTypeHandle<CRotationSpeed>(true),
            DeltaTime = Time.DeltaTime
        };
        this.Dependency = job.ScheduleParallel(m_Query, this.Dependency);
    }
}

[BurstCompile]
struct SRotationSpeedSystemJob : IJobChunk
{
    public float DeltaTime;
    public ComponentTypeHandle<Rotation> RotationTypeHandle;                       //使用ComponentTypeHandle<T> 声明需要访问的组件类型
    [ReadOnly] public ComponentTypeHandle<CRotationSpeed> RotationSpeedTypeHandle;

    public void Execute(ArchetypeChunk chunk, int chunkIndex, int firstEntityIndex)
    {        
        var chunkRotations = chunk.GetNativeArray(RotationTypeHandle);
        var chunkRotationSpeeds = chunk.GetNativeArray(RotationSpeedTypeHandle);
        for (var i = 0; i < chunk.Count; i++)
        {
            var rotation = chunkRotations[i];
            var rotationSpeed = chunkRotationSpeeds[i];

            // Rotate something about its up vector at the speed given by RotationSpeed.
            chunkRotations[i] = new Rotation
            {
                Value = math.mul(math.normalize(rotation.Value), quaternion.AxisAngle(math.up(), rotationSpeed.Value * DeltaTime))
            };
        }
    }
}
