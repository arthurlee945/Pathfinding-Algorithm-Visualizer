using System.Collections;
using System.Collections.Generic;
using Unity.Entities;
using Unity.Mathematics;
using UnityEngine;

public struct Zone : IComponentData
{
    public int2 coordinates;
    public bool isWalkable, isExplored, isPath;
    Entity connectedTo;
}
public class ZoneAuthoring : MonoBehaviour
{
    public int2 coordinates;
    class Baker : Baker<ZoneAuthoring>
    {
        public override void Bake(ZoneAuthoring authoring)
        {
            Entity entity = GetEntity(TransformUsageFlags.None);
            AddComponent(entity, new Zone
            {
                coordinates = authoring.coordinates,
                isWalkable = true,
            });
        }
    }
}
