using System.Collections;
using System.Collections.Generic;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;
using UnityEngine;

public partial class AlgoContainerSystem : SystemBase
{
    int2 currentSize2D = new int2(100, 100);
    int3 currentSize3D = new int3(100, 100, 100);
    protected override void OnCreate()
    {
        // base.OnCreate();

    }
    protected override void OnUpdate()
    {
        ResizeContainer();
    }
    void ResizeContainer()
    {
        foreach (AlgoContainerAspect algoContainerAspect in SystemAPI.Query<AlgoContainerAspect>())
        {
            ContainerMode mode = algoContainerAspect.algoContainer.ValueRO.containerMode;
            if (mode == ContainerMode.Scene2D)
            {
                algoContainerAspect.ResizeContainer(ref currentSize2D);
            }
            else if (mode == ContainerMode.Scene3D)
            {
                algoContainerAspect.ResizeContainer(ref currentSize3D);
            }
        }
    }
}
