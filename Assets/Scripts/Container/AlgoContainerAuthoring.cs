using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Entities;

public struct AlgoContainer : IComponentData
{
    public ContainerMode containerMode;
}

public class AlgoContainerAuthoring : MonoBehaviour
{
    public ContainerMode containerMode;
    class Baker : Baker<AlgoContainerAuthoring>
    {
        public override void Bake(AlgoContainerAuthoring authoring)
        {
            Entity entity = GetEntity(TransformUsageFlags.Dynamic);
            AddComponent(entity, new AlgoContainer
            {
                containerMode = authoring.containerMode
            });
        }
    }
}
