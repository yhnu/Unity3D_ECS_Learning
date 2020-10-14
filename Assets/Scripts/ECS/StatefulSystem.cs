using Unity.Entities;
using Unity.Jobs;
using Unity.Collections;

[DisableAutoCreation]
public class StatefulSystem : SystemBase
{
    private EntityCommandBufferSystem ecbSource;
    private World w;
    protected override void OnCreate()
    {
        w = World.DefaultGameObjectInjectionWorld;
        //ecbSource = World.GetExistingSystem<EndSimulationEntityCommandBufferSystem>();
        ecbSource = w.GetExistingSystem<EndSimulationEntityCommandBufferSystem>();

        // Create some test entities
        // This runs on the main thread, but it is still faster to use a command buffer
        EntityCommandBuffer creationBuffer = new EntityCommandBuffer(Allocator.Temp);
        EntityArchetype archetype = EntityManager.CreateArchetype(typeof(GeneralPurposeComponentData));
        for (int i = 0; i < 10000; i++)
        {
            Entity newEntity = creationBuffer.CreateEntity(archetype);
            creationBuffer.SetComponent<GeneralPurposeComponentData>
            (
                newEntity,
                new GeneralPurposeComponentData() { Lifetime = i }
            );
        }
        //Execute the command buffer
        creationBuffer.Playback(EntityManager);
    }

    protected override void OnUpdate()
    {
        EntityCommandBuffer.ParallelWriter parallelWriterECB = ecbSource.CreateCommandBuffer().AsParallelWriter();

        // Entities with GeneralPurposeComponentA but not StateComponentB
        Entities
            .WithNone<StateComponentData>()
            .ForEach(
                (Entity entity, int entityInQueryIndex, in GeneralPurposeComponentData gpA) =>
                {
                    // Add an ISystemStateComponentData instance
                    parallelWriterECB.AddComponent<StateComponentData>
                        (
                            entityInQueryIndex,
                            entity,
                            new StateComponentData() { State = 1 }
                        );
                })
            .ScheduleParallel();
        ecbSource.AddJobHandleForProducer(this.Dependency);

        // Create new command buffer
        parallelWriterECB = ecbSource.CreateCommandBuffer().AsParallelWriter();

        // Entities with both GeneralPurposeComponentA and StateComponentB
        Entities
            .WithAll<StateComponentData>()
            .ForEach(
                (Entity entity,
                 int entityInQueryIndex,
                 ref GeneralPurposeComponentData gpA) =>
                {
                    // Process entity, in this case by decrementing the Lifetime count
                    gpA.Lifetime--;

                    // If out of time, destroy the entity
                    if (gpA.Lifetime <= 0)
                    {
                        parallelWriterECB.DestroyEntity(entityInQueryIndex, entity);
                    }
                })
            .ScheduleParallel();
        ecbSource.AddJobHandleForProducer(this.Dependency);

        // Create new command buffer
        parallelWriterECB = ecbSource.CreateCommandBuffer().AsParallelWriter();

        // Entities with StateComponentB but not GeneralPurposeComponentA
        Entities
            .WithAll<StateComponentData>()
            .WithNone<GeneralPurposeComponentData>()
            .ForEach(
                (Entity entity, int entityInQueryIndex) =>
                {
                    // This system is responsible for removing any ISystemStateComponentData instances it adds
                    // Otherwise, the entity is never truly destroyed.
                    parallelWriterECB.RemoveComponent<StateComponentData>(entityInQueryIndex, entity);
                })
            .ScheduleParallel();
        ecbSource.AddJobHandleForProducer(this.Dependency);

    }

    protected override void OnDestroy()
    {
        // Implement OnDestroy to cleanup any resources allocated by this system.
        // (This simplified example does not allocate any resources, so there is nothing to clean up.)
    }
}