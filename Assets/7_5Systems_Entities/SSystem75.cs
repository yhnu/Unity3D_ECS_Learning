using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Jobs;
using Unity.Mathematics;
using Unity.Transforms;

//[DisableAutoCreation]
public class SSystem75 : SystemBase
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

        int Count = query.CalculateChunkCount();



        Entities
            .WithStoreEntityQueryInField(ref query)
            .ForEach((Entity e, int entityInQueryIndex, int nativeThreadIndex, in CData75 data) => {

                UnityEngine.Debug.LogFormat("{0} {1}", entityInQueryIndex, nativeThreadIndex);
                // Implement the work to perform for each entity here.
                // You should only access data that is local or that is a
                // field on this job. Note that the 'rotation' parameter is
                // marked as 'in', which means it cannot be modified,
                // but allows this job to run in parallel with other jobs
                // that want to read Rotation component data.
                // For example,
                //     translation.Value += math.mul(rotation.Value, new float3(0, 0, 1)) * deltaTime;
            })
        .WithoutBurst()
        .WithName("OnUpdate")
        .Run();
    }
}

/*
 * In addition to components, you can pass the following special, named parameters to the Entities.ForEach lambda function, 
 * which are assigned values based on the entity the job is currently processing.
 * 
 * Entity entity                    //类型是Entity即可
 * 
 * int entityInQueryIndex
 * 
 * int nativeThreadIndex
 * 
 * 针对ForEach的lambda表达式,有一些特殊的参数,你可以传递.
 * 
 * Q: 为甚么都是entityInQueryIndex 和 nativeThreadIndex 是一样的数值
 */
