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
        foreach (AlgoContainerAspect algoContaineraspect in SystemAPI.Query<AlgoContainerAspect>())
        {
            ContainerMode mode = algoContaineraspect.algoContainer.ValueRO.containerMode;
            // if ((mode == ContainerMode.Scene2D && currentSize2D.x == GameManager.GM.panel2DSize.x && currentSize2D.y == GameManager.GM.panel2DSize.y)
            // || (mode == ContainerMode.Scene3D && currentSize3D.x == GameManager.GM.panel3DSize.x && currentSize3D.y == GameManager.GM.panel3DSize.y && currentSize3D.z == GameManager.GM.panel3DSize.z))
            //     return;
            // if (mode == ContainerMode.Scene2D)
            // {
            //     currentSize2D = new int2(GameManager.GM.panel2DSize.x, GameManager.GM.panel2DSize.y);
            // }
            // else
            // {
            //     currentSize3D = new int3(GameManager.GM.panel3DSize.x, GameManager.GM.panel3DSize.y, GameManager.GM.panel3DSize.z);
            // }
            // transform.Position = new float3();
            // float4 c0 = new float4(mode == ContainerMode.Scene2D ? currentSize2D.x : currentSize3D.x, 0, 0, mode == ContainerMode.Scene2D ? currentSize2D.x / 2 : currentSize3D.x / 2);
            // float4 c1 = new float4(0f, mode == ContainerMode.Scene2D ? 0.2f : currentSize3D.y, 0, mode == ContainerMode.Scene2D ? 0.1f : currentSize3D.y);
            // float4 c2 = new float4(0, 0, mode == ContainerMode.Scene2D ? currentSize2D.y : currentSize3D.z, mode == ContainerMode.Scene2D ? currentSize2D.y / 2 : currentSize3D.z / 2);
            // float4 c3 = new float4(9, 0, 0, 1);
            // float4x4 newScaledSize = new float4x4(c0, c1, c2, c3);
            // localToWorld.Value = newScaledSize;
        }
    }
}
