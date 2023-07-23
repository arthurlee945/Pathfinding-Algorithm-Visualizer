
using Unity.Entities;
using Unity.Mathematics;


public partial class AlgoContainerSystem : SystemBase
{
    int2 currentSize2D = new int2(100, 100);
    protected override void OnCreate()
    {
        ResizeContainer();
    }
    protected override void OnUpdate()
    {
        ResizeContainer();
    }
    void ResizeContainer()
    {
        foreach (AlgoContainerAspect algoContainerAspect in SystemAPI.Query<AlgoContainerAspect>())
        {
            algoContainerAspect.ResizeContainer(ref currentSize2D);
        }
    }
}
