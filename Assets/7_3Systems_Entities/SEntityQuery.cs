using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Jobs;
using Unity.Mathematics;
using Unity.Transforms;
using UnityEngine;

//[DisableAutoCreation]
public class SEntityQuery : SystemBase
{
    //protected override void OnCreate()
    //{
    //
    //}

    private EntityQuery query;
    protected override void OnUpdate()
    {
        int dataCount = query.CalculateEntityCount();
        NativeArray<float> dataSquared = new NativeArray<float>(dataCount, Allocator.Temp);
        
        //public static TDescription ForEach<TDescription, T0>(this TDescription description, [AllowDynamicValueAttribute] R<T0> codeToRun) where TDescription : struct, ISupportForEachWithUniversalDelegate;
        Entities
            .WithStoreEntityQueryInField(ref query)
            .ForEach((int entityInQueryIndex, in CData data) =>
            {
                
                dataSquared[entityInQueryIndex] = data.Value * data.Value;
            })
            .ScheduleParallel();

        Job.WithCode(() =>
            {
                //Use dataSquared array...
                var v = dataSquared[dataSquared.Length - 1];
                
            })
            .WithDisposeOnCompletion(dataSquared)
            .Schedule();
       
        //Debug.Log(dataSquared.Length);
    }



}
