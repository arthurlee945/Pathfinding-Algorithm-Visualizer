using System.Collections;
using System.Collections.Generic;
using Unity.Entities;
using Unity.Mathematics;
using UnityEngine;

public struct Zone2D : IComponentData
{
    public int2 coordinates;
    public bool isWalkable, isExplored, isPath;
    Entity connectedTo;
}
public class Zone2DAuthoring : MonoBehaviour
{
    public int2 coordinates;
    class Baker : Baker<Zone2DAuthoring>
    {
        public override void Bake(Zone2DAuthoring authoring)
        {
            Entity entity = GetEntity(TransformUsageFlags.Renderable);
            AddComponent(entity, new Zone2D
            {
                coordinates = authoring.coordinates,
                isWalkable = true,
            });
        }
    }
}
