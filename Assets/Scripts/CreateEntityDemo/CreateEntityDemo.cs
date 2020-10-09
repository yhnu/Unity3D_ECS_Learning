using UnityEngine;
using Unity.Entities;

public class CreateEntityDemo : MonoBehaviour
{
    static World w;
    static Entity e2_origin;
    // Start is called before the first frame update
    void Start()
    {
        //从World获取EntityManager
        w = World.DefaultGameObjectInjectionWorld;

        /* 创建单个实体 */
        CreatEntity1();

        CreatEntity2();
        CreatEntity3();
        /* 创建多个实体 */

        CreatEntity4();
        CreatEntity5();

        /* 拷贝创建单个实体 */
        Instantiate1();

        /* 拷贝创建多个实体 */
        Instantiate2();
        Instantiate3();
        Instantiate4();        
    }
    public static void CreatEntity1()
    {
        //method1
        //CreateEntity(),然后自己组个添加对应的组件
        Entity e = w.EntityManager.CreateEntity();

        //组件的添加需要需要通过EntityManager
        w.EntityManager.AddComponent(e, typeof(EmptyData));
    }

    public static void CreatEntity2()
    {
        //method2
        //CreateEntity(ComponentType[]), 直接传入组件列表
        //Entity e2 = w.EntityManager.CreateEntity(typeof(EmptyData));
        e2_origin = w.EntityManager.CreateEntity(typeof(EmptyData));
    }

    public static void CreatEntity3()
    {
        var a3 = w.EntityManager.CreateArchetype(typeof(Transform), typeof(EmptyData));
        w.EntityManager.CreateEntity(a3);
    }

    public static void CreatEntity4()
    {
        //method3
        //CreateEntity(EntityArchetype, Int32, Allocator), 通过原型创建       
        var a3 = w.EntityManager.CreateArchetype(typeof(Transform), typeof(EmptyData));
        Unity.Collections.NativeArray<Entity> e3_array = w.EntityManager.CreateEntity(a3, 10, Unity.Collections.Allocator.Temp);
        e3_array.Dispose();
    }

    public static void CreatEntity5()
    {
        //method4
        var a4 = w.EntityManager.CreateArchetype(typeof(Transform));
        Unity.Collections.NativeArray<Entity> e4_array = new Unity.Collections.NativeArray<Entity>(10, Unity.Collections.Allocator.Temp);
        w.EntityManager.CreateEntity(a4, e4_array);
        e4_array.Dispose();
    }


    //public Entity Instantiate(Entity srcEntity)
    public static void Instantiate1()
    {
        w.EntityManager.Instantiate(e2_origin);
    }

    //public void Instantiate(Entity srcEntity, NativeArray<Entity> outputEntities)
    static Unity.Collections.NativeArray<Entity> e2_array_with_ins;
    public static void Instantiate2()
    {
        e2_array_with_ins = new Unity.Collections.NativeArray<Entity>(10, Unity.Collections.Allocator.Temp);
        w.EntityManager.Instantiate(e2_origin, e2_array_with_ins);
    }
    //public NativeArray<Entity> Instantiate(Entity srcEntity, int instanceCount, Allocator allocator)
    public static void Instantiate3()
    {
        var e2_array = w.EntityManager.Instantiate(e2_origin, 10, Unity.Collections.Allocator.Temp);
    }

    //Instantiate(NativeArray<Entity>, NativeArray<Entity>)
    public static void Instantiate4()
    {
        var e2_array_tmp = new Unity.Collections.NativeArray<Entity>(10, Unity.Collections.Allocator.Temp);
        w.EntityManager.Instantiate(e2_array_with_ins, e2_array_tmp);
    }
}

