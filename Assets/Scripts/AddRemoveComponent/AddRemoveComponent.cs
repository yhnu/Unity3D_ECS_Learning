using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Entities;
using Unity.Collections;
public class AddRemoveComponent : MonoBehaviour
{
    private World w;
    private Entity e;
    // Start is called before the first frame update
    void Start()
    {
        w = World.DefaultGameObjectInjectionWorld;

        e = w.EntityManager.CreateEntity(typeof(EmptyData), typeof(Transform));

        AddComponent1();
    }

    // AddComponent(Entity, ComponentType)
    void AddComponent1()
    {
        w.EntityManager.AddComponent(e, typeof(IntData));
    }
}
