using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;

public readonly partial struct AlgoContainerAspect : IAspect
{
    public readonly Entity entity;
    public readonly RefRO<AlgoContainer> algoContainer;
    private readonly LocalTransform localTransform;
    public void ResizeContainer(ref float2 currentSize)
    {
        ContainerMode mode = algoContainer.ValueRO.containerMode;
        if (currentSize.x == GameManager.GM.panel2DSize.x && currentSize.y == GameManager.GM.panel2DSize.y)
            return;
        currentSize = new int2(GameManager.GM.panel2DSize.x, GameManager.GM.panel2DSize.y);
        // localTransform.Position = new float3().zero;
    }
}
