using Unity.Entities;
using Unity.Jobs;
using Unity.Collections;

public struct StateComponentData : ISystemStateComponentData
{
    public int State;
}
