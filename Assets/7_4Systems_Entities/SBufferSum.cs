using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Jobs;
using Unity.Mathematics;
using Unity.Transforms;

//[DisableAutoCreation]
public class SBufferSum : SystemBase
{
    //protected override void OnCreate()
    //{
    //
    //}
    private EntityQuery query;

    protected override void OnUpdate()
    {
        // Assign values to local variables captured in your job here, so that it has
        // everything it needs to do its work when it runs later.
        // For example,
        //     float deltaTime = Time.DeltaTime;

        // This declares a new kind of job, which is a unit of work to do.
        // The job is declared as an Entities.ForEach with the target components as parameters,
        // meaning it will process all entities in the world that have both
        // Translation and Rotation components. Change it to process the component
        // types you want.
        int entitiesInQuery = query.CalculateEntityCount();


        //Create a native array to hold the intermediate sums
        //(one element per entity)
        NativeArray<int> intermediateSums = new NativeArray<int>(entitiesInQuery, Allocator.TempJob);

        Entities.WithStoreEntityQueryInField(ref query)
            .ForEach((int entityInQueryIndex, in DynamicBuffer<CIntBufferData> buffer) => {
            for (int i = 0; i < buffer.Length; i++)
            {
                intermediateSums[entityInQueryIndex] += buffer[i].Value;
            }
            
        }).WithName("IntermediateSums")
        .Schedule();
        
        
        Job.WithCode(() =>
        {
            int result = 0;
            for (int i = 0; i < intermediateSums.Length; i++)
            {
                result += intermediateSums[i];
            }
            UnityEngine.Debug.Log(result);
        })
        .WithDisposeOnCompletion(intermediateSums)
        .WithName("FinalSum")  //给对应的Job取一个名字,方便调试器识别
        .WithoutBurst()        //必不可少, 否则Debug.Log不能兼容
        .Schedule();           //单子线程执行
    }

    //Q: 里面有两个Schedule,是一起执行?
    //A: 不是一起执行的
}
