using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Entities;
using Unity.Jobs;

public class CreateMyBufferElement : MonoBehaviour
{
    World w;
    // Start is called before the first frame update
    void Start()
    {
        w = World.DefaultGameObjectInjectionWorld;

        var e = w.EntityManager.CreateEntity(typeof(MyBufferElement));

        

        var b = w.EntityManager.GetBuffer<MyBufferElement>(e);
        for(int i=0; i < 10; i++)
        {
            b.Add(new MyBufferElement { Value = 10 });
        }
    }    
}
