using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Jobs;
using Unity.Mathematics;
using Unity.Transforms;

[DisableAutoCreation]

public class SRandomSumJob : SystemBase
{
    //protected override void OnCreate()
    //{
    //
    //}
    uint seed = 1;

    protected override void OnUpdate()
    {
        Random r = new Random(seed++);

        NativeArray<float> randomNumbers = new NativeArray<float>(100000, Allocator.TempJob);

        Job.WithCode(() =>
        {
            for (int i = 0; i < randomNumbers.Length; i++)
            {
                randomNumbers[i] = r.NextFloat();
            }
        })
        .WithName("Random_Generator_Job") //不能有空格,使用下划线代替
        .Schedule();

        NativeArray<float> result = new NativeArray<float>(1, Allocator.TempJob);

        Job.WithCode(() =>
        {
            for (int i = 0; i < randomNumbers.Length; i++)
            {
                result[0] += randomNumbers[i];
            }
            UnityEngine.Debug.Log("The sum of " + randomNumbers.Length + " numbers is " + result[0]);
        })
        .WithoutBurst()
        .WithName("Sum_Job")
        .WithDisposeOnCompletion(randomNumbers) //
        .WithDisposeOnCompletion(result)         //
        .Schedule();

        //this.CompleteDependency();    // 等待JOB完成
        
        //UnityEngine.Debug.Log("The sum of " + randomNumbers.Length + " numbers is " + result[0]);

        //randomNumbers.Dispose();    //必须要有否则4帧后会认为是内存泄漏
        //result.Dispose();        
    }
}

/**
 * 根据上面的案例, 我们
 */