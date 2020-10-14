using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Jobs;
using Unity.Mathematics;
using Unity.Transforms;

//[DisableAutoCreation]
public class SManualDependency : SystemBase
{
    //protected override void OnCreate()
    //{
    //
    //}

    protected override void OnUpdate()
    {
        JobHandle One = Entities.ForEach((ref CJobAData d1) =>
        {
            for (int i = 0; i < 3000; i++)
            {
                float c = math.sin(i);
            }
        })
        .WithName("ForEach_Job_1")
        .Schedule(this.Dependency); //Schedule 需要传入对应的依赖,才会返回Job Handle

        JobHandle Two = Entities.ForEach((ref CJobBData d2) =>
        {
            for (int i = 0; i < 3000; i++)
            {
                float c = math.sin(i);
            }
        })
        .WithName("ForEach_Job_2") 
        .Schedule(this.Dependency); //Schedule 需要传入对应的依赖,才会返回Job Handle


        JobHandle intermediateDependencies = JobHandle.CombineDependencies(One, Two);

        NativeArray<int> result = new NativeArray<int>(1, Allocator.TempJob);

        JobHandle finalDependency = Job
            .WithName("Job_Three")
            .WithDisposeOnCompletion(result)
            .WithCode(() =>
            {
                /*...*/
                result[0] = 1;
            })
            .Schedule(intermediateDependencies);

        this.Dependency = finalDependency;

    }
}

/**
 * 隐式序列依赖
 * 
 * The function schedules three jobs, each depending on the previous one
 * 
 * 
 */
