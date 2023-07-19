using System.Collections;
using System.Collections.Generic;
using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;
using UnityEngine;


public struct ZoneManager : IComponentData
{
    public Entity zone2DPrefab;
    public Entity zone3DPrefab;
    public Entity startingZone, endZone;
    // public NativeHashMap<int2, Entity> zones;
    // public NativeList<Entity> test;
}
public class ZoneManagerAuthoring : MonoBehaviour
{
    public GameObject zone2DPrefab;
    public GameObject zone3DPrefab;
    class Baker : Baker<ZoneManagerAuthoring>
    {
        public override void Bake(ZoneManagerAuthoring authoring)
        {
            Entity entity = GetEntity(TransformUsageFlags.None);
            AddComponent(entity, new ZoneManager
            {
                zone2DPrefab = GetEntity(authoring.zone2DPrefab, TransformUsageFlags.Renderable),
                zone3DPrefab = GetEntity(authoring.zone3DPrefab, TransformUsageFlags.Renderable),
                // zones = new NativeHashMap<int2, Entity>(),
            });
        }
    }
}
