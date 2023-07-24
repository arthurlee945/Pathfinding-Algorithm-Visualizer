using Unity.Entities;
using Unity.Mathematics;
using Unity.Rendering;
using UnityEngine;

public struct ZoneComponent : IComponentData
{
    public int2 coordinates;
    public bool isWalkable, isExplored, isPath;
    Entity connectedTo;
}
public class Zone : MonoBehaviour
{
    public int2 coordinates;
    class Baker : Baker<Zone>
    {
        public override void Bake(Zone authoring)
        {
            Entity entity = GetEntity(TransformUsageFlags.Renderable);
            AddComponent(entity, new ZoneComponent
            {
                coordinates = authoring.coordinates,
            });
            AddComponent(entity, new URPMaterialPropertyBaseColor
            {
                Value = new float4(1, 1, 1, 1)
            });
        }
    }
}
