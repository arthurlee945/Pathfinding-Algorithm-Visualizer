using System.Collections;
using System.Collections.Generic;
using Unity.Entities;
using Unity.Mathematics;
using UnityEngine;

public struct Zone3D : IComponentData
{
    public int3 coordinates;
    public bool isWalkable, isExplored, isPath;
    Entity connectedTo;
}
public class Zone3DAuthoring : MonoBehaviour
{
    public int3 coordinates;
    class Baker : Baker<Zone3DAuthoring>
    {
        public override void Bake(Zone3DAuthoring authoring)
        {
            Entity entity = GetEntity(TransformUsageFlags.Renderable);
            AddComponent(entity, new Zone3D
            {
                coordinates = authoring.coordinates,
                isWalkable = true,
            });
        }
    }
}
