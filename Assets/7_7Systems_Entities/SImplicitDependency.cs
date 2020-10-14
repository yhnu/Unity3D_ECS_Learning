using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Jobs;
using Unity.Mathematics;
using Unity.Transforms;

[DisableAutoCreation]  //测试时需要禁用掉
public class SImplicitDependency : SystemBase
{
    //protected override void OnCreate()
    //{
    //
    //}

    protected override void OnUpdate()
    {
        Entities.ForEach((ref CJobAData d1) =>
        {
            for(int i=0; i < 3000; i++)
            {
                float c = math.sin(i);
            }
        })
        .WithName("ForEach_Job_1")
        .Schedule();

        Entities.ForEach((ref CJobBData d2) =>
        {
            for (int i = 0; i < 3000; i++)
            {
                float c = math.sin(i);
            }
        })
        .WithName("ForEach_Job_2")
        .Schedule();

        Entities.ForEach((ref CJobCData d3) =>
        {
            for (int i = 0; i < 3000; i++)
            {
                float c = math.sin(i);
            }
        })
        .WithName("ForEach_Job_3")
        .Schedule();        
    }
}

/**
 * 隐式序列依赖
 * 
 * The function schedules three jobs, each depending on the previous one
 * 
 * 
 */
