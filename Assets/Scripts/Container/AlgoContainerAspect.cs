using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;

public readonly partial struct AlgoContainerAspect : IAspect
{
    public readonly Entity entity;
    public readonly RefRO<AlgoContainerComponent> algoContainer;
    private readonly RefRW<LocalToWorld> transform;

    public void ResizeContainer(ref int2 currentSize)
    {
        if (currentSize.x == GameManager.GM.panelSize.x && currentSize.y == GameManager.GM.panelSize.y)
            return;
        currentSize = new int2(GameManager.GM.panelSize.x, GameManager.GM.panelSize.y);
        float4 r0 = new float4(currentSize.x, 0, 0, 0);
        float4 r1 = new float4(0, 0.2f, 0, 0);
        float4 r2 = new float4(0, 0, currentSize.y, 0);
        float4 r3 = new float4(currentSize.x / 2, 0.1f, currentSize.y / 2, 1);
        float4x4 newScaledSize = new float4x4(r0, r1, r2, r3);
        transform.ValueRW.Value = newScaledSize;
    }

    // public void ResizeContainer(ref int3 currentSize)
    // {
    //     ContainerMode mode = algoContainer.ValueRO.containerMode;
    //     if (currentSize.x == GameManager.GM.panel3DSize.x && currentSize.y == GameManager.GM.panel3DSize.y && currentSize.z == GameManager.GM.panel3DSize.z)
    //         return;
    //     currentSize = new int3(GameManager.GM.panel3DSize.x, GameManager.GM.panel3DSize.y, GameManager.GM.panel3DSize.z);

    //     float4 r0 = new float4(currentSize.x, 0, 0, 0);
    //     float4 r1 = new float4(0, currentSize.y, 0, 0);
    //     float4 r2 = new float4(0, 0, currentSize.z, 0);
    //     float4 r3 = new float4(currentSize.x / 2, currentSize.y / 2, currentSize.z / 2, 1);
    //     float4x4 newScaledSize = new float4x4(r0, r1, r2, r3);
    //     transform.ValueRW.Value = newScaledSize;
    // }
}
