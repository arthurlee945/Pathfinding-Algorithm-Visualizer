using UnityEngine;
using Unity.Entities;

public struct AlgoContainerComponent : IComponentData { }

public class AlgoContainer : MonoBehaviour
{
    class Baker : Baker<AlgoContainer>
    {
        public override void Bake(AlgoContainer authoring)
        {
            Entity entity = GetEntity(TransformUsageFlags.Dynamic);
            AddComponent(entity, new AlgoContainerComponent());
        }
    }
}
