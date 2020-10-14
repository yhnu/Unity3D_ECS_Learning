using Unity.Entities;

[GenerateAuthoringComponent]
public struct DataToSpawn : IComponentData
{
    public int EntityCount;
    public int ElementCount;
}
