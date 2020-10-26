# Unity3D_ECS_Learning

## 第一章: 整体概览(ECS Introduction)

[初学时初略整理的笔记,对应的目标是能够大概理解里面一些新的概念](https://github.com/yhnu/Unity3D_DOTS_Learning/blob/master/2020.09.23Unity3D_DOTS.md/2020.09.23Unity3D_DOTS.md)

例如:

1. DOTS是如何运作的?
2. Archetypes 是什么概念?
3. Memory Chunks 是什么概念?
4. Authoring 是什么意思?
5. World 又是个什么概念?



## 第二章: NativeContainer Allocator

[Native Container Allocator](https://github.com/yhnu/Unity3D_DOTS_Learning/blob/master/2020.10.26Unity3D_NativeMemoryAllocators.md/2020.10.26Unity3D_NativeMemoryAllocators.mdhttps://github.com/yhnu/Unity3D_DOTS_Learning/blob/master/2020.10.26Unity3D_NativeMemoryAllocators.md/2020.10.26Unity3D_NativeMemoryAllocators.md) 讲解原始内存分配器的的使用情况,以及优缺点



## 第三章: 详细使用Entities.ForEach进行System编写

[Entity.ForEach 详细使用说明](https://github.com/yhnu/Unity3D_DOTS_Learning/blob/master/2020.10.14Unity3D_DOTS_System_Entites_ForEach.md/2020.10.14Unity3D_DOTS_System_Entites_ForEach.md)

例如:

1. 局部变量获取
2. 访问修饰函数.WithReadOnly
3. 给Job去名称,方便调试
4. 如何返回变量
5. 局部变量销毁工作?
6. JOB的依赖管理

## 第四章: JOBWithCode

[JobWithCode 详细使用说明已经注意事项](https://github.com/yhnu/Unity3D_DOTS_Learning/blob/master/2020.10.14Unity3D_DOTS_System_JOBWithCode.md/2020.10.14Unity3D_DOTS_System_JOBWithCode.md)

例如:

1. XX.WithName("Random_Generator_Job") //不能有空格,使用下划线代替
2. Allocator不同传参对应不同的含义

![](_v_images/20201026141056539_27577.png)

3. CompleteDependency 依赖同步问题处理,以及原始内存分配和释放
   
   使用 Dispose
   
   使用 WithDisposeOnCompletion

## 第五章: IJobChunk

[Unity3D_DOTS_Learning/2020.10.15Unity3D_DOTS_System_IJobChunk.md at master · yhnu/Unity3D_DOTS_Learning · GitHub](https://github.com/yhnu/Unity3D_DOTS_Learning/blob/master/2020.10.15Unity3D_DOTS_System_IJobChunk.md/2020.10.15Unity3D_DOTS_System_IJobChunk.md)



使用IJOBChunk进行System编写,详细讲解了为什么使用IjobChunk进行,有什么优缺点,已经需要哪些预备基础知识.

1. 使用EntityQuery

2. 定义IJobChunk结构体

3. 编写好Execute执行函数

4. 使用changefilter进行高效过滤未修改的数据



## 第六章: Jobdependencies

https://github.com/yhnu/Unity3D_DOTS_Learning/blob/master/2020.10.16Unity3D_DOTS_System_Jobdependencies.md/2020.10.16Unity3D_DOTS_System_Jobdependencies.md



Jobdependencies 的设计目的是什么? 有哪几种方式供我们选择



## 第七章: Manualiteration

https://github.com/yhnu/Unity3D_DOTS_Learning/blob/master/2020.10.16Unity3D_DOTS_System_Manualiteration.md/2020.10.16Unity3D_DOTS_System_Manualiteration.md



手动对System迭代需要注意的事项以及相关示例(估计后期会被遗弃)



## 第八章: System的更新和执行顺序

https://github.com/yhnu/Unity3D_DOTS_Learning/blob/master/2020.10.16Unity3D_DOTS_System_UpdateOrder.md/2020.10.16Unity3D_DOTS_System_UpdateOrder.md



讲解如何手动指定System的更新顺序,其中包括: 默认的更新顺序, UpdateInGroup, UpdateBefore, UpdateAftaer



## 第九章: System的直接数据如何同步

[Unity3D_DOTS_Learning/2020.10.17Unity3D_DOTS_System_EntityCommandBuffers.md at master · yhnu/Unity3D_DOTS_Learning · GitHub](https://github.com/yhnu/Unity3D_DOTS_Learning/blob/master/2020.10.17Unity3D_DOTS_System_EntityCommandBuffers.md/2020.10.17Unity3D_DOTS_System_EntityCommandBuffers.md)



[Unity3D_DOTS_Learning/2020.10.19Unity3D_DOTS_System_Syncpoints.md at master · yhnu/Unity3D_DOTS_Learning · GitHub](https://github.com/yhnu/Unity3D_DOTS_Learning/blob/master/2020.10.19Unity3D_DOTS_System_Syncpoints.md/2020.10.19Unity3D_DOTS_System_Syncpoints.md)



讲解如何提高系统直接的数据操作,使用EntityCommandBuffers.
