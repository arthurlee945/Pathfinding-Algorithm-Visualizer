using System.Collections;
using System.Collections.Generic;
using Unity.Entities;
using UnityEngine;

public struct ZoneTag : IComponentData
{
}
public class ZoneTagAuthoring : MonoBehaviour
{
    class Baker : Baker<ZoneTagAuthoring>
    {
        public override void Bake(ZoneTagAuthoring authoring)
        {
            Entity entity = GetEntity(TransformUsageFlags.None);
            AddComponent(entity, new ZoneTag());
        }
    }
}
