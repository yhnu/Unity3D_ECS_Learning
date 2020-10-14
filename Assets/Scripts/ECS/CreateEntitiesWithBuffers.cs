using Unity.Entities;
using Unity.Jobs;

//public struct DataToSpawn:IComponentData
//{
//    public int EntityCount;
//    public int ElementCount;
//}

public struct MyBufferElement : IBufferElementData
{
    public int Value;
}

[DisableAutoCreation]
public class CreateEntitiesWithBuffers : SystemBase
{
    // A command buffer system executes command buffers in its own OnUpdate
    public EntityCommandBufferSystem cbs;

    protected override void OnCreate()
    {
        // Get the command buffer system
        cbs = World.DefaultGameObjectInjectionWorld.GetExistingSystem<EndSimulationEntityCommandBufferSystem>();
    }

    protected override void OnUpdate()
    {
        // The command buffer to record commands,
        // which are executed by the command buffer system later in the frame
        EntityCommandBuffer.ParallelWriter commandBuffer = cbs.CreateCommandBuffer().AsParallelWriter();

        //The DataToSpawn component tells us how many entities with buffers to create
        Entities.ForEach((Entity spawnEntity, int entityInQueryIndex, in DataToSpawn data) =>
        {
            for (int e = 0; e < data.EntityCount; e++)
            {
                //Create a new entity for the command buffer
                Entity newEntity = commandBuffer.CreateEntity(entityInQueryIndex);

                //Create the dynamic buffer and add it to the new entity
                DynamicBuffer<MyBufferElement> buffer =
                    commandBuffer.AddBuffer<MyBufferElement>(entityInQueryIndex, newEntity);

                //Reinterpret to plain int buffer
                DynamicBuffer<int> intBuffer = buffer.Reinterpret<int>();

                //Optionally, populate the dynamic buffer
                for (int j = 0; j < data.ElementCount; j++)
                {
                    intBuffer.Add(j);
                }
            }

            //Destroy the DataToSpawn entity since it has done its job
            commandBuffer.DestroyEntity(entityInQueryIndex, spawnEntity);
        }).ScheduleParallel();

        cbs.AddJobHandleForProducer(this.Dependency);
    }
}