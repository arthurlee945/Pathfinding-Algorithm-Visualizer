using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Entities;

public struct AlgoContainerComponent : IComponentData
{
    public ContainerMode containerMode;
}

public class AlgoContainer : MonoBehaviour
{
    public ContainerMode containerMode;
    class Baker : Baker<AlgoContainer>
    {
        public override void Bake(AlgoContainer authoring)
        {
            Entity entity = GetEntity(TransformUsageFlags.Dynamic);
            AddComponent(entity, new AlgoContainerComponent
            {
                containerMode = authoring.containerMode
            });
        }
    }
}
