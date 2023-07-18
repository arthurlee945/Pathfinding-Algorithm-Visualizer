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
    public int2 mode2DStart;
    public int2 mode2DEnd;
    public int3 mode3DStart;
    public int3 mode3DEnd;
    public NativeHashMap<int2, Entity> zones;
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
                zone2DPrefab = GetEntity(authoring.zone2DPrefab, TransformUsageFlags.None),
                zone3DPrefab = GetEntity(authoring.zone3DPrefab, TransformUsageFlags.None),
                mode2DStart = new int2(0, 0),
                mode2DEnd = new int2(100, 100),
                mode3DStart = new int3(0, 0, 0),
                mode3DEnd = new int3(100, 100, 100),
                zones = new NativeHashMap<int2, Entity>(),
            });
        }
    }
}
