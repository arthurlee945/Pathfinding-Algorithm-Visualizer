using System.Collections;
using System.Collections.Generic;
using Unity.Entities;
using UnityEngine;


public struct ZoneManager : IComponentData
{
    public Entity zone2DPrefab;
    public Entity zone3DPrefab;
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
            });
        }
    }
}
