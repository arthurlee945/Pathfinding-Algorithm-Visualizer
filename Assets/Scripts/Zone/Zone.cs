using Unity.Entities;
using Unity.Mathematics;
using UnityEngine;

public struct ZoneComponent : IComponentData
{
    public int3 coordinates;
    public bool isWalkable, isExplored, isPath;
    Entity connectedTo;
}
public class Zone : MonoBehaviour
{
    public int3 coordinates;
    class Baker : Baker<Zone>
    {
        public override void Bake(Zone authoring)
        {
            Entity entity = GetEntity(TransformUsageFlags.Renderable);
            AddComponent(entity, new ZoneComponent
            {
                coordinates = authoring.coordinates,
                isWalkable = true,
            });
        }
    }
}
