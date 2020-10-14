using UnityEngine;
using Unity.Collections;
using Unity.Entities;
using Unity.Jobs;

public class DynamicBufferJobSystem : SystemBase
{
    private EntityQuery query;

    protected override void OnCreate()
    {
        //Create a query to find all entities with a dynamic buffer
        // containing MyBufferElement
        EntityQueryDesc queryDescription = new EntityQueryDesc();
        queryDescription.All = new[] { ComponentType.ReadWrite<MyBufferElement>() };
        query = GetEntityQuery(queryDescription);
    }

    public struct BuffersInChunks : IJobChunk
    {
        //The data type and safety object
        public BufferTypeHandle<MyBufferElement> BufferTypeHandle;

        //An array to hold the output, intermediate sums
        public NativeArray<int> sums;

        public void Execute(ArchetypeChunk chunk,
            int chunkIndex,
            int firstEntityIndex)
        {
            //A buffer accessor is a list of all the buffers in the chunk
            BufferAccessor<MyBufferElement> buffers
                = chunk.GetBufferAccessor(BufferTypeHandle);

            for (int c = 0; c < chunk.Count; c++)
            {
                //An individual dynamic buffer for a specific entity
                DynamicBuffer<MyBufferElement> buffer = buffers[c];
                for (int i = 0; i < buffer.Length; i++)
                {
                    sums[chunkIndex] += buffer[i].Value;
                }
            }
        }
    }

    //Sums the intermediate results into the final total
    public struct SumResult : IJob
    {
        [DeallocateOnJobCompletion] public NativeArray<int> sums;
        public NativeArray<int> result;
        public void Execute()
        {
            for (int i = 0; i < sums.Length; i++)
            {
                result[0] += sums[i];
            }
        }
    }

    protected override void OnUpdate()
    {
        //Create a native array to hold the intermediate sums
        int chunksInQuery = query.CalculateChunkCount();
        NativeArray<int> intermediateSums = new NativeArray<int>(chunksInQuery, Allocator.TempJob);

        //Schedule the first job to add all the buffer elements
        BuffersInChunks bufferJob = new BuffersInChunks();
        bufferJob.BufferTypeHandle = GetBufferTypeHandle<MyBufferElement>();
        bufferJob.sums = intermediateSums;
        this.Dependency = bufferJob.ScheduleParallel(query, this.Dependency);

        //Schedule the second job, which depends on the first
        SumResult finalSumJob = new SumResult();
        finalSumJob.sums = intermediateSums;
        NativeArray<int> finalSum = new NativeArray<int>(1, Allocator.TempJob);
        finalSumJob.result = finalSum;
        this.Dependency = finalSumJob.Schedule(this.Dependency);

        this.CompleteDependency();
        Debug.Log("Sum of all buffers: " + finalSum[0]);        
        finalSum.Dispose();
    }
}

