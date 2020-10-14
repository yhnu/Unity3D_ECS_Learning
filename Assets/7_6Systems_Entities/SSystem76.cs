using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Jobs;
using Unity.Mathematics;
using Unity.Transforms;

[DisableAutoCreation]
public class SSystem76 : SystemBase
{
    //protected override void OnCreate()
    //{
    //
    //}
    EntityQuery query;
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

        var count = query.CalculateChunkCount();

        /*
        float sum = 0;

        Entities.WithReadOnly(typeof(CData76))
            .ForEach((int entityInQueryIndex, in DynamicBuffer<CData76> data) =>
            {

                for (int i = 0; i < data.Length; i++)
                {
                   sum += data[i].Value;
                }
                // Implement the work to perform for each entity here.
                // You should only access data that is local or that is a
                // field on this job. Note that the 'rotation' parameter is
                // marked as 'in', which means it cannot be modified,
                // but allows this job to run in parallel with other jobs
                // that want to read Rotation component data.
                // For example,
                //     translation.Value += math.mul(rotation.Value, new float3(0, 0, 1)) * deltaTime;
            }).WithName("OnUpdate")
        .Run();
        */

        /*
        NativeArray<float> sumArray = new NativeArray<float>(count, Allocator.TempJob);

        Entities.WithReadOnly(typeof(CData76))
            .ForEach((int entityInQueryIndex, in DynamicBuffer<CData76> data) => {
            
                for(int i=0;i < data.Length; i++)
                {
                    sumArray[i] += data[i].Value;
                }            
            // Implement the work to perform for each entity here.
            // You should only access data that is local or that is a
            // field on this job. Note that the 'rotation' parameter is
            // marked as 'in', which means it cannot be modified,
            // but allows this job to run in parallel with other jobs
            // that want to read Rotation component data.
            // For example,
            //     translation.Value += math.mul(rotation.Value, new float3(0, 0, 1)) * deltaTime;
        }).WithName("OnUpdate")
        .Schedule();
        */
    }
}

/*
 * Capturing variables
 * 
 * //使用Run方法调度job
 * You can capture local variables for Entities.ForEach lambda functions. 
 
 * 
 * //使用Schedule 方法调用job, 数据访问有一些限制. 
 * When you execute the function using a job (by calling one of the Schedule functions instead of Run) there are some restrictions on the captured variables and how you use them:
   
    1. Only native containers and blittable types can be captured. 只支持值类型
    2. A job can only write to captured variables that are native containers. (To “return” a single value, create a native array with one element.) 如果你向写入数据,需要使用native containers
 * 
 */
