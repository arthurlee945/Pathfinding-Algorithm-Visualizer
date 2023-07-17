using System.Collections;
using System.Collections.Generic;
using Unity.Entities;
using Unity.Mathematics;
using UnityEngine;

public struct Zone : IComponentData
{
    public int2 coordinates;
}
public class ZoneAuthoring : MonoBehaviour
{
    class Baker : Baker<ZoneAuthoring>
    {
        public override void Bake(ZoneAuthoring authoring)
        {
            Entity entity = GetEntity(TransformUsageFlags.None);
        }
    }
}
