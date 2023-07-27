
using Unity.Entities;
using Unity.Mathematics;

public partial class AlgoContainerSystem : SystemBase
{
    int2 currentSize2D = new int2(50, 50);
    protected override void OnUpdate()
    {
        ResizeContainer();
    }
    void ResizeContainer()
    {
        if (PathFinder.Instance.IsRunning) return;
        foreach (AlgoContainerAspect algoContainerAspect in SystemAPI.Query<AlgoContainerAspect>())
        {
            algoContainerAspect.ResizeContainer(ref currentSize2D);
        }
    }
}
