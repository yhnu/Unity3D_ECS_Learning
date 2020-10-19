using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Jobs;
using Unity.Mathematics;
using Unity.Transforms;

//[DisableAutoCreation]
public class SLifetimeSystem : SystemBase
{
    EndSimulationEntityCommandBufferSystem m_EndSimulationEcbSystem;

    protected override void OnCreate()
    {
        base.OnCreate();

        m_EndSimulationEcbSystem = World.GetExistingSystem<EndSimulationEntityCommandBufferSystem>();
    
    }

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

        var ecb = m_EndSimulationEcbSystem.CreateCommandBuffer().AsParallelWriter();        



        Entities.ForEach((Entity entity, int entityInQueryIndex, ref CLifeTime lifetime) =>
        {
            if (lifetime.Value == 0)
            {
                ecb.DestroyEntity(entityInQueryIndex, entity);
            }
            else
            {
                lifetime.Value -= 1;
            }

        }).WithName("OnUpdate")
        .ScheduleParallel();

        m_EndSimulationEcbSystem.AddJobHandleForProducer(this.Dependency);
    }
}
